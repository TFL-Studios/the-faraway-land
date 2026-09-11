using UnityEngine;
using UnityEngine.InputSystem;

public interface ICustomInput
{
    public void RegisterCallbacks();
    public void Enable();
    public void Disable();
}

public class CustomInput<T> : ICustomInput where T : struct
{
    private InputAction _action;

    private T _value;
    public T Value { get { return this._value; } }

    public bool WasPressed { get { return this._action.WasPressedThisFrame(); } }
    public bool IsPressed { get { return this._action.IsPressed(); } }
    public bool WasReleased { get { return this._action.WasReleasedThisFrame(); } }

    public CustomInput(InputAction action)
    {
        this._action = action;
    }

    public void RegisterCallbacks()
    {
        if (typeof(T) == typeof(bool)) return;
        this._action.performed += context => this._value = context.ReadValue<T>();
        this._action.canceled += context => this._value = default(T);
    }

    public void Enable() => this._action.Enable();
    public void Disable() => this._action.Disable();
}

public class InputHandler : PersistentSingleton<InputHandler>
{
    [SerializeField] private InputActionAsset _inputActionAsset;

    /* Custom Inputs */
    private ICustomInput[] _allCustomInputs;
    // User Interface
    private CustomInput<bool> _pauseMenuInput;
    private CustomInput<bool> _inventoryInput;
    private CustomInput<bool> _journalInput;
    private CustomInput<Vector2> _navigationInput;
    private CustomInput<bool> _selectionInput;
    // Player
    private CustomInput<Vector2> _movementInput;
    private CustomInput<bool> _sprintInput;
    private CustomInput<bool> _crouchInput;
    private CustomInput<bool> _interactionInput;
    private CustomInput<bool> _flashlightInput;
    private CustomInput<Vector2> _cameraInput;
    private CustomInput<bool> _pointOfViewInput;

    /* Access */
    // User Interface
    public CustomInput<bool> PauseMenuInput { get { return this._pauseMenuInput; } }
    public CustomInput<bool> InventoryInput { get { return this._inventoryInput; } }
    public CustomInput<bool> JournalInput { get { return this._journalInput; } }
    public CustomInput<Vector2> NavigationInput { get { return this._navigationInput; } }
    public CustomInput<bool> SelectionInput { get { return this._selectionInput; } }
    // Player
    public CustomInput<Vector2> MovementInput { get { return this._movementInput; } }
    public CustomInput<bool> SprintInput { get { return this._sprintInput; } }
    public CustomInput<bool> CrouchInput { get { return this._crouchInput; } }
    public CustomInput<bool> InteractionInput { get { return this._interactionInput; } }
    public CustomInput<bool> FlashlightInput { get { return this._flashlightInput; } }
    public CustomInput<Vector2> CameraInput { get { return this._cameraInput; } }
    public CustomInput<bool> PointOfViewInput { get { return this._pointOfViewInput; } }

    protected override void Awake()
    {
        base.Awake();

        InputActionMap userInterfaceActionMap = this._inputActionAsset.FindActionMap("User Interface");
        InputActionMap playerActionMap = this._inputActionAsset.FindActionMap("Player");

        this._allCustomInputs = new ICustomInput[]
        {
            this._pauseMenuInput = new CustomInput<bool>(userInterfaceActionMap.FindAction("Pause Menu")),
            this._inventoryInput = new CustomInput<bool>(userInterfaceActionMap.FindAction("Inventory")),
            this._journalInput = new CustomInput<bool>(userInterfaceActionMap.FindAction("Journal")),
            this._navigationInput = new CustomInput<Vector2>(userInterfaceActionMap.FindAction("Navigation")),
            this._selectionInput = new CustomInput<bool>(userInterfaceActionMap.FindAction("Selection")),
            
            this._movementInput = new CustomInput<Vector2>(playerActionMap.FindAction("Movement")),
            this._sprintInput = new CustomInput<bool>(playerActionMap.FindAction("Sprint")),
            this._crouchInput = new CustomInput<bool>(playerActionMap.FindAction("Crouch")),
            this._interactionInput = new CustomInput<bool>(playerActionMap.FindAction("Interact")),
            this._flashlightInput = new CustomInput<bool>(playerActionMap.FindAction("Flashlight")),
            this._cameraInput = new CustomInput<Vector2>(playerActionMap.FindAction("Camera")),
            this._pointOfViewInput = new CustomInput<bool>(playerActionMap.FindAction("ChangePOV"))
        };

        this.RegisterInputCallbacks();
    }

    private void OnEnable()
    {
        for (int i = 0; i < this._allCustomInputs.Length; i++)
        {
            this._allCustomInputs[i].Enable();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < this._allCustomInputs.Length; i++)
        {
            this._allCustomInputs[i].Disable();
        }
    }

    private void RegisterInputCallbacks()
    {
        for (int i = 0; i < this._allCustomInputs.Length; i++)
        {
            this._allCustomInputs[i].RegisterCallbacks();
        }
    }
}
