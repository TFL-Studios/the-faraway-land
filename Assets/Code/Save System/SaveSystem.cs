using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSaveData
{
    [JsonProperty("LevelID")] public int levelID;
    [JsonProperty("MinProgress")] public int minProgress;
    [JsonProperty("MedProgress")] public int medProgress;
    [JsonProperty("MaxProgress")] public int maxProgress;
}

public class CharactersStateData
{
    [JsonProperty("CharacterName")] public string characterName;
    [JsonProperty("StateIndex")] public int stateIndex;
}

public class SaveData
{
    public List<LevelSaveData> levelsSaveData;
    public List<CharactersStateData> charactersSateData;
}

public class SaveSystem : MonoBehaviour
{
    private SaveData _saveData;

    public SaveSystem()
    {
        this._saveData = this.Load();
    }

    private SaveData Load()
    {
        SaveData result;

        string jsonString = File.ReadAllText($"{Application.persistentDataPath}/SaveData.txt");
        result = JsonConvert.DeserializeObject<SaveData>(jsonString);

        if (result == null) { result = new SaveData(); }
        if (result.levelsSaveData == null ) { result.levelsSaveData = new List<LevelSaveData>(); }
        if (result.charactersSateData == null ) { result.charactersSateData = new List<CharactersStateData>(); }

        return result;
    }

    public void PersistSavedData()
    {
        string jsonString = JsonConvert.SerializeObject(this._saveData, Formatting.Indented);
        string path = $"{Application.persistentDataPath}/SaveData.txt";
        File.WriteAllText(path, jsonString);
    }

    public bool SaveLevel(int levelID, int minProgress, int medProgress, int maxProgress)
    {
        if (this._saveData == null) { return false; }

        if (this._saveData.levelsSaveData == null) { return false; }

        for (int i = 0; i < this._saveData.levelsSaveData.Count; i++)
        {
            if (this._saveData.levelsSaveData[i].levelID == levelID)
            {
                this._saveData.levelsSaveData[i].minProgress = minProgress;
                this._saveData.levelsSaveData[i].medProgress = medProgress;
                this._saveData.levelsSaveData[i].maxProgress = maxProgress;
                return true;
            }
        }

        LevelSaveData newLevelSaveData = new LevelSaveData();
        newLevelSaveData.minProgress = minProgress;
        newLevelSaveData.medProgress = medProgress;
        newLevelSaveData.maxProgress = maxProgress;
        this._saveData.levelsSaveData.Add(newLevelSaveData);
        return true;
    }
}
