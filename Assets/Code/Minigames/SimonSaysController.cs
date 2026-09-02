using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SimonSaysColors
{
    Red,
    Green,
    Blue,
    Yellow,
    _COUNT,
}

public enum SimonSaysStage
{
    Off,
    Initializing,
    DisplayingColors,
    WaitingForInput,
    Done,
}

public class SimonSaysController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _tvs;
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    [SerializeField] private Material blue;
    [SerializeField] private Material yellow;
    [SerializeField] private Material black;
    [SerializeField] private Material white;

    private List<SimonSaysColors> _currentSequence = new List<SimonSaysColors>();

    private int _sequenceLength = 10; // full sequence length
    private int _sequenceStep = 1; // section length
    private int _sequenceInput = 0; // index

    private SimonSaysStage _currentStage = SimonSaysStage.Off;

    public void InitSequence()
    {
        this._currentStage = SimonSaysStage.Initializing;

        this._currentSequence.Clear();
        for (int i = 0; i < this._sequenceLength; i++)
        {
            int color = Random.Range((int)SimonSaysColors.Red, (int)SimonSaysColors._COUNT);
            this._currentSequence.Add((SimonSaysColors)color);
        }
        this._sequenceStep = 1;
        this._sequenceInput = 0;

        this.StartCoroutine(this.DisplayColorSequence());
    }

    public Material GetColorMaterial(SimonSaysColors color)
    {
        switch (color)
        {
            default:
                return this.white;
            case SimonSaysColors.Red:
                return this.red;
            case SimonSaysColors.Green:
                return this.green;
            case SimonSaysColors.Blue:
                return this.blue;
            case SimonSaysColors.Yellow:
                return this.yellow;
        }
    }

    public IEnumerator DisplayColorSequence()
    {
        this._currentStage = SimonSaysStage.DisplayingColors;

        for (int i = 0; i < this._sequenceStep; i++)
        {
            yield return new WaitForSeconds(1);
            foreach (SpriteRenderer tv in this._tvs) { tv.material = this.black; }
            yield return new WaitForSeconds(.5f);
            foreach (SpriteRenderer tv in this._tvs) { tv.material = this.GetColorMaterial(this._currentSequence[i]); }
        }

        yield return new WaitForSeconds(1);
        foreach (SpriteRenderer tv in this._tvs) { tv.material = this.black; }
        yield return new WaitForSeconds(.5f);
        foreach (SpriteRenderer tv in this._tvs) { tv.material = this.white; }

        this._currentStage = SimonSaysStage.WaitingForInput;
    }

    public void InputColor(SimonSaysColors selectedColor)
    {
        if (this._currentStage != SimonSaysStage.WaitingForInput) return;

        if (selectedColor != this._currentSequence[this._sequenceInput])
        {
            Debug.Log("ERROU, COMECA DO ZERO");
            this.InitSequence();
            return;
        }

        this._sequenceInput++;
        foreach (SpriteRenderer tv in this._tvs) { tv.material = this.GetColorMaterial(selectedColor); }

        if (this._sequenceInput >= this._sequenceLength)
        {
            Debug.Log("GANHO!");
            this._currentStage = SimonSaysStage.Done;
            return;
        }

        if (this._sequenceInput >= this._sequenceStep)
        {
            Debug.Log("CORRETO; MAIS UMA COR!");
            this._sequenceStep++;
            this._sequenceInput = 0;

            this.StartCoroutine(this.DisplayColorSequence());
            return;
        }
    }
}
