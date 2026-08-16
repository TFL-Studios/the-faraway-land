using System.Collections;
using System.Collections.Generic;
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

    [Header("MemoriesMode")]
    [SerializeField] private GameObject _memoryImagePanel;
    [SerializeField] private TextMeshProUGUI _memoryText;

    private GameObject _memoryImagePrefab;

    [Header("CharacterSheetMode")]
    [SerializeField] private GameObject _characterSheetLeftPanel;
    [SerializeField] private GameObject _characterSheetRightPanel;
    [SerializeField] private Image _characterPortrait;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI _activeMemoryDisplay;
    [SerializeField] private List<JournalMemories> _memories;
    private int _activeMemoryIndex = 0;
    private int[] _maxEntryUnlocked;

    private void Start()
    {
        this._memoryImagePrefab = this._memoryImagePanel.transform.GetChild(0).gameObject;
        this._maxEntryUnlocked = new int[this._memories.Count];

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

            this.ChangeJournalMode((int)inputValue.y);

            switch (this._currentMode)
            {
                case JournalMode.Collectables:
                    //if (this.ChangeSelectedCollectable((int)inputValue)) { this.UpdateCollectableUI(); }
                    break;
                case JournalMode.Memories:
                    if (this.ChangeActiveMemory((int)inputValue.x)) { this.UpdateMemoryUI(); }
                    break;
                case JournalMode.CharacterSheets:
                    if (this.ChangeActiveCharacterSheet((int)inputValue.x)) { this.UpdateCharacterSheetUI(); }
                    break;
            }
        }

        // debug
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (this.ChangeUnlockAmount(1)) { this.UpdateMemoryUI(); }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (this.ChangeUnlockAmount(-1)) { this.UpdateMemoryUI(); }
        }
    }

    /* General */
    private void ChangeJournalMode(int amount)
    {
        this.SetJournalModeObjects(false);

        this._currentMode -= amount;
        if (this._currentMode >= JournalMode._COUNT) this._currentMode = 0;
        if (this._currentMode < 0) this._currentMode = JournalMode._COUNT - 1;

        this.SetJournalModeObjects(true);
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
                    this._characterSelectors[i].color = i == this._currentActiveChar ? Color.white : Color.gray; // TODO: change sprite instead
                }
                this._characterSheetLeftPanel.SetActive(state);
                this._characterSheetRightPanel.SetActive(state);
                break;
        }
    }

    /* Memories Mode */

    private bool ChangeUnlockAmount(int amount) // DEBUG
    {
        int buffer = this._maxEntryUnlocked[this._activeMemoryIndex];

        this._maxEntryUnlocked[this._activeMemoryIndex] += amount;
        if (this._maxEntryUnlocked[this._activeMemoryIndex] < 0) { this._maxEntryUnlocked[this._activeMemoryIndex] = 0; }
        else if (this._maxEntryUnlocked[this._activeMemoryIndex] > this._memories[this._activeMemoryIndex].memoryEntries.Length) this._maxEntryUnlocked[this._activeMemoryIndex] = this._memories[this._activeMemoryIndex].memoryEntries.Length;

        return this._maxEntryUnlocked[this._activeMemoryIndex] != buffer;
    }

    private bool ChangeActiveMemory(int amount)
    {
        int lastMemory = this._activeMemoryIndex;

        this._activeMemoryIndex += amount;
        if (this._activeMemoryIndex < 0) { this._activeMemoryIndex = 0; }
        else if (this._activeMemoryIndex >= this._memories.Count) { this._activeMemoryIndex = this._memories.Count - 1; }

        return this._activeMemoryIndex != lastMemory;
    }

    private void UpdateMemoryUI()
    {
        JournalMemories activeMemory = this._memories[this._activeMemoryIndex];
        string resultText = string.Empty;

        for (int index = 0; index < this._memoryImagePanel.transform.childCount; index++)
        {
            GameObject currentChild = this._memoryImagePanel.transform.GetChild(index).gameObject;
            if (currentChild == this._memoryImagePrefab) { continue; }
            GameObject.Destroy(currentChild);
        }

        for (int index = 0; index < activeMemory.memoryEntries.Length; index++)
        {
            if (index >= this._maxEntryUnlocked[this._activeMemoryIndex]) break;

            Image newImage = GameObject.Instantiate(this._memoryImagePrefab, this._memoryImagePanel.transform, false).GetComponent<Image>();
            newImage.gameObject.SetActive(true);
            newImage.sprite = activeMemory.memoryEntries[index].entrySprite;

            resultText += $"{activeMemory.memoryEntries[index].entryText}\n\n";
        }

        this._memoryText.text = resultText;

        this._activeMemoryDisplay.text = $"Memory {this._activeMemoryIndex + 1} with {this._maxEntryUnlocked[this._activeMemoryIndex]}/{this._memories[this._activeMemoryIndex].memoryEntries.Length} entries unlocked"; // DEBUG
    }

    /* Character Sheet Mode */

    private int _currentActiveChar = 0;

    private bool ChangeActiveCharacterSheet(int amount)
    {
        this._characterSelectors[this._currentActiveChar].color = Color.gray; // TODO: change sprite instead

        this._currentActiveChar += amount;
        if (this._currentActiveChar < 0) this._currentActiveChar = this._characterSelectors.Length - 1;
        if (this._currentActiveChar >= this._characterSelectors.Length) this._currentActiveChar = 0;

        this._characterSelectors[this._currentActiveChar].color = Color.white; // TODO: change sprite instead

        return true;
    }

    private void UpdateCharacterSheetUI()
    {

    }
}
