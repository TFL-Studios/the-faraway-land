using UnityEngine;

public enum ControllMode
{
    FreeRoam, // HUD
    UI_Dialog,
    UI_Inventory,
    UI_Journal,
}

public class GeneralUIController : MonoBehaviour
{
    private InterfaceNode[] _interfaceNodes;

    private HeadsUpDisplayUIController _hudUI;
    private DialogController _dialogUI;
    private Inventory _inventoryUI;
    private JournalUIController _journalUI;

    private ControllMode _currentControlMode;
    public ControllMode CurrentControlMode { get { return this._currentControlMode; } }

    private void Awake()
    {
        this._hudUI = this.GetComponentInChildren<HeadsUpDisplayUIController>();
        this._dialogUI = this.GetComponentInChildren<DialogController>();
        this._inventoryUI = this.GetComponentInChildren<Inventory>();
        this._journalUI = this.GetComponentInChildren<JournalUIController>();
    }

    private void Start()
    {
        this._interfaceNodes = new InterfaceNode[]
        {
            this._hudUI,
            this._dialogUI,
            this._inventoryUI,
            this._journalUI,
        };

        this._currentControlMode = ControllMode.FreeRoam;
    }

    private void Update()
    {
        if (this._currentControlMode == ControllMode.UI_Dialog) return; // ph

        if (InputHandler.Instance.InventoryInput.WasPressed)
        {
            this.ChangeActiveUI(this._currentControlMode == ControllMode.UI_Inventory ? ControllMode.FreeRoam : ControllMode.UI_Inventory);
        }

        if (InputHandler.Instance.JournalInput.WasPressed)
        {
            this.ChangeActiveUI(this._currentControlMode == ControllMode.UI_Journal ? ControllMode.FreeRoam : ControllMode.UI_Journal);
        }
    }

    private void ChangeActiveUI(ControllMode nextControlMode)
    {
        this._interfaceNodes[(int)this._currentControlMode].SetNodeUI(false);
        this._currentControlMode = nextControlMode;
        this._interfaceNodes[(int)this._currentControlMode].SetNodeUI(true);

        InputHandler.Instance.SetActiveMap(this._currentControlMode != ControllMode.FreeRoam ? InputMap.UserInterface : InputMap.Player);
    }

    public void EnableDialogUI(DialogBlock dialogBlock)
    {
        this.ChangeActiveUI(ControllMode.UI_Dialog);
        this._dialogUI.InitDialog(dialogBlock);
    }

    public void DisableDialogUI()
    {
        this.ChangeActiveUI(ControllMode.FreeRoam);
    }
}
