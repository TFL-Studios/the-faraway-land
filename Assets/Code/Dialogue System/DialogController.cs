using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogBox;
    [SerializeField] private GameObject _answerPanel;
    [SerializeField] private TextMeshProUGUI _linesBox;
    [SerializeField] private GameObject _buttonsParent;
    [SerializeField] private GameObject _answerBoxPrefab;
    [SerializeField] private Image _firstCharacter;
    [SerializeField] private Image _secondCharacter;
    private int _indexButtons = 0;
    private string[] _lanes = new[] { "Linha1", "Linha2", "Linha3" };
    private string[] _options = new[] { "eca", "ola", "..." };
    private Image[] _buttons;
    private int _index = 0;
    private int _spawnController = 1;
    private int _changeCharacter = 1;
    private int _activeCharacter = 1;
    
    private void Update()
    {
        if (this._buttons != null)
        {
            if (Input.GetKeyDown("w"))
            {
                this._buttons[this._indexButtons].color = Color.white;
                this._indexButtons--;

                if (this._indexButtons < 0)
                {
                    this._indexButtons = this._buttons.Length - 1;
                }
                this._buttons[this._indexButtons].color = Color.red;
            }

            if (Input.GetKeyDown("s"))
            {
                this._buttons[this._indexButtons].color = Color.white;
                this._indexButtons++;
                if (this._indexButtons >= this._buttons.Length)
                {
                    this._indexButtons = 0;
                }
                this._buttons[this._indexButtons].color = Color.red;
            }

            if (Input.GetKeyDown("j"))
            {
                this.ConfirmAnswer();
            }
        }

        if (Input.GetKeyDown("r"))
        {
            this._dialogBox.SetActive(!this._dialogBox.activeSelf);
        }

        if (Input.GetKeyDown("e"))
        {
            this.NextLine();
        }

        if (Input.GetKeyDown("q"))
        {
            this.AnswerBox();
        }

        if(Input.GetKeyDown("f"))
        {
            this._dialogBox.SetActive(true);
            this._answerPanel.SetActive(true);
            this.SpawnCharacter();
        }

        if (Input.GetKeyDown("c"))
        {
            this.SwichtCharacter();
        }

        if(Input.GetKeyDown("x"))
        {
            this.ActiveCharacter();
        }
    }

    public bool NextLine()
    {
        if (this._index >= this._lanes.Length)
        {
            return false;
        }
        this._linesBox.text = this._lanes[this._index];
        this._index++;
        
        return true;
    }

    public void SpawnCharacter()
    {
        switch(this._spawnController)
        {
            case 1:
                this._firstCharacter.gameObject.SetActive(!this._firstCharacter.gameObject.activeSelf);
                break;
            case 2:
                this._secondCharacter.gameObject.SetActive(!this._secondCharacter.gameObject.activeSelf);
                break;
            case 3:
                this._firstCharacter.gameObject.SetActive(!this._firstCharacter.gameObject.activeSelf);
                this._secondCharacter.gameObject.SetActive(!this._secondCharacter.gameObject.activeSelf);
                break;
        }
    }

    public void SwichtCharacter()
    {
        switch (this._changeCharacter) 
        { 
            case 1:
                this._firstCharacter.color = Color.red;
                TextMeshProUGUI textfirstCharacter = this._firstCharacter.GetComponentInChildren<TextMeshProUGUI>();
                textfirstCharacter.text = "Circulo Vermelho FUDIDO";
                break;
            case 2:
                this._secondCharacter.color = Color.orange;
                TextMeshProUGUI textsecondCharacter = this._secondCharacter.GetComponentInChildren<TextMeshProUGUI>();
                textsecondCharacter.text = "Quadrado Laranja MIJADO";
                break;
        }
    }

    public void ActiveCharacter()
    {
        Vector3 secondCharacterPosition = this._secondCharacter.rectTransform.anchoredPosition;
        Vector3 firstCharacterPosition = this._firstCharacter.rectTransform.anchoredPosition;
        Color firstCharacterColor = this._firstCharacter.GetComponent<Image>().color;
        Color secondCharacterColor = this._secondCharacter.color;
        switch (this._activeCharacter)
        {
            case 1:
                firstCharacterPosition.y = 0f;
                this._firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterPosition.y = 10f;
                this._secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterColor.a = 0.4f;
                this._firstCharacter.color = firstCharacterColor;
                secondCharacterColor.a = 1f;
                this._secondCharacter.color = secondCharacterColor;
                break;

            case 2:
                secondCharacterPosition.y = 0f;
                this._secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterPosition.y = 10f;
                this._firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterColor.a = 0.4f;
                this._secondCharacter.color = secondCharacterColor;
                firstCharacterColor.a = 1f;
                this._firstCharacter.color = firstCharacterColor;
                break;

            case 3:
                secondCharacterPosition.y = 0f;
                this._secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterPosition.y = 0f;
                this._firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterColor.a = 0.4f;
                this._secondCharacter.color = secondCharacterColor;
                firstCharacterColor.a = 0.4f;
                this._firstCharacter.color = firstCharacterColor;
                break;
            
            case 4:
                secondCharacterPosition.y = 10f;
                this._secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterPosition.y = 10f;
                this._firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterColor.a = 1f;
                this._secondCharacter.color = secondCharacterColor;
                firstCharacterColor.a = 1f;
                this._firstCharacter.color = firstCharacterColor;
                break;
        }
    }

    public void ConfirmAnswer()
    {
        TextMeshProUGUI textButtons = this._buttons[this._indexButtons].GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(textButtons.text);
        for (int i = this._buttons.Length - 1; i >= 0; i--) 
        {
            GameObject.Destroy(this._buttons[i].gameObject);
        }
    }

    public void AnswerBox()
    {
        for (int i = 0; i < this._options.Length; i++)
        {
            GameObject answerBoxInstance = GameObject.Instantiate(this._answerBoxPrefab, this._buttonsParent.transform, false);
            TextMeshProUGUI textPrefab = answerBoxInstance.GetComponentInChildren<TextMeshProUGUI>();
            textPrefab.text = this._options[i];
            
        }

        this._buttons = this._buttonsParent.GetComponentsInChildren<Image>();
        this._indexButtons = 0;
        this._buttons[this._indexButtons].color = Color.red;
    }
}
