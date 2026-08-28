using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimonSays_MainTrigger : Interactable
{
    [SerializeField] private SimonSaysController ssc;

    private void Start()
    {
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
                this.ssc.InitSequence();
                this.interactionStage = 1;
                return true;

            case 1:
                Debug.Log("Game in Progress");
                return true;
        }
    }
}
