using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalUIController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject _journalUIPanel;
    [SerializeField] private GameObject _modeSelectorsPanel;
    [SerializeField] private Image _collectablesModeSelector;
    [SerializeField] private Image _memoriesModeSelector;
    [SerializeField] private Image _characterSheetsModeSelector;
    [SerializeField] private Image[] _characterSelectors;

    private JournalMode _currentMode;
    
    [Header("CollectablesMode")]
    [SerializeField] private GameObject _collectablesLeftPanel;
    [SerializeField] private GameObject _collectablesRightPanel;
    [SerializeField] private GameObject _collectableFocusPanel;

    private List<Sprite> _collectables; // TODO: create data type

    [Header("MemoriesMode")]
    [SerializeField] private GameObject _memoryImagePanel;
    [SerializeField] private TextMeshProUGUI _memoryText;

    private GameObject _memoryImagePrefab;

    private Dictionary<string, JournalMemories> _allMemories = new Dictionary<string, JournalMemories>(); // Resources
    private Dictionary<string, int[]> _unlockedMemories = new Dictionary<string, int[]>()
    {
        {"Memory1", new[] { 0, 1, 3 } },
        {"Memory2", new[] { 1, 2 }}
    }; // Load

    private int _activeMemoryIndex = 0;

    [Header("CharacterSheetMode")]
    [SerializeField] private GameObject _characterSheetLeftPanel;
    [SerializeField] private GameObject _characterSheetRightPanel;
    [SerializeField] private Image _characterPortraitImage;

    [SerializeField] private Sprite[] _allCharacterPortraitsShadow; // TODO: change to resources.load
    [SerializeField] private Sprite[] _allCharacterPortraits; // TODO: change to resources.load
    [SerializeField] private bool[] _unlockedCharacterPortraits;

    private int _activeCharacterSheetIndex = 0;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI _activeMemoryDisplay;

    private void Start()
    {
        JournalMemories[] allMemories = Resources.LoadAll<JournalMemories>("Memories");
        foreach (JournalMemories mem in allMemories)
        {
            this._allMemories.Add(mem.name, mem);
        }

        this._memoryImagePrefab = this._memoryImagePanel.transform.GetChild(0).gameObject;

        for (int i = (int)JournalMode._COUNT - 1; i >= 0; i--)
        {
            this._currentMode = (JournalMode)i;
            this.SetJournalModeObjects(i == 0);
        }
    }

    private void Update()
    {
        if (InputHandler.Instance.EntradaDiario.FoiPressionada)
        {
            this._collectableFocusPanel.SetActive(false);
            this._journalUIPanel.SetActive(!this._journalUIPanel.activeSelf);
        }

        if (InputHandler.Instance.EntradaNavegacao.FoiPressionada && this._journalUIPanel.activeSelf)
        {
            Vector2 inputValue = InputHandler.Instance.EntradaNavegacao.Valor;

            bool modeChangeFlag = this.ChangeJournalMode((int)inputValue.y);

            switch (this._currentMode)
            {
                case JournalMode.Collectables:
                    //if (this.ChangeSelectedCollectable((int)inputValue) || modeChangeFlag) { this.UpdateCollectableUI(); }
                    break;
                case JournalMode.Memories:
                    if (this.ChangeActiveMemory((int)inputValue.x) || modeChangeFlag) { this.UpdateMemoryUI(); }
                    break;
                case JournalMode.CharacterSheets:
                    if (this.ChangeActiveCharacterSheet((int)inputValue.x) || modeChangeFlag) { this.UpdateCharacterSheetUI(); }
                    break;
            }
        }
    }

    /* General */
    private bool ChangeJournalMode(int amount)
    {
        if (amount == 0) return false;

        this.SetJournalModeObjects(false);

        this._currentMode -= amount;
        if (this._currentMode >= JournalMode._COUNT) this._currentMode = 0;
        if (this._currentMode < 0) this._currentMode = JournalMode._COUNT - 1;

        this.SetJournalModeObjects(true);

        return true;
    }

    private void SetJournalModeObjects(bool state)
    {
        switch (this._currentMode)
        {
            case JournalMode.Collectables:
                this._collectablesModeSelector.color = state ? Color.white : Color.gray; // TODO: change sprite instead
                this._collectablesLeftPanel.SetActive(state);
                this._collectablesRightPanel.SetActive(state);
                break;

            case JournalMode.Memories:
                this._memoriesModeSelector.color = state ? Color.white : Color.gray; // TODO: change sprite instead
                this._memoryImagePanel.SetActive(state);
                this._memoryText.gameObject.SetActive(state);
                break;

            case JournalMode.CharacterSheets:
                this._characterSheetsModeSelector.color = state ? Color.white : Color.gray; // TODO: change sprite instead
                for (int i = this._characterSelectors.Length - 1; i >= 0; i--)
                {
                    this._characterSelectors[i].color = i == this._activeCharacterSheetIndex ? Color.white : Color.gray; // TODO: change sprite instead
                }
                this._characterSheetLeftPanel.SetActive(state);
                this._characterSheetRightPanel.SetActive(state);
                break;
        }
    }

    /* Memories Mode */

    private bool ChangeActiveMemory(int amount)
    {
        int lastMemoryIndex = this._activeMemoryIndex;

        this._activeMemoryIndex += amount;
        if (this._activeMemoryIndex < 0) { this._activeMemoryIndex = 0; }
        else if (this._activeMemoryIndex >= this._unlockedMemories.Count) { this._activeMemoryIndex = this._unlockedMemories.Count - 1; }

        return this._activeMemoryIndex != lastMemoryIndex;
    }

    private void UpdateMemoryUI()
    {
        KeyValuePair<string, int[]> unlockedMemory = this._unlockedMemories.ElementAt(this._activeMemoryIndex);
        JournalMemories activeMemoryData = this._allMemories[unlockedMemory.Key];
        string resultText = string.Empty;

        // Clear Previous Image
        for (int index = 0; index < this._memoryImagePanel.transform.childCount; index++)
        {
            GameObject currentChild = this._memoryImagePanel.transform.GetChild(index).gameObject;
            if (currentChild == this._memoryImagePrefab) { continue; }
            GameObject.Destroy(currentChild);
        }

        // Construct New Image
        for (int index = 0; index < activeMemoryData.memoryEntries.Length; index++)
        {
            string appendage = new string('.', activeMemoryData.memoryEntries[index].entryText.Length);

            if (unlockedMemory.Value.Contains(index))
            {
                Image newImage = GameObject.Instantiate(this._memoryImagePrefab, this._memoryImagePanel.transform, false).GetComponent<Image>();
                newImage.gameObject.SetActive(true);
                newImage.sprite = activeMemoryData.memoryEntries[index].entrySprite;

                appendage = activeMemoryData.memoryEntries[index].entryText;
            }

            resultText += $"{appendage}\n\n";
        }

        this._memoryText.text = resultText;

        this._activeMemoryDisplay.text = $"Memory {unlockedMemory.Key} with entries {unlockedMemory.Value.ToString()} unlocked"; // DEBUG
    }

    /* Character Sheet Mode */

    private bool ChangeActiveCharacterSheet(int amount)
    {
        this._characterSelectors[this._activeCharacterSheetIndex].color = Color.gray; // TODO: change sprite instead

        this._activeCharacterSheetIndex += amount;
        if (this._activeCharacterSheetIndex < 0) this._activeCharacterSheetIndex = this._characterSelectors.Length - 1;
        if (this._activeCharacterSheetIndex >= this._characterSelectors.Length) this._activeCharacterSheetIndex = 0;

        this._characterSelectors[this._activeCharacterSheetIndex].color = Color.white; // TODO: change sprite instead

        return true;
    }

    private void UpdateCharacterSheetUI()
    {
        Sprite shadow = this._allCharacterPortraitsShadow[this._activeCharacterSheetIndex];
        Sprite portrait = this._allCharacterPortraits[this._activeCharacterSheetIndex];
        bool isUnlocked = this._unlockedCharacterPortraits[this._activeCharacterSheetIndex];

        this._characterPortraitImage.sprite = isUnlocked ? portrait : shadow;
    }
}
