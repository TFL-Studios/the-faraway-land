using System.Collections.Generic;
using UnityEngine;

public class InterationPopupController : MonoBehaviour
{
    [SerializeField] private GameObject _openPopup;
    [SerializeField] private GameObject _closedPopupPrefab;
    [SerializeField] private GameObject _closedPopupPanel;

    private List<GameObject> _closedPopups = new List<GameObject>();

    private CameraController _cameraController;

    private Transform _targetAnchor;
    private List<Transform> _anchorsInRange = new List<Transform>();

    private void Awake()
    {
        this._cameraController = GameObject.FindAnyObjectByType<CameraController>();
    }

    private void Update()
    {
        for (int i = 0; i < Mathf.Max(this._anchorsInRange.Count, this._closedPopups.Count); i++)
        {
            if (i >= this._closedPopups.Count)
            {
                this._closedPopups.Add(GameObject.Instantiate(this._closedPopupPrefab, this._closedPopupPanel.transform));
            }

            if (i >= this._anchorsInRange.Count)
            {
                this._closedPopups[i].SetActive(false);
                continue;
            }

            this._closedPopups[i].transform.position = this._cameraController.MainCamera.OutputCamera.WorldToScreenPoint(this._anchorsInRange[i].position);
            this._closedPopups[i].SetActive(this._anchorsInRange[i] != this._targetAnchor);
        }

        if (this._targetAnchor)
        {
            this._openPopup.transform.position = this._cameraController.MainCamera.OutputCamera.WorldToScreenPoint(this._targetAnchor.position);
        }
        this._openPopup.SetActive(this._targetAnchor);
    }

    public void SetTarget(Transform interactableAnchor)
    {
        this._targetAnchor = interactableAnchor;
    }

    public void RegisterInRange(Transform interactableAnchor)
    {
        this._anchorsInRange.Add(interactableAnchor);
    }

    public void UnregisterFromRange(Transform interactableAnchor)
    {
        this._anchorsInRange.Remove(interactableAnchor);
    }
}
