using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private TextMeshProUGUI linesBox;
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private GameObject answerBoxPrefab;
    [SerializeField] private Image firstCharacter;
    [SerializeField] private Image secondCharacter;
    [SerializeField] private int indexButtons = 0;
    private string[] lanes = new[] { "Linha1", "Linha2", "Linha3" };
    private string[] options = new[] { "eca", "ola", "..." };
    private Image[] buttons;
    private int index = 0;
    [SerializeField] private int spawnController = 1;
    [SerializeField] private int changeCharacter = 1;
    [SerializeField] private int activeCharacter = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (buttons != null)
        {
            

            if (Input.GetKeyDown("w"))
            {
                buttons[indexButtons].color = Color.white;
                indexButtons--;

                if (indexButtons < 0)
                {
                    indexButtons = buttons.Length - 1;
                }
                buttons[indexButtons].color = Color.red;
                
            }

            if (Input.GetKeyDown("s"))
            {
                buttons[indexButtons].color = Color.white;
                indexButtons++;
                if (indexButtons >= buttons.Length)
                {
                    indexButtons = 0;
                }
                buttons[indexButtons].color = Color.red;
                
                
            }

            if (Input.GetKeyDown("j"))
            {
                ConfirmAnswer();
            }
        }


        if (Input.GetKeyDown("r"))
        {
            dialogBox.SetActive(!dialogBox.activeSelf);

        }

        if (Input.GetKeyDown("e"))
        {
            NextLine();
        }

        if (Input.GetKeyDown("q"))
        {
            AnswerBox();
        }

        if(Input.GetKeyDown("f"))
        {
            dialogBox.SetActive(true);
            answerPanel.SetActive(true);
            SpawnCharacter();
        }

        if (Input.GetKeyDown("c"))
        {
            SwichtCharacter();
        }

        if(Input.GetKeyDown("x"))
        {
            ActiveCharacter();
        }



    }

    public bool NextLine()
    {
        if (index >= lanes.Length)
        {
            return false;
        }
            linesBox.text = lanes[index];
        index++;
        
        return true;
    }

    public void SpawnCharacter()
    {
        switch(spawnController)
        {
            case 1:
                firstCharacter.gameObject.SetActive(!firstCharacter.gameObject.activeSelf);
                break;
            case 2:
                secondCharacter.gameObject.SetActive(!secondCharacter.gameObject.activeSelf);
                break;
            case 3:
                firstCharacter.gameObject.SetActive(!firstCharacter.gameObject.activeSelf);
                secondCharacter.gameObject.SetActive(!secondCharacter.gameObject.activeSelf);
                break;

        }


    }

    public void SwichtCharacter()
    {
        
        switch (changeCharacter) 
        { 
            case 1:
                firstCharacter.color = Color.red;
                TextMeshProUGUI textfirstCharacter = firstCharacter.GetComponentInChildren<TextMeshProUGUI>();
                textfirstCharacter.text = "Circulo Vermelho FUDIDO";
                break;
            case 2:
                secondCharacter.color = Color.orange;
                TextMeshProUGUI textsecondCharacter = secondCharacter.GetComponentInChildren<TextMeshProUGUI>();
                textsecondCharacter.text = "Quadrado Laranja MIJADO";
                break;

        }
    }

    public void ActiveCharacter()
    {
        Vector3 secondCharacterPosition = secondCharacter.rectTransform.anchoredPosition;
        Vector3 firstCharacterPosition = firstCharacter.rectTransform.anchoredPosition;
        Color firstCharacterColor = firstCharacter.GetComponent<Image>().color;
        Color secondCharacterColor = secondCharacter.color;
        switch (activeCharacter)
        {
            case 1:
                firstCharacterPosition.y = 0f;
                firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterPosition.y = 10f;
                secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterColor.a = 0.4f;
                firstCharacter.color = firstCharacterColor;
                secondCharacterColor.a = 1f;
                secondCharacter.color = secondCharacterColor;
                break;
            case 2:
                secondCharacterPosition.y = 0f;
                secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterPosition.y = 10f;
                firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterColor.a = 0.4f;
                secondCharacter.color = secondCharacterColor;
                firstCharacterColor.a = 1f;
                firstCharacter.color = firstCharacterColor;

                break;
            case 3:
                secondCharacterPosition.y = 0f;
                secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterPosition.y = 0f;
                firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterColor.a = 0.4f;
                secondCharacter.color = secondCharacterColor;
                firstCharacterColor.a = 0.4f;
                firstCharacter.color = firstCharacterColor;
                break;
                case 4:
                secondCharacterPosition.y = 10f;
                secondCharacter.rectTransform.anchoredPosition = secondCharacterPosition;
                firstCharacterPosition.y = 10f;
                firstCharacter.rectTransform.anchoredPosition = firstCharacterPosition;
                secondCharacterColor.a = 1f;
                secondCharacter.color = secondCharacterColor;
                firstCharacterColor.a = 1f;
                firstCharacter.color = firstCharacterColor;
                break;
        }
    }

    public void ConfirmAnswer()
    {
        TextMeshProUGUI textButtons = buttons[indexButtons].GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(textButtons.text);
        for (int i = buttons.Length - 1; i >= 0; i--) 
        {
            Destroy(buttons[i].gameObject);
        }
    }

    public void AnswerBox()
    {
        for (int i = 0; i < options.Length; i++)
        {
            GameObject answerBoxInstance = Instantiate(answerBoxPrefab, buttonsParent.transform, false);
            TextMeshProUGUI textPrefab = answerBoxInstance.GetComponentInChildren<TextMeshProUGUI>();
            textPrefab.text = options[i];
            
        }

        buttons = buttonsParent.GetComponentsInChildren<Image>();
        indexButtons = 0;
        buttons[indexButtons].color = Color.red;
    }

    
}
