using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public struct StageData
    {
        public int stageID;
        public string objective;
        [TextArea] public string passCondition;
    }

    [System.Serializable]
    public struct QuestData
    {
        public StageData[] stages;
    }

    public int levelID;

    public QuestData minProgression;
    public QuestData medProgression;
    public QuestData maxProgression;
}
