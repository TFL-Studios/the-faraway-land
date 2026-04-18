using System.Reflection;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Primeira Pessoa")]
    [Range(5f, 85f)]
    [SerializeField] private float _firstPerson_maxLookDownAngle = 85f;
    [Range(275, 355f)]
    [SerializeField] private float _firstPerson_maxLookUpAngle = 275f;
    [SerializeField] private Vector2 _firstPerson_sensitivity = new Vector2(50f, 50f); // TODO: botar como preferencia pro player editar

    [Header("Terceira Pessoa")]
    [Range(5f, 85f)]
    [SerializeField] private float _thirdPerson_maxLookDownAngle = 85f;
    [Range(275, 355f)]
    [SerializeField] private float _thirdPerson_maxLookUpAngle = 275f;
    [SerializeField] private Vector2 _thirdPerson_sensitivity = new Vector2(50f, 50f); // TODO: botar como preferencia pro player editar

    private GameObject _cameraGimblePrefab;
    private GameObject _cameraGimbleInstance;

    private bool _isThirdPerson = false;
    private CinemachineCamera _firstPersonCamera;
    private CinemachineCamera _thirdPersonCamera;

    private float _cameraDistance = 2.5f; // TODO: botar como preferencia pro player editar, talvez

    private void Awake()
    {
        this._cameraGimblePrefab = Resources.Load<GameObject>("CameraGimble");
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        this.InitCameraGimble();

        this._firstPersonCamera = this._cameraGimbleInstance.transform.GetChild(0).GetComponent<CinemachineCamera>();
        this._thirdPersonCamera = this._cameraGimbleInstance.transform.GetChild(1).GetComponent<CinemachineCamera>();

        if (this._isThirdPerson) { this._thirdPersonCamera.Prioritize(); }
        else { this._firstPersonCamera.Prioritize(); }
    }

    private void Update()
    {
        // Inputs
        if (InputHandler.Instance.EntradaPOV.FoiPressionada)
        {
            this._isThirdPerson = !this._isThirdPerson;
            if (this._isThirdPerson) { this._thirdPersonCamera.Prioritize(); }
            else { this._firstPersonCamera.Prioritize(); }
        }
        Vector2 mouseDelta = InputHandler.Instance.EntradaVisao.Valor;
        
        // Horizontal Rotation
        Vector3 playerRotation = this.transform.localRotation.eulerAngles;
        playerRotation.y += mouseDelta.x * Time.deltaTime * (this._isThirdPerson ? this._thirdPerson_sensitivity : this._firstPerson_sensitivity).x;

        // Vertical Rotation
        Vector3 gimbleRotation = this._cameraGimbleInstance.transform.localRotation.eulerAngles;
        gimbleRotation.x -= mouseDelta.y * Time.deltaTime * (this._isThirdPerson ? this._thirdPerson_sensitivity : this._firstPerson_sensitivity).y;
        gimbleRotation.y = 0f;
        gimbleRotation.z = 0f;
        
        // Clamps
        float maxLookDownAngle = this._isThirdPerson ? this._thirdPerson_maxLookDownAngle : this._firstPerson_maxLookDownAngle;
        float maxLookUpAngle = this._isThirdPerson ? this._thirdPerson_maxLookUpAngle : this._firstPerson_maxLookUpAngle;
        
        if (gimbleRotation.x <= 180f && gimbleRotation.x > maxLookDownAngle) gimbleRotation.x = this._thirdPerson_maxLookDownAngle;
        if (gimbleRotation.x > 180f && gimbleRotation.x < maxLookUpAngle) gimbleRotation.x = this._thirdPerson_maxLookUpAngle;

        // Apply Rotations
        this.transform.localRotation = Quaternion.Euler(playerRotation);
        this._cameraGimbleInstance.transform.localRotation = Quaternion.Euler(gimbleRotation);

        // Avoid Camera Clipping
        this._cameraGimbleInstance.transform.localScale = Vector3.one * this.CalculateCameraDistance();
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
#if UNITY_EDITOR
        Debug.Log("<color=#ff0000>[!] Prefab do CameraGimble nao encontrada na pasta Resources</color>");
#endif
    }

    private float CalculateCameraDistance()
    {
        float result = this._cameraDistance;

        if (this._isThirdPerson && Physics.Raycast(this._cameraGimbleInstance.transform.position, -this._cameraGimbleInstance.transform.forward, out RaycastHit hitInfo, this._cameraDistance))
        {
            Debug.Log(hitInfo.distance);
            result = hitInfo.distance;
        }

        return result * 2;
    }
}
