using Resources.Scripts.SceneCreators.LogBookCreator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcusedAcusatorCreator :  MonoBehaviour
{
    public LocalizableString acusedText;
    public LocalizableString acusatorText;
    
    public TextMeshProUGUI titleName;
    public Image photo;
    public TextMeshProUGUI name;
    public TextMeshProUGUI surnames;
    public TextMeshProUGUI gender;
    public TextMeshProUGUI birthdate;
    public TextMeshProUGUI profesion;
    public TextMeshProUGUI background;
    public Image fingerprint;

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
        
        AcusedAcusatorData data = (s.node as AcusedAcusatorPageNode).data;
        
        titleName.text = data.isAcused ? acusedText.value : acusatorText.value;
        
        photo.sprite = data.photo;
        name.text = data.litigantName.value;
        surnames.text = data.surnames.value;
        gender.text = data.gender == Gender.MALE
            ? GeneralTraductions.maleText.value
            : GeneralTraductions.femaleText.value;
        birthdate.text = data.birthDate;
        profesion.text = data.profession.value;
        background.text = data.criminalRecord.value;
        fingerprint.sprite = data.fingerPrint;
    }
}