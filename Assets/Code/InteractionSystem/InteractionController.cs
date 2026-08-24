using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool isInFirstPerson = false;

    CameraController _cameraController;

    private void Start()
    {
        this._cameraController = this.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (InputHandler.Instance.EntradaInteracao.FoiPressionada)
        {
            if (this.isInFirstPerson)
            {
                if (Physics.Raycast(this._cameraController.MainCamera.transform.position, this._cameraController.MainCamera.transform.forward, out RaycastHit hitInfo, float.MaxValue))
                {
                    IInteractable interactableObj = hitInfo.collider.GetComponent<IInteractable>();

                    if (interactableObj == null) return;
                    interactableObj.Interact();
                }
            }
            else
            {
                // pra dps
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (this._cameraController != null && Physics.Raycast(this._cameraController.MainCamera.transform.position, this._cameraController.MainCamera.transform.forward, out RaycastHit hitInfo, float.MaxValue))
        {
            Gizmos.color = hitInfo.collider.GetComponent<IInteractable>() != null ? Color.green : Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitInfo.point, .2f);
        }
    }
#endif
}
