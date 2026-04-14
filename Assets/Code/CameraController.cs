using System.Reflection;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(5f, 85f)]
    [SerializeField] private float _maxLookDownAngle = 85f;
    [Range(275, 355f)]
    [SerializeField] private float _maxLookUpAngle = 275f;

    private GameObject _cameraGimblePrefab;
    private GameObject _cameraGimbleInstance;

    private void Awake()
    {
        this._cameraGimblePrefab = Resources.Load<GameObject>("CameraGimble");
    }

    private void Start()
    {
        this.InitCameraGimble();
    }

    private void Update()
    {
        Vector2 mouseDelta = InputHandler.Instance.EntradaVisao.Valor;
        
        // Horizontal Rotation
        Vector3 playerRotation = this.transform.localRotation.eulerAngles;
        playerRotation.y += mouseDelta.x;

        // Vertical Rotation
        Vector3 gimbleRotation = this._cameraGimbleInstance.transform.localRotation.eulerAngles;
        gimbleRotation.x -= mouseDelta.y;
        gimbleRotation.y = 0f;
        gimbleRotation.z = 0f;
        // Clamps
        if (gimbleRotation.x <= 180f && gimbleRotation.x > this._maxLookDownAngle) gimbleRotation.x = this._maxLookDownAngle;
        if (gimbleRotation.x > 180f && gimbleRotation.x < this._maxLookUpAngle) gimbleRotation.x = this._maxLookUpAngle;

        // Apply Rotations
        this.transform.localRotation = Quaternion.Euler(playerRotation);
        this._cameraGimbleInstance.transform.localRotation = Quaternion.Euler(gimbleRotation);
    }

    private void InitCameraGimble()
    {
        if (this._cameraGimbleInstance) return;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject currentChild = this.transform.GetChild(i).gameObject;
            if (currentChild.name != "CameraGimble") continue;
            this._cameraGimbleInstance = currentChild;
            return;
        }

        if (this._cameraGimblePrefab)
        {
            this._cameraGimbleInstance = GameObject.Instantiate(this._cameraGimblePrefab, this.transform);
            return;
        }

        GameObject cameraGimble = new GameObject("CameraGimble");
        cameraGimble.transform.parent = this.transform;
        cameraGimble.transform.localPosition = Vector3.zero;
        cameraGimble.transform.localRotation = Quaternion.identity;
        cameraGimble.transform.localScale = Vector3.one * 5f;

        CinemachineCamera firstPersonCamera = new GameObject("FirstPersonCamera").AddComponent<CinemachineCamera>();
        firstPersonCamera.transform.parent = cameraGimble.transform;
        firstPersonCamera.transform.localPosition = Vector3.zero;
        firstPersonCamera.transform.localRotation = Quaternion.identity;

        CinemachineCamera thirdPersonCamera = new GameObject("ThirdPersonCamera").AddComponent<CinemachineCamera>();
        thirdPersonCamera.transform.parent = cameraGimble.transform;
        thirdPersonCamera.transform.localPosition = Vector3.back * .5f;
        thirdPersonCamera.transform.localRotation = Quaternion.identity;

        this._cameraGimbleInstance = cameraGimble;
    }
}
