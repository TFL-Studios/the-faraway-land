using UnityEngine;

public class BoxPizzlePh : Interactable
{
    [SerializeField] private DialogBlock _dialog;

    public override bool Interact()
    {
        GameManager.Instance.StartDialog(this._dialog);
        return true;
    }
}
