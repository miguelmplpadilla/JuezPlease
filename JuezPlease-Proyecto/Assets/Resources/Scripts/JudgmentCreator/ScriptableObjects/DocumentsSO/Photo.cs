using UnityEngine;

[CreateAssetMenu(fileName = "Photo", menuName = "ScriptableObjects/Documents/Photo", order = 1)]
public class Photo : Document
{
    public Sprite imagePhoto;
    public Sprite backgroundImage;

    [SerializeField] public ExtraDataDocument extraDataDocument;
}