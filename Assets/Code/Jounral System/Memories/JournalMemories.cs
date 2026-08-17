using UnityEngine;

[CreateAssetMenu(fileName = "NewJournalMemory", menuName = "Scriptable Objects/JournalMemory")]
public class JournalMemories : ScriptableObject
{
    public MemoryEntry[] memoryEntries;
}
