using Resources.Scripts.SceneCreators.LogBookCreator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WitnessCreator : MonoBehaviour
{
    public Image photo;
    public TextMeshProUGUI name;
    public TextMeshProUGUI surnames;
    public TextMeshProUGUI gender;
    public TextMeshProUGUI description;
    public TextMeshProUGUI witnessNameText;
    
    private void Awake()
    {
        EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Register(
            new EventBinding<LogBookSceneCreatorListener.SetNodeDataEvent>(SetDataAcusedAcusator, gameObject));
    }
    
    private void OnDestroy()
    {
        EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Deregister(
            new EventBinding<LogBookSceneCreatorListener.SetNodeDataEvent>(SetDataAcusedAcusator, gameObject));
    }

    private void SetDataAcusedAcusator(LogBookSceneCreatorListener.SetNodeDataEvent s)
    {
        if (!s.obj.Equals(gameObject)) return;
        
        WitnessNode data = s.node as WitnessNode;
        
        photo.sprite = data.photo;
        name.text = data.witnessName.value;
        surnames.text = data.surnames.value;
        gender.text = data.gender == Gender.MALE
            ? GeneralTraductions.maleText.value
            : GeneralTraductions.femaleText.value;
        description.text = data.description.value;
        witnessNameText.text = data.witnessName.value + " " + data.surnames.value;
    }
}