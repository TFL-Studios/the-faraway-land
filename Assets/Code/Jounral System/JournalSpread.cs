using UnityEngine;

[CreateAssetMenu(fileName = "NewJournalSpread", menuName = "Scriptable Objects/JournalSpread")]
public class JournalSpread : ScriptableObject
{
    public JournalEntry[] spreadEntries;
}
