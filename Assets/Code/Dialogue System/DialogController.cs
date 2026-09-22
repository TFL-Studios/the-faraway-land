using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogBox;
    [SerializeField] private GameObject _answerPanel;
    [SerializeField] private TextMeshProUGUI _dialogLinesBox;
    [SerializeField] private GameObject _buttonsParent;
    [SerializeField] private GameObject _answerBoxPrefab;
    [SerializeField] private Image _firstCharacter;
    [SerializeField] private Image _secondCharacter;
    [SerializeField] private DialogBlock _dialogBlock;
    private int _indexButtons = 0;
    private string[] _options = new[] { "eca", "ola", "..." };
    private Image[] _buttons;
    private int _index = 0;
    Dialog currentDialog;
    private int talking;


    private void Update()
    {
        if (this._buttons != null)
        {
            if (InputHandler.Instance.NavigationInput.Value.y > 0)
            {
                this._buttons[this._indexButtons].color = Color.white;
                this._indexButtons--;

                if (this._indexButtons < 0)
                {
                    this._indexButtons = this._buttons.Length - 1;
                }
                this._buttons[this._indexButtons].color = Color.red;
            }

            if ((InputHandler.Instance.NavigationInput.Value.y < 0))
            {
                this._buttons[this._indexButtons].color = Color.white;
                this._indexButtons++;
                if (this._indexButtons >= this._buttons.Length)
                {
                    this._indexButtons = 0;
                }
                this._buttons[this._indexButtons].color = Color.red;
            }

            if (InputHandler.Instance.SelectionInput.WasPressed)
            {
                this.ConfirmAnswer();
            }
        }

        if (Input.GetKeyDown("r"))
        {
            currentDialog = _dialogBlock.dialog[_index];
            talking = currentDialog.talking;
            this._dialogBox.SetActive(!this._dialogBox.activeSelf);
            this.SpawnCharacter();
            this._dialogLinesBox.text = _dialogBlock.dialog[0].speechs;
            this.ActiveCharacter();
        }

        
        if (InputHandler.Instance.SelectionInput.WasPressed)
        {
            this.NextLine();
        }

        if (Input.GetKeyDown("q"))
        {
            this.AnswerBox();
        }

        

        

        if(Input.GetKeyDown("x"))
        {
            this.ActiveCharacter();
        }
    }

    public bool NextLine()
    {
        if (this._index >= _dialogBlock.dialog.Length)
        {
            return false;
        }
        this._dialogLinesBox.text = _dialogBlock.dialog[this._index].speechs;
        this._index++;
        currentDialog = _dialogBlock.dialog[_index];
        talking = currentDialog.talking;
        this.ActiveCharacter();

        return true;
    }

    public void SpawnCharacter()
    {
        TextMeshProUGUI textfirstCharacter = this._firstCharacter.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textsecondCharacter = this._secondCharacter.GetComponentInChildren<TextMeshProUGUI>();
        CharacterTalking characterTalking = _dialogBlock.characterTalking[talking];


        if (!currentDialog.right)
        {
            this._firstCharacter.gameObject.SetActive(!this._firstCharacter.gameObject.activeSelf);

            this._firstCharacter.sprite = characterTalking.characterTalkingImagem;
            textfirstCharacter.text = characterTalking.characterTalkingName;
        }

        else
        {

            this._secondCharacter.gameObject.SetActive(!this._secondCharacter.gameObject.activeSelf);
            this._secondCharacter.sprite = characterTalking.characterTalkingImagem;
            textsecondCharacter.text = characterTalking.characterTalkingName;


        }
    }



    public void ActiveCharacter()
    {
        Vector3 secondCharacterPosition = this._secondCharacter.rectTransform.anchoredPosition;
        Vector3 firstCharacterPosition = this._firstCharacter.rectTransform.anchoredPosition;
        Color firstCharacterColor = this._firstCharacter.GetComponent<Image>().color;
        Color secondCharacterColor = this._secondCharacter.color;
        if (!currentDialog.right)
        {
            firstCharacterPosition.y = 0f;
            this._firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
            secondCharacterPosition.y = 10f;
            this._secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
            firstCharacterColor.a = 0.4f;
            this._firstCharacter.color = firstCharacterColor;
            secondCharacterColor.a = 1f;
            this._secondCharacter.color = secondCharacterColor;
            
        }
        else 
            {
           
                secondCharacterPosition.y = 0f;
                this._secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterPosition.y = 10f;
                this._firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterColor.a = 0.4f;
                this._secondCharacter.color = secondCharacterColor;
                firstCharacterColor.a = 1f;
                this._firstCharacter.color = firstCharacterColor;
            }

                /* Dois personagens apagado e destacados no dialogo
                    secondCharacterPosition.y = 0f;
                    this._secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                    firstCharacterPosition.y = 0f;
                    this._firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                    secondCharacterColor.a = 0.4f;
                    this._secondCharacter.color = secondCharacterColor;
                    firstCharacterColor.a = 0.4f;
                    this._firstCharacter.color = firstCharacterColor;



                    secondCharacterPosition.y = 10f;
                    this._secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                    firstCharacterPosition.y = 10f;
                    this._firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                    secondCharacterColor.a = 1f;
                    this._secondCharacter.color = secondCharacterColor;
                    firstCharacterColor.a = 1f;
                    this._firstCharacter.color = firstCharacterColor;*/

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
