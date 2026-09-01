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
    [SerializeField] private GameObject firstCharacter;
    [SerializeField] private GameObject secondCharacter;
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
                firstCharacter.SetActive(!firstCharacter.activeSelf);
                break;
            case 2:
                secondCharacter.SetActive(!secondCharacter.activeSelf);
                break;
            case 3:
                firstCharacter.SetActive(!firstCharacter.activeSelf);
                secondCharacter.SetActive(!secondCharacter.activeSelf);
                break;

        }


    }

    public void SwichtCharacter()
    {
        
        switch (changeCharacter) 
        { 
            case 1:
                firstCharacter.GetComponent<Image>().color = Color.red;
                TextMeshProUGUI textfirstCharacter = firstCharacter.GetComponentInChildren<TextMeshProUGUI>();
                textfirstCharacter.text = "Circulo Vermelho FUDIDO";
                break;
            case 2:
                secondCharacter.GetComponent<Image>().color = Color.orange;
                TextMeshProUGUI textsecondCharacter = secondCharacter.GetComponentInChildren<TextMeshProUGUI>();
                textsecondCharacter.text = "Quadrado Laranja MIJADO";
                break;

        }
    }

    public void ActiveCharacter()
    {
        switch(activeCharacter)
        {
            case 1:
                secondCharacter.transform.position = new Vector3(50f,0f,0f);// arrumar a posicao
                Color firstCharacterColor = firstCharacter.GetComponent<Image>().color;
                firstCharacterColor.a = 0.4f;
                firstCharacter.GetComponent<Image>().color = firstCharacterColor;
                break;
            case 2:
                firstCharacter.transform.position = new Vector3(0f, 0f, 0f);
                secondCharacter.transform.position = new Vector3(15f, 0f, 0f);
                Color secondCharacterColor = firstCharacter.GetComponent<Image>().color;
                secondCharacterColor.a = 0.4f;
                secondCharacter.GetComponent<Image>().color = secondCharacterColor;
                         
                break;
            case 3:

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
