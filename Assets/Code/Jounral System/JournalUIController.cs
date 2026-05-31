using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalUIController : MonoBehaviour
{
    [SerializeField] private GameObject _journalUIPanel;
    [SerializeField] private GameObject _spreadImagePanel;
    [SerializeField] private TextMeshProUGUI _spreadText;

    private GameObject _spreadImagePrefab;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI _activeSpreadDisplay;
    [SerializeField] private JournalSpread[] _spreads;
    private int _activeSpreadIndex = 0;
    private int[] _maxEntryUnlocked;

    private void Start()
    {
        this._spreadImagePrefab = this._spreadImagePanel.transform.GetChild(0).gameObject;
        this._maxEntryUnlocked = new int[this._spreads.Length];
    }

    private void Update()
    {
        if (InputHandler.Instance.EntradaDiario.FoiPressionada)
        {
            this._journalUIPanel.SetActive(!this._journalUIPanel.activeSelf);
        }

        if (InputHandler.Instance.EntradaNavegacao.FoiPressionada && this._journalUIPanel.activeSelf)
        {
            Vector2 inputValue = InputHandler.Instance.EntradaNavegacao.Valor;

            if (this.ChangeActiveSpread((int)inputValue.x)) { this.UpdateSpreadUI(); }
            if (this.ChangeUnlockAmount((int)inputValue.y)) { this.UpdateSpreadUI(); };
        }
    }

    private bool ChangeUnlockAmount(int amount) // DEBUG
    {
        int buffer = this._maxEntryUnlocked[this._activeSpreadIndex];

        this._maxEntryUnlocked[this._activeSpreadIndex] += amount;
        if (this._maxEntryUnlocked[this._activeSpreadIndex] < 0) { this._maxEntryUnlocked[this._activeSpreadIndex] = 0; }
        else if (this._maxEntryUnlocked[this._activeSpreadIndex] > this._spreads[this._activeSpreadIndex].spreadEntries.Length) this._maxEntryUnlocked[this._activeSpreadIndex] = this._spreads[this._activeSpreadIndex].spreadEntries.Length;

        return this._maxEntryUnlocked[this._activeSpreadIndex] != buffer;
    }

    private bool ChangeActiveSpread(int amount)
    {
        int pastSpread = this._activeSpreadIndex;

        this._activeSpreadIndex += amount;
        if (this._activeSpreadIndex < 0) { this._activeSpreadIndex = 0; }
        else if (this._activeSpreadIndex >= this._spreads.Length) { this._activeSpreadIndex = this._spreads.Length - 1; }

        return this._activeSpreadIndex != pastSpread;
    }

    private void UpdateSpreadUI()
    {
        JournalSpread activeSpread = this._spreads[this._activeSpreadIndex];
        string resultText = string.Empty;

        for (int index = 0; index < this._spreadImagePanel.transform.childCount; index++)
        {
            GameObject currentChild = this._spreadImagePanel.transform.GetChild(index).gameObject;
            if (currentChild == this._spreadImagePrefab) { continue; }
            GameObject.Destroy(currentChild);
        }

        for (int index = 0; index < activeSpread.spreadEntries.Length; index++)
        {
            if (index >= this._maxEntryUnlocked[this._activeSpreadIndex]) break;

            Image newImage = GameObject.Instantiate(this._spreadImagePrefab, this._spreadImagePanel.transform, false).GetComponent<Image>();
            newImage.gameObject.SetActive(true);
            newImage.sprite = activeSpread.spreadEntries[index].entrySprite;

            resultText += $"{activeSpread.spreadEntries[index].entryText}\n\n";
        }

        this._spreadText.text = resultText;

        this._activeSpreadDisplay.text = $"Spread {this._activeSpreadIndex + 1} with {this._maxEntryUnlocked[this._activeSpreadIndex]}/{this._spreads[this._activeSpreadIndex].spreadEntries.Length} entries unlocked"; // DEBUG
    }
}
