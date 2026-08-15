using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI textDialogue;
    public Canvas dialogueBox;
    public DialogueData dd;
    private float textDelay = .05f;
    private int index = 0;

    public RawImage char1;
    private CharacterExpressionDescriber char1Describer;
    public RawImage char2;
    private CharacterExpressionDescriber char2Describer;


    public void DisableDialogue()
    {
        dialogueBox.enabled = false;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        index = -1;
        NextLine();
    } 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(textDialogue.text == strip_operation(dd.lines[index]))
                NextLine();
            else
            {
                StopAllCoroutines();
                textDialogue.text = strip_operation(dd.lines[index]);
            }
        }
    }

    private IEnumerator TypeLine(string str)
    {
        foreach(char c in str.ToCharArray())
        {
            textDialogue.text += c;
            yield return new WaitForSeconds(textDelay);
        }
    }

    private string get_operation(string s)
    {
        string op = "";
        foreach (char c in s.ToCharArray())
        {
            if (c == ':')
                break;
            op += c;
        }
        return op;
    }

    private string strip_operation(string s)
    {
        return s.Substring(get_operation(s).Length + 2);
    }

    private void NextLine()
    {
        index += 1;
        string op = get_operation(dd.lines[index]);
        switch (op)
        {
            case "SetChars":

                NextLine();
                break;
            case "ChangeExpression":

                NextLine();
                break;
            default:
                if (index < dd.lines.Length - 1)
                {
                    textDialogue.text = "";
                    StartCoroutine(TypeLine(strip_operation(dd.lines[index])));
                }
                break;
        }
    }

    public void ChangeDialogueData(DialogueData ndd)
    {
        dd = ndd;   
    }

    private CharacterExpressionDescriber FindChar(string charName)
    {
        foreach(CharacterExpressionDescriber ced in dd.characters)
        {
            if (ced.name == charName)
                return ced;
        }
        return null;
    }

    public void ChangeChar(int charToChange, string name)
    {
        if(charToChange == 1)
        {
            char1Describer = FindChar(name);
            char1.texture = char1Describer.charExpressionSprite[0];
        }
        else
        {
            char2Describer = FindChar(name);
            char2.texture = char2Describer.charExpressionSprite[0];
        }
    }

    private Texture FindTexture(CharacterExpressionDescriber charDescriber, string expressionName)
    {
        for (int i = 0; i < charDescriber.charExpressionName.Length; i++)
        {
            if (charDescriber.charExpressionName[i] == expressionName)
                return charDescriber.charExpressionSprite[i];
        }

        return null;
    }

    private void ChangeExpression(int charToChange, string expressionName)
    {
        if (charToChange == 1)
            char1.texture = FindTexture(char1Describer, expressionName);
        else
            char2.texture = FindTexture(char2Describer, expressionName);
    }


}
