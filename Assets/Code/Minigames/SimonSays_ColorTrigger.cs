using UnityEngine;

public class SimonSays_ColorTrigger : Interactable
{
    [SerializeField] private SimonSaysController ssc;
    [SerializeField] private SimonSaysColors color;

    private void Start()
    {
        this.interactionStage = 0;
    }

    public override bool Interact()
    {
        if (this.interactionStage < 0) return false;

        ssc.InputColor(color);
        return true;
    }
}
