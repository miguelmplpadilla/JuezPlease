using UnityEngine;

public class DocumentNode : ConectionsNode
{
    [SerializeField] private string guid;

    public string Guid => guid;

    protected override void Init() {
        base.Init();
        if (string.IsNullOrEmpty(guid)) {
            guid = System.Guid.NewGuid().ToString();
        }
    }
}