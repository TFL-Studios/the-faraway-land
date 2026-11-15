using UnityEngine;
using UnityEngine.InputSystem;

public enum EInputMap
{
    UserInterface,
	Player,
}

public class InputHandler : PersistentSingleton<InputHandler>
{
    private InputActionAsset _inputActionAsset;
    
    /* Maps */
    private InputActionMap _mapUserInterface;
	private InputActionMap _mapPlayer;

    /* Custom Inputs */
    private ICustomInput[] _allCustomInputs;
    // User Interface Map
	private CustomInput<bool> _inputPauseMenu;
	private CustomInput<bool> _inputInventory;
	private CustomInput<bool> _inputJournal;
	private CustomInput<Vector2> _inputNavigation;
	private CustomInput<bool> _inputSelection;
	// Player Map
	private CustomInput<Vector2> _inputMovement;
	private CustomInput<bool> _inputSprint;
	private CustomInput<bool> _inputCrouch;
	private CustomInput<bool> _inputInteract;
	private CustomInput<bool> _inputFlashlight;
	private CustomInput<Vector2> _inputCamera;
	private CustomInput<bool> _inputChangePOV;
    
    /* Access */
    // User Interface Map
	public CustomInput<bool> PauseMenuInput { get { return this._inputPauseMenu; } }
	public CustomInput<bool> InventoryInput { get { return this._inputInventory; } }
	public CustomInput<bool> JournalInput { get { return this._inputJournal; } }
	public CustomInput<Vector2> NavigationInput { get { return this._inputNavigation; } }
	public CustomInput<bool> SelectionInput { get { return this._inputSelection; } }
	// Player Map
	public CustomInput<Vector2> MovementInput { get { return this._inputMovement; } }
	public CustomInput<bool> SprintInput { get { return this._inputSprint; } }
	public CustomInput<bool> CrouchInput { get { return this._inputCrouch; } }
	public CustomInput<bool> InteractInput { get { return this._inputInteract; } }
	public CustomInput<bool> FlashlightInput { get { return this._inputFlashlight; } }
	public CustomInput<Vector2> CameraInput { get { return this._inputCamera; } }
	public CustomInput<bool> ChangePOVInput { get { return this._inputChangePOV; } }
    
    protected override void Awake()
    {
        base.Awake();
        
        if (!this._inputActionAsset)
        {
            this._inputActionAsset = Resources.Load<InputActionAsset>("InputSystem_Actions");
        }

        this._mapUserInterface = this._inputActionAsset.FindActionMap("User Interface");
		this._mapPlayer = this._inputActionAsset.FindActionMap("Player");
        
        this._allCustomInputs = new ICustomInput[]
        {
            // User Interface Map
			this._inputPauseMenu = new CustomInput<bool>(_mapUserInterface.FindAction("Pause Menu")),
			this._inputInventory = new CustomInput<bool>(_mapUserInterface.FindAction("Inventory")),
			this._inputJournal = new CustomInput<bool>(_mapUserInterface.FindAction("Journal")),
			this._inputNavigation = new CustomInput<Vector2>(_mapUserInterface.FindAction("Navigation")),
			this._inputSelection = new CustomInput<bool>(_mapUserInterface.FindAction("Selection")),
			// Player Map
			this._inputMovement = new CustomInput<Vector2>(_mapPlayer.FindAction("Movement")),
			this._inputSprint = new CustomInput<bool>(_mapPlayer.FindAction("Sprint")),
			this._inputCrouch = new CustomInput<bool>(_mapPlayer.FindAction("Crouch")),
			this._inputInteract = new CustomInput<bool>(_mapPlayer.FindAction("Interact")),
			this._inputFlashlight = new CustomInput<bool>(_mapPlayer.FindAction("Flashlight")),
			this._inputCamera = new CustomInput<Vector2>(_mapPlayer.FindAction("Camera")),
			this._inputChangePOV = new CustomInput<bool>(_mapPlayer.FindAction("ChangePOV")),
        };
        
        for (int i = 0; i < this._allCustomInputs.Length; i++)
        {
            this._allCustomInputs[i].RegisterCallbacks();
        }
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
    
    public bool SetMapEnabled(EInputMap map, bool enable)
    {
        switch (map)
        {
            case EInputMap.UserInterface: if (enable) _mapUserInterface.Enable(); else _mapUserInterface.Disable(); break;
			case EInputMap.Player: if (enable) _mapPlayer.Enable(); else _mapPlayer.Disable(); break;
            default: return false;
        }
        return true;
    }
}
