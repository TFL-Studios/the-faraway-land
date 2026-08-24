using NUnit.Framework.Constraints;
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
    [SerializeField] private GameObject _collectablesPanel;
    [SerializeField] private GameObject _collectableImagePrefab;
    [SerializeField] private GameObject[] _collectablesPagePanels;
    [SerializeField] private RectTransform _collectableSelector;
    [SerializeField] private GameObject _collectableFocusPanel;
    [SerializeField] private Image _collectableFocusImage;
    [SerializeField] private TextMeshProUGUI _collectableFocusText;

    [System.Serializable]
    public struct Collectable // TODO: create data type
    {
        public int page;
        public Vector2 offset;
        public Sprite missing;
        public Sprite found;
        public Sprite focused;
        [TextArea] public string description;
    }

    [SerializeField] private Collectable[] _allCollectables; // TODO: change to resources.load
    private Collectable[][] _pagedCollectables;
    private bool[][] _unlockedCollectables;

    private int _activeCollectablesPage = 0;
    private int _selectedCollectableIndex = 0;

    private RectTransform _targetCollectable;

    private bool isFocusedOnCollectable = false;

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

    [System.Serializable]
    public struct Portrait // TODO: create data type
    {
        public Sprite silhouette;
        public Sprite colored;
    }

    [SerializeField] private Portrait[] _ainallCharacterPortraits; // TODO: change to resources.load
    [SerializeField] private bool[] _unlockedCharacterPortraits;

    private int _activeCharacterSheetIndex = 0;

    private void Start()
    {
        this.InitCollectables();

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
        this.UpdateCollectableSelector();

        if (InputHandler.Instance.EntradaDiario.FoiPressionada)
        {
            this._collectableFocusPanel.SetActive(false);
            this._journalUIPanel.SetActive(!this._journalUIPanel.activeSelf);
        }

        if (!this._journalUIPanel.activeSelf) return;

        if (InputHandler.Instance.EntradaNavegacao.FoiPressionada)
        {
            Vector2 inputValue = InputHandler.Instance.EntradaNavegacao.Valor;

            this.ChangeJournalMode((int)inputValue.y);

            switch (this._currentMode)
            {
                case JournalMode.Collectables:
                    if (this.ChangeSelectedCollectable((int)inputValue.x, out bool pageFlag)) { if (pageFlag) this.UpdateCollectablesUI(); else this.UpdateCollectableTarget(); }
                    break;
                case JournalMode.Memories:
                    if (this.ChangeActiveMemory((int)inputValue.x)) { this.UpdateMemoryUI(); }
                    break;
                case JournalMode.CharacterSheets:
                    if (this.ChangeActiveCharacterSheet((int)inputValue.x)) { this.UpdateCharacterSheetUI(); }
                    break;
            }
        }

        if (InputHandler.Instance.EntradaSelecao.FoiPressionada)
        {
            switch (this._currentMode)
            {
                case JournalMode.Collectables:
                    if (this.isFocusedOnCollectable)
                    {
                        this.isFocusedOnCollectable = false;
                    }
                    else if (this._unlockedCollectables[this._activeCollectablesPage][this._selectedCollectableIndex])
                    {
                        Collectable selectedCollectable = this._pagedCollectables[this._activeCollectablesPage][this._selectedCollectableIndex];
                        
                        this._collectableFocusImage.sprite = selectedCollectable.focused;
                        this._collectableFocusImage.rectTransform.sizeDelta = selectedCollectable.focused.rect.size;

                        this._collectableFocusText.text = selectedCollectable.description;

                        this.isFocusedOnCollectable = true;
                    }

                    this._collectableFocusPanel.SetActive(this.isFocusedOnCollectable);
                    break;
                case JournalMode.Memories:
                    
                    break;
                case JournalMode.CharacterSheets:
                    
                    break;
            }
        }

        // Debug

        if (Input.GetKeyDown(KeyCode.Return))
        {
            bool self = this._unlockedCollectables[this._activeCollectablesPage][this._selectedCollectableIndex];
            this._unlockedCollectables[this._activeCollectablesPage][this._selectedCollectableIndex] = !self;
            this.UpdateCollectablesUI();
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
                this._collectablesPanel.SetActive(state);
                this.UpdateCollectablesUI();
                break;

            case JournalMode.Memories:
                this._memoriesModeSelector.color = state ? Color.white : Color.gray; // TODO: change sprite instead
                this._memoryImagePanel.SetActive(state);
                this._memoryText.gameObject.SetActive(state);
                this.UpdateMemoryUI();
                break;

            case JournalMode.CharacterSheets:
                this._characterSheetsModeSelector.color = state ? Color.white : Color.gray; // TODO: change sprite instead
                for (int i = this._characterSelectors.Length - 1; i >= 0; i--)
                {
                    this._characterSelectors[i].color = i == this._activeCharacterSheetIndex ? Color.white : Color.gray; // TODO: change sprite instead
                }
                this._characterSheetLeftPanel.SetActive(state);
                this._characterSheetRightPanel.SetActive(state);
                this.UpdateCharacterSheetUI();
                break;
        }
    }

    /* Collectables Mode */

    private void InitCollectables()
    {
        int pageAmount = this._allCollectables[this._allCollectables.Length - 1].page + 1;
        this._pagedCollectables = new Collectable[pageAmount][];
        this._unlockedCollectables = new bool[pageAmount][];

        int curPage = 0;
        int start = 0;
        for (int i = 0; i < this._allCollectables.Length; i++)
        {
            if (i != this._allCollectables.Length - 1 && this._allCollectables[i].page == curPage) continue;
            if (i == this._allCollectables.Length - 1) i++;

            int colAmount = i - start;
            this._pagedCollectables[curPage] = new Collectable[colAmount];
            this._unlockedCollectables[curPage] = new bool[colAmount];
            for (int j = 0; j < this._pagedCollectables[curPage].Length; j++)
            {
                this._pagedCollectables[curPage][j] = this._allCollectables[start + j];
                this._unlockedCollectables[curPage][j] = false;
            }

            start = i;
            curPage++;
        }
    }

    private void UpdateCollectablesUI()
    {
        // Clear Previous Collectables
        for (int index = 0; index < this._collectablesPagePanels[0].transform.childCount; index++)
        {
            GameObject.Destroy(this._collectablesPagePanels[0].transform.GetChild(index).gameObject);
        }

        // Spawn os Negocio
        Collectable[] curPageCollectables = this._pagedCollectables[this._activeCollectablesPage];
        for (int index = 0; index < curPageCollectables.Length; index++)
        {
            Sprite sprite = this._unlockedCollectables[this._activeCollectablesPage][index] ? curPageCollectables[index].found : curPageCollectables[index].missing;

            Image newImage = GameObject.Instantiate(this._collectableImagePrefab, this._collectablesPagePanels[0].transform, false).GetComponent<Image>();
            newImage.sprite = sprite;
            newImage.rectTransform.sizeDelta = sprite.rect.size;
            newImage.rectTransform.anchoredPosition = curPageCollectables[index].offset;
            newImage.gameObject.SetActive(true);

            if (index == this._selectedCollectableIndex) this._targetCollectable = newImage.rectTransform;
        }

        // this._collectablesPagePanels[0].SetActive(true);
    }

    private void UpdateCollectableTarget()
    {
        this._targetCollectable = this._collectablesPagePanels[0].transform.GetChild(this._selectedCollectableIndex).GetComponent<RectTransform>();
    }

    private bool ChangeCollectablesPage(int amount)
    {
        this._activeCollectablesPage += amount;

        if (this._activeCollectablesPage < 0) this._activeCollectablesPage = this._pagedCollectables.Length - 1;
        if (this._activeCollectablesPage >= this._pagedCollectables.Length) this._activeCollectablesPage = 0;

        return amount != 0;
    }

    private bool ChangeSelectedCollectable(int amount, out bool pageFlag)
    {
        pageFlag = false;

        this._selectedCollectableIndex += amount;

        if (this._selectedCollectableIndex < 0)
        {
            pageFlag = this.ChangeCollectablesPage(-1);
            this._selectedCollectableIndex = this._pagedCollectables[this._activeCollectablesPage].Length - 1;
        }
        else if (this._selectedCollectableIndex >= this._pagedCollectables[this._activeCollectablesPage].Length)
        {
            pageFlag = this.ChangeCollectablesPage(1);
            this._selectedCollectableIndex = 0;
        }

        return amount != 0;
    }

    private void UpdateCollectableSelector()
    {
        if (!this._targetCollectable) return;

        Vector2 targetPosition = this._targetCollectable.anchoredPosition;
        targetPosition.x += this._targetCollectable.sizeDelta.x / 2;
        targetPosition.y -= this._targetCollectable.sizeDelta.y / 2;

        this._collectableSelector.anchoredPosition = Vector2.Lerp(this._collectableSelector.anchoredPosition, targetPosition, Time.deltaTime * 20f);
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
        Sprite silhouette = this._ainallCharacterPortraits[this._activeCharacterSheetIndex].silhouette;
        Sprite portrait = this._ainallCharacterPortraits[this._activeCharacterSheetIndex].colored;
        bool isUnlocked = this._unlockedCharacterPortraits[this._activeCharacterSheetIndex];

        this._characterPortraitImage.sprite = isUnlocked ? portrait : silhouette;
    }
}
