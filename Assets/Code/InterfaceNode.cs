using UnityEngine;

public abstract class InterfaceNode : MonoBehaviour
{
    [SerializeField] protected GameObject _nodeUIPanel;

    public virtual void SetNodeUI(bool active)
    {
        this._nodeUIPanel.SetActive(active);
    }
}
