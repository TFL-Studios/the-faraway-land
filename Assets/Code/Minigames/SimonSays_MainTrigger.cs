using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimonSays_MainTrigger : Interactable
{
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
        switch (this.interactionStage)
        {
            default:
                return false;

            case 0:
                Debug.Log("Creating Sequence");
                this._simonSaysController.InitSequence();
                this.interactionStage = 1;
                return true;

            case 1:
                Debug.Log("Game in Progress");
                return true;
        }
    }
}
