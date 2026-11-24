using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    [JsonProperty("LevelID")] public int levelID;
    [JsonProperty("MinProgress")] public int minProgress;
    [JsonProperty("MedProgress")] public int medProgress;
    [JsonProperty("MaxProgress")] public int maxProgress;
}

public class CharacterSaveData
{
    [JsonProperty("CharacterName")] public string characterName;
    [JsonProperty("StateIndex")] public int stateIndex;
}

public class SaveData
{
    public int currentLevelID;
    public List<LevelSaveData> levelSaveDataList;
    public List<CharacterSaveData> characterSaveDataList;

    public SaveData()
    {
        this.currentLevelID = -1;
        this.levelSaveDataList = new List<LevelSaveData>();
        this.characterSaveDataList = new List<CharacterSaveData>();
    }
}

public class SaveSystem
{
    private SaveData _saveData;

    public SaveSystem()
    {
        this._saveData = this.Load();
    }

    private SaveData Load()
    {
        SaveData result;
        string jsonString = string.Empty;

        try
        {
            jsonString = File.ReadAllText($"{Application.persistentDataPath}/SaveData.txt");
        }
        catch (FileNotFoundException e)
        {
            File.WriteAllText($"{Application.persistentDataPath}/SaveData.txt", "");
        }
        finally
        {
            result = JsonConvert.DeserializeObject<SaveData>(jsonString);
        }

        if (result == null) { result = new SaveData(); }

        return result;
    }

    public void PersistSavedData()
    {
        string jsonString = JsonConvert.SerializeObject(this._saveData, Formatting.Indented);
        string path = $"{Application.persistentDataPath}/SaveData.txt";
        File.WriteAllText(path, jsonString);
    }

    public void SaveCurrentLevelID(int levelID)
    {
        this._saveData.currentLevelID = levelID;
    }

    public int FetchCurrentLevelID()
    {
        return this._saveData.currentLevelID;
    }

    public bool SaveLevel(LevelSaveData levelSaveData)
    {
        int levelID = levelSaveData.levelID;
        int minProgress = levelSaveData.minProgress;
        int medProgress = levelSaveData.medProgress;
        int maxProgress = levelSaveData.maxProgress;

        for (int index = 0; index < this._saveData.levelSaveDataList.Count; index++)
        {
            if (this._saveData.levelSaveDataList[index].levelID == levelID)
            {
                this._saveData.levelSaveDataList[index].minProgress = minProgress;
                this._saveData.levelSaveDataList[index].medProgress = medProgress;
                this._saveData.levelSaveDataList[index].maxProgress = maxProgress;
                return true;
            }
        }

        LevelSaveData newLevelSaveData = new LevelSaveData();
        newLevelSaveData.levelID = levelID;
        newLevelSaveData.minProgress = minProgress;
        newLevelSaveData.medProgress = medProgress;
        newLevelSaveData.maxProgress = maxProgress;
        this._saveData.levelSaveDataList.Add(newLevelSaveData);
        return true;
    }

    public LevelSaveData FetchLevel(int levelID)
    {
        LevelSaveData result = null;

        for (int index = 0; index < this._saveData.levelSaveDataList.Count; index++)
        {
            if (levelID == this._saveData.levelSaveDataList[index].levelID)
            {
                result = this._saveData.levelSaveDataList[index];
                break;
            }
        }

        return result;
    }

    public bool SaveCharacterState(string characterName, int stateIndex)
    {
        for (int index = 0; index < this._saveData.characterSaveDataList.Count; index++)
        {
            if (this._saveData.characterSaveDataList[index].characterName == characterName)
            {
                this._saveData.characterSaveDataList[index].stateIndex = stateIndex;
                return true;
            }
        }

        CharacterSaveData mewCharacterSaveData = new CharacterSaveData();
        mewCharacterSaveData.characterName = characterName;
        mewCharacterSaveData.stateIndex = stateIndex;
        this._saveData.characterSaveDataList.Add(mewCharacterSaveData);
        return true;
    }

    public int FetchCharacterState(string characterName)
    {
        int result = -1;

        for (int index = 0; index < this._saveData.characterSaveDataList.Count; index++)
        {
            if (this._saveData.characterSaveDataList[index].characterName == characterName)
            {
                result = this._saveData.characterSaveDataList[index].stateIndex;
                break;
            }
        }

        return result;
    }
}
