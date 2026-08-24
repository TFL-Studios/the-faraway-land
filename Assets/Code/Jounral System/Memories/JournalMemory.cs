using UnityEngine;

[CreateAssetMenu(fileName = "NewJournalMemory", menuName = "Scriptable Objects/JournalMemory")]
public class JournalMemory : ScriptableObject
{
    public MemoryEntry[] memoryEntries;
}
