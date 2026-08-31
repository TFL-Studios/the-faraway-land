using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private TextMeshProUGUI linesBox;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private GameObject answerBoxPrefab;
    [SerializeField] private int indexButtons = 0;
    private string[] lanes = new[] { "Linha1", "Linha2", "Linha3" };
    private string[] options = new[] { "eca", "ola", "..." };
    private Image[] buttons;
    private int index = 0;
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
        }


        if (Input.GetKeyDown("r"))
        {
            textBox.SetActive(!textBox.activeSelf);

        }

        if (Input.GetKeyDown("e"))
        {
            NextLine();
        }

        if (Input.GetKeyDown("q"))
        {
            AnswerBox();
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

    public void AnswerBox()
    {
        for (int i = 0; i < options.Length; i++)
        {
            GameObject answerBoxInstance = Instantiate(answerBoxPrefab, answerPanel.transform, false);
            TextMeshProUGUI textPrefab = answerBoxInstance.GetComponentInChildren<TextMeshProUGUI>();
            textPrefab.text = options[i];
            
        }

        buttons = answerPanel.GetComponentsInChildren<Image>();
        indexButtons = 0;
        buttons[indexButtons].color = Color.red;
    }

    
}
