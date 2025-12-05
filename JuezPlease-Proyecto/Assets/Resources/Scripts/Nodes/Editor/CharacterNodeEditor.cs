using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;

[CustomNodeEditor(typeof(CharacterNode))]
public class CharacterNodeEditor : NodeEditor {
    private CharacterNode characterNode;

    public override void OnBodyGUI() {
        if (characterNode == null) characterNode = target as CharacterNode;
        serializedObject.Update();
        if (characterNode == null) return;
        
        if (GUILayout.Button("Restart Data"))
            characterNode.RestartData();
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // Dibuja los puertos
        NodeEditorGUILayout.PortField(target.GetInputPort("baseInput"));

        EditorGUILayout.Space();

        // Lista editable de sprites en el inspector del nodo
        SerializedProperty characterData = serializedObject.FindProperty("characterData");
        EditorGUILayout.PropertyField(characterData, true);
        SerializedProperty armAnimationToShowLeft = serializedObject.FindProperty("armAnimationToShowLeft");
        EditorGUILayout.PropertyField(armAnimationToShowLeft, true);
        SerializedProperty armAnimationToShowRight = serializedObject.FindProperty("armAnimationToShowRight");
        EditorGUILayout.PropertyField(armAnimationToShowRight, true);

        if (characterNode.IsDataVisualNotSeted())
        {
            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUILayout.Space();
        
        Rect r = GUILayoutUtility.GetRect(200, 150);
        
        SetSprite(characterNode.characterData.eyes, r, new Vector2(-0.005f, -0.005f), 1);
        SetSprite(characterNode.characterData.body, r, Vector2.zero, 1);
        
        if (characterNode.armAnimationToShowLeft != CharacterNode.TypeArmAnimation.POINT)
            SetSprite(characterNode.characterData.armLeft.normalArm, r, new Vector2(0.004f, 0), 1);
        if (characterNode.armAnimationToShowRight != CharacterNode.TypeArmAnimation.POINT)
            SetSprite(characterNode.characterData.armRight.normalArm, r, Vector2.zero, 1);

        SetSprite(GetTypeArm(characterNode.characterData.armLeft, characterNode.armAnimationToShowLeft), r,
            new Vector2(0.01f, 0.006f),1);

        SetSprite(GetTypeArm(characterNode.characterData.armRight, characterNode.armAnimationToShowRight), r,
            new Vector2(0.004f, 0),1);
        
        serializedObject.ApplyModifiedProperties();
    }

    private void SetSprite(Sprite sprite, Rect r, Vector2 sumCord, float scaleX)
    {
        if (sprite == null) return;
        
        Texture2D tex = sprite.texture;
        Rect texCoords = new Rect(
            (sprite.rect.x / tex.width)+sumCord.x,
            (sprite.rect.y / tex.height)+sumCord.y,
            sprite.rect.width / tex.width * scaleX,
            sprite.rect.height / tex.height
        );

        GUI.DrawTextureWithTexCoords(r, tex, texCoords);
    }

    private Sprite GetTypeArm(Arm arm, CharacterNode.TypeArmAnimation typeArmAnimation)
    {
        switch (typeArmAnimation)
        {
            //case CharacterNode.TypeArmAnimation.FIST:
                //return arm.fistArm;
            case CharacterNode.TypeArmAnimation.POINT:
                return arm.pointArm;
        }

        return null;
    }
}