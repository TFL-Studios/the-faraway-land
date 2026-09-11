using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Transform _interactionPopupAnchor;
    public Transform InteractionPopupAnchor { get { return this._interactionPopupAnchor; } }

    [HideInInspector] public int interactionStage = -1; // -1 inactive, 0... active
    
    protected virtual void Start()
    {
        if (!this._interactionPopupAnchor)
        {
            this._interactionPopupAnchor = new GameObject("InteractionPopupAnchor").transform;
            this._interactionPopupAnchor.parent = this.transform;
            this._interactionPopupAnchor.localPosition = Vector3.up;
        }
    }

    public virtual bool Interact()
    {
        Debug.LogWarning("Interactable not overriden");
        return false;
    }
}
