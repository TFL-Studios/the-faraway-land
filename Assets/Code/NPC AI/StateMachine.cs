using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected interface IState
    {
        public void OnEnter();
        public int OnUpdate();
        public void OnExit();

        public DialogBlock OnInteract();
        public void OnEndDialog(DialogBlock dialogBlock, int dialogOutcome);
    }

    protected IState[] _states;

    protected int _currentState;

    private void OnEnable()
    {
        DialogController.OnEndDialog += OnEndDialog_Callback;
    }

    private void OnDisable()
    {
        DialogController.OnEndDialog -= OnEndDialog_Callback;
    }

    protected virtual void Start()
    {
        this._currentState = 0;
        if (this._states == null) return;
        this._states[this._currentState].OnEnter();
    }

    protected virtual void Update()
    {
        if (this._states == null) return;
        
        int nextState = this._states[this._currentState].OnUpdate();

        if (this._currentState != nextState)
        {
            this._states[this._currentState].OnExit();
            this._currentState = nextState;
            this._states[this._currentState].OnEnter();
            Debug.Log(this._currentState);
        }
    }

    public virtual bool Interact(out DialogBlock dialogBlock)
    {
        dialogBlock = null;
        return false;
    }

    public virtual void OnEndDialog_Callback(DialogBlock dialogBlock, int dialogOutcome) { }
}
