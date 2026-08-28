using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Transform _interactionPopupAnchor;

    public int interactionStage = -1; // -1 inactive, 0... active
    
    public virtual bool Interact()
    {
        Debug.LogWarning("Interactable not overriden");
        return false;
    }
}
