using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI linesBox;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private GameObject answerBoxPrefab;
    private string[] linhas = new[] { "Linha1", "Linha2", "Linha3" };
    private int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        if (index >= linhas.Length)
        {
            return false;
        }
            linesBox.text = linhas[index];
        index++;
        
        return true;
    }

    public void AnswerBox()
    {
        Instantiate(answerBoxPrefab, answerPanel.transform, false);
    }

    
}
