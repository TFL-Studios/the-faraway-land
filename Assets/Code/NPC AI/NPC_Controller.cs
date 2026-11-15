using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Controller : Interactable
{
    [Header("NPC")]
    [SerializeField] private CharacterData _characterData;

    private StateMachine _charStateMachine;

    [Header("GameObject Config")]
    [SerializeField] private SpriteRenderer _npcGraphics;

    public override bool Interact()
    {
        bool flag = this._charStateMachine.Interact(out DialogBlock interactionDialog);
        
        DialogBlock dialog = flag ? interactionDialog : this._characterData.genericDialog;

        GameManager.Instance.StartDialog(dialog);
        return true;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string name = "NPC";

        if (this._characterData)
        {
            name = this._characterData.characterName.ToUpper().Replace(" ", "_");
            this._npcGraphics.sprite = this._characterData.characterSprite;

            if (this._characterData.stateMachiune)
            {
                System.Type stateMachineType = this._characterData.stateMachiune.GetClass();

                if (this._charStateMachine == null)
                {
                    StateMachine result = this.gameObject.GetComponent(stateMachineType) as StateMachine;
                    if (result == null) result = this.gameObject.AddComponent(stateMachineType) as StateMachine;
                    this._charStateMachine = result;
                }
                else if (this._charStateMachine.GetType() != stateMachineType)
                {
                    StateMachine targetStateMachine = this._charStateMachine;
                    UnityEditor.EditorApplication.delayCall += () => UnityEditor.Undo.DestroyObjectImmediate(targetStateMachine);
                    this._charStateMachine = this.gameObject.AddComponent(stateMachineType) as StateMachine;
                }
            }
            else if (this._charStateMachine != null)
            {
                UnityEditor.EditorApplication.delayCall += () => UnityEditor.Undo.DestroyObjectImmediate(this._charStateMachine);
            }
        }

        this.name = $"{name}_Controller";
        this.transform.GetChild(0).name = $"{name}_Graphic";
    }
#endif
}
