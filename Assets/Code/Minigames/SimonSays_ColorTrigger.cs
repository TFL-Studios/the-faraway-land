using UnityEngine;

public class SimonSays_ColorTrigger : Interactable
{
    [SerializeField] private SimonSaysColors color;
    
    private SimonSaysController _simonSaysController;

    private void Awake()
    {
        this._simonSaysController = GameObject.FindAnyObjectByType<SimonSaysController>();
    }

    protected override void Start()
    {
        base.Start();

        this.interactionStage = 0;
    }

    public override bool Interact()
    {
        if (this.interactionStage < 0) return false;

        this._simonSaysController.InputColor(color);
        return true;
    }
}
