using UnityEngine;

[CreateAssetMenu(fileName = "CollectableData", menuName = "Scriptable Objects/NewCollectableData")]
public class CollectableData : ScriptableObject
{
    [Header("General")]
    public string collectableName;
    [TextArea] public string journalDescription;

    [Header("World Prop")]
    public Sprite propSprite;

    [Header("Journal UI")]
    public int journalPage;
    public Vector2 journalOffset;
    public Sprite journalMissing;
    public Sprite journalFound;
    public Sprite journalFocused;
}
