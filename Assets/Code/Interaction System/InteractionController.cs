using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float _interactionDistance;

    private CameraController _cameraController;
    private InterationPopupController _interactionPopupController;

    private List<Interactable> _interactables = new List<Interactable>();
    private Interactable _targetInteractable;

    private void Awake()
    {
        this._cameraController = this.GetComponent<CameraController>();
        this._interactionPopupController = GameObject.FindAnyObjectByType<InterationPopupController>();
    }

    private void Update()
    {
        if (this.UpdateTargetInteractable())
        {
            this._interactionPopupController.SetTarget(this._targetInteractable ? this._targetInteractable.InteractionPopupAnchor : null);
        }

        if (InputHandler.Instance.EntradaInteracao.FoiPressionada)
        {
            this.TryInteraction();
        }
    }

    private bool TryInteraction()
    {
        return this._targetInteractable && this._targetInteractable.GetComponent<Interactable>().Interact();
    }

    private Interactable FindNearestInteractable()
    {
        if (this._interactables.Count <= 0)
        {
            return null;
        }

        Interactable nearest = this._interactables[0];
        for (int index = 1; index < this._interactables.Count; index++)
        {
            float lastDistance = (nearest.transform.position - (this.transform.position + this.transform.forward)).magnitude;
            float newDistance = (this._interactables[index].transform.position - (this.transform.position + this.transform.forward)).magnitude;

            if (newDistance >= lastDistance) continue;
            nearest = this._interactables[index];
        }

        return nearest;
    }

    private bool UpdateTargetInteractable()
    {
        Interactable nearest = this.FindNearestInteractable();
        bool hasChanged = this._targetInteractable != nearest;
        bool isInRange = nearest && (nearest.transform.position - (this.transform.position + this.transform.forward)).magnitude <= this._interactionDistance;
        this._targetInteractable = isInRange ? nearest : null;
        return hasChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable)
        {
            this._interactables.Add(interactable);
            this._interactionPopupController.RegisterInRange(interactable.InteractionPopupAnchor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable)
        {
            this._interactables.Remove(interactable);
            this._interactionPopupController.UnregisterFromRange(interactable.InteractionPopupAnchor);
        }
    }
}
