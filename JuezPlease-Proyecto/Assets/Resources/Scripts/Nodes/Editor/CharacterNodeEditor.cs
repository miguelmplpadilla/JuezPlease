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
        
        SetSprite(characterNode.characterData.eyes, r);
        SetSprite(characterNode.characterData.body, r);
        foreach (var sprite in GetTypeArm(characterNode.characterData.armLeft, characterNode.armAnimationToShowLeft))
            SetSprite(sprite, r);
        foreach (var sprite in GetTypeArm(characterNode.characterData.armRight, characterNode.armAnimationToShowRight))
            SetSprite(sprite, r);
        
        serializedObject.ApplyModifiedProperties();
    }

    private void SetSprite(Sprite sprite, Rect r)
    {
        if (sprite == null) return;
        
        Texture2D tex = sprite.texture;
        Rect texCoords = new Rect(
            sprite.rect.x / tex.width,
            sprite.rect.y / tex.height,
            sprite.rect.width / tex.width,
            sprite.rect.height / tex.height
        );

        GUI.DrawTextureWithTexCoords(r, tex, texCoords);
    }

    private List<Sprite> GetTypeArm(Arm arm, CharacterNode.TypeArmAnimation typeArmAnimation)
    {
        List<Sprite> sprites = new List<Sprite>();
        sprites.Add(arm.normalArm);
        
        switch (typeArmAnimation)
        {
            case CharacterNode.TypeArmAnimation.FIST:
                sprites.Add(arm.fistArm);
                return sprites;
            case CharacterNode.TypeArmAnimation.POINT:
                sprites.Clear();
                sprites.Add(arm.pointArm);
                return sprites;
        }

        return sprites;
    }
}