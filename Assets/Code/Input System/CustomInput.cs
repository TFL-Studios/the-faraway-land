using UnityEngine.InputSystem;

public class CustomInput<TValue> : ICustomInput where TValue : struct
{
    private InputAction _inputAction;

    private TValue _value;
    public TValue Value { get { return this._value; } }

    public bool WasPressed { get { return this._inputAction.WasPressedThisFrame(); } }
    public bool IsPressed { get { return this._inputAction.IsPressed(); } }
    public bool WasReleased { get { return this._inputAction.WasReleasedThisFrame(); } }

    public CustomInput(InputAction inputAction)
    {
        this._inputAction = inputAction;
    }

    public void RegisterCallbacks()
    {
        if (typeof(TValue) == typeof(bool)) return;
        this._inputAction.performed += context => this._value = context.ReadValue<TValue>();
        this._inputAction.canceled += context => this._value = default(TValue);
    }

    public void Enable() => this._inputAction.Enable();
    public void Disable() => this._inputAction.Disable();
}