using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    private CameraController _cameraController;

    private List<GameObject> _interactables = new List<GameObject>();
    private GameObject _nearestInteractable;

    private void Start()
    {
        this._cameraController = this.GetComponent<CameraController>();
    }

    private void Update()
    {
        this.FindNearestInteractable();

        if (InputHandler.Instance.EntradaInteracao.FoiPressionada)
        {
            this.TryInteraction();
        }
    }

    private bool TryInteraction()
    {
        if (this._cameraController.IsThirdPerson)
        {
            return this._nearestInteractable && this._nearestInteractable.GetComponent<Interactable>().Interact();
        }

        if (Physics.Raycast(this._cameraController.MainCamera.transform.position, this._cameraController.MainCamera.transform.forward, out RaycastHit hitInfo, float.MaxValue))
        {
            Interactable interactableObj = hitInfo.collider.GetComponent<Interactable>();

            if (interactableObj == null) return false;
            return interactableObj.Interact();
        }

        return false;
    }

    private bool FindNearestInteractable()
    {
        if (this._interactables.Count <= 0)
        {
            this._nearestInteractable = null;
            return false;
        }

        this._nearestInteractable = this._interactables[0];
        for (int index = 1; index < this._interactables.Count; index++)
        {
            float lastDistance = (this._nearestInteractable.transform.position - this.transform.position).magnitude;
            float newDistance = (this._interactables[index].transform.position - this.transform.position).magnitude;

            if (newDistance < lastDistance)
            {
                this._nearestInteractable = this._interactables[index];
            }
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            this._interactables.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            this._interactables.Remove(other.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (this._cameraController != null && Physics.Raycast(this._cameraController.MainCamera.transform.position, this._cameraController.MainCamera.transform.forward, out RaycastHit hitInfo, float.MaxValue))
        {
            Gizmos.color = hitInfo.collider.GetComponent<Interactable>() != null ? Color.green : Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitInfo.point, .2f);

            Gizmos.color = this._nearestInteractable ? Color.green : Color.red;
            Gizmos.DrawWireCube(this._nearestInteractable ? this._nearestInteractable.transform.position + Vector3.up : hitInfo.point, Vector3.one * .5f);
        }
    }
#endif
}
