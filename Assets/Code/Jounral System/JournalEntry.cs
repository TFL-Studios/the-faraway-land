using UnityEngine;

[System.Serializable]
public struct JournalEntry
{
    public Sprite entrySprite;
    [TextArea] public string entryText;
}