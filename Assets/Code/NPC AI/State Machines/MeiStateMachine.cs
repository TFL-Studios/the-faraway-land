using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MeiStateMachine : StateMachine
{
    private enum EMeiStates
    {
        FirstEncounter,
        // Sad Route
        Stalking,
        WeepingAngel,
        // Happy Route
        HideAndSeek,
        Roaming,
        SecrectRoom,
    }

    [Header("Generic")]
    [SerializeField] private bool _interactable;

    [Header("First Encounter")]
    [SerializeField] private Transform _meetLocation;
    [SerializeField] private DialogBlock _meetDialog;

    [Header("Stalking")]
    [SerializeField] private Transform[] _standingCorners;
    [SerializeField] private DialogBlock _secondChanceDialog;
    [SerializeField] private DialogBlock _lastChanceDialog;

    [Header("HideAndSeek")]
    [SerializeField] private Transform _ballpitHideSpot;
    [SerializeField] private Transform _arcadeHideSpot;
    [SerializeField] private Transform _foodcourtHideSpot;
    [SerializeField] private DialogBlock _ballpitDialog;
    [SerializeField] private DialogBlock _arcadeDialog;
    [SerializeField] private DialogBlock _foodcourtDialog;
    [SerializeField] private DialogBlock _finishHideAndSeekDialog;

    [Header("Roaming")]
    [SerializeField] private DialogBlock _secretRoomUnlockDialog; // TODO: ph

    [Header("SecretRoom")]
    [SerializeField] private Transform _secretRoomLocation;
    [SerializeField] private DialogBlock _secretRoomDialog;

    protected override void Start()
    {
        this._states = new IState[]
        {
            new FirstEncounter(this.transform, this._meetLocation, this._meetDialog),
            new Stalking(this.transform, this._standingCorners, this._secondChanceDialog, this._lastChanceDialog),
            new WeepingAngel(this.transform, GameObject.FindGameObjectWithTag("Player").transform),
            new HideAndSeek(this.transform, this._ballpitHideSpot, this._arcadeHideSpot, this._foodcourtHideSpot, this._ballpitDialog, this._arcadeDialog, this._foodcourtDialog, this._finishHideAndSeekDialog),
            new Roaming(this.transform, GameObject.FindGameObjectWithTag("Player").transform, this._secretRoomUnlockDialog), // TODO: target eh só pra teste
            new SecretRoom(this.transform, this._secretRoomLocation, this._secretRoomDialog),
        };

        base.Start();
    }

    public override bool Interact(out DialogBlock dialogBlock)
    {
        dialogBlock = null;
        if (!this._interactable) return false;

        dialogBlock = this._states[this._currentState].OnInteract();
        return true;
    }

    public override void OnEndDialog_Callback(DialogBlock dialogBlock, int dialogOutcome)
    {
        this._states[this._currentState].OnEndDialog(dialogBlock, dialogOutcome);
    }

    private class FirstEncounter : IState
    {
        private EMeiStates _nextStage;

        private Transform _self;

        private Transform _meetLocation;
        private DialogBlock _meetDialog;

        public FirstEncounter(Transform self, Transform meetLocation, DialogBlock meetDialog)
        {
            this._self = self;
            this._meetLocation = meetLocation;
            this._meetDialog = meetDialog;
        }

        public void OnEnter()
        {
            this._self.position = this._meetLocation.position;
            this._nextStage = EMeiStates.FirstEncounter;
        }

        public int OnUpdate()
        {
            return (int)this._nextStage;
        }

        public void OnExit()
        {

        }

        public DialogBlock OnInteract()
        {
            return this._meetDialog;
        }

        public void OnEndDialog(DialogBlock dialogBlock, int dialogOutcome)
        {
            if (dialogBlock != this._meetDialog) { return; }

            this._nextStage = dialogOutcome == 0 ? EMeiStates.HideAndSeek : EMeiStates.Stalking;    
        }
    }

    private class Stalking : IState
    {
        private EMeiStates _nextStage;

        private Transform _self;
        private Transform[] _standingCorners;
        private DialogBlock _secondChanceDialog;
        private DialogBlock _lastChanceDialog;
        private bool _rejected;
        private bool _isTalking;
        private int _currentCorner;
        private float _relocateTimer;

        public Stalking(Transform self, Transform[] standingCorners, DialogBlock secondChanceDialog, DialogBlock lastChanceDialog)
        {
            this._self = self;
            this._standingCorners = standingCorners;
            this._secondChanceDialog = secondChanceDialog;
            this._lastChanceDialog = lastChanceDialog;
            this._rejected = false;
            this._isTalking = false;
            this._currentCorner = 0;
            this._relocateTimer = 0f;
        }

        public void OnEnter()
        {
            this._nextStage = EMeiStates.Stalking;
            this.Relocate();
        }

        public int OnUpdate()
        {
            this._relocateTimer += Time.deltaTime;
            if (this._relocateTimer >= 3f && !this._isTalking) // TODO: mudar e randomizar valor
            {
                this.Relocate();
                this._relocateTimer = 0f;
            }

            // fica nos cantinho te encarando
            return (int)this._nextStage;
        }

        public void OnExit()
        {

        }

        public DialogBlock OnInteract()
        {
            this._isTalking = true;
            return this._rejected ? this._lastChanceDialog : this._secondChanceDialog;
        }

        public void OnEndDialog(DialogBlock dialogBlock, int dialogOutcome)
        {
            if (dialogBlock != this._secondChanceDialog && dialogBlock != this._lastChanceDialog) { return; }

            this._isTalking = false;
            this._nextStage = dialogOutcome == 0 ? EMeiStates.HideAndSeek : (!this._rejected ? EMeiStates.Stalking : EMeiStates.WeepingAngel);
            this._rejected = dialogOutcome == 1;
        }

        private void Relocate()
        {
            int nextCorner = 0;
            
            do
            {
                nextCorner = UnityEngine.Random.Range(0, this._standingCorners.Length);
            } while (nextCorner == this._currentCorner);

            this._currentCorner = nextCorner;
            this._self.position = this._standingCorners[this._currentCorner].position;
        }
    }

    private class WeepingAngel : IState
    {
        private EMeiStates _nextStage;

        private Transform _self;
        private Transform _target;

        public WeepingAngel(Transform self, Transform target)
        {
            this._self = self;
            this._target = target;
        }

        public void OnEnter()
        {
            this._nextStage = EMeiStates.WeepingAngel;
        }

        public int OnUpdate()
        {
            Vector3 direction = this._target.position - this._self.position;
            direction.y = 0f;
            float dotProd = Vector3.Dot(this._target.forward, direction);
            Debug.Log(dotProd);
            if (dotProd > 0)
            {
                this._self.position += direction.normalized * 2f * Time.deltaTime;
            }


            if (direction.magnitude <= 1f)
            {
                float X = UnityEngine.Random.Range(-1f, 1f);
                float Z = UnityEngine.Random.Range(-1f, 1f);
                Vector3 jumpDirection = new Vector3(X, 0, Z);

                this._self.position += jumpDirection.normalized * 20f;
            }

            Debug.Log(direction);

            return (int)this._nextStage;
        }

        public void OnExit()
        {

        }

        public DialogBlock OnInteract()
        {
            return null;
        }

        public void OnEndDialog(DialogBlock dialogBlock, int dialogOutcome)
        {
            return;
        }
    }

    private class HideAndSeek : IState
    {
        private EMeiStates _nextStage;

        private Transform _self;
        private DialogBlock _finalDialog;
        private (Transform, DialogBlock)[] _hidespots;
        private List<int> _completedHidespots;
        private int _currentHidespot = -1;

        public HideAndSeek(Transform self, Transform ballpitHideSpot, Transform arcadeHideSpot, Transform foodcourtHideSpot, DialogBlock ballpitDialog, DialogBlock arcadeDialog, DialogBlock foodcourtDialog, DialogBlock finalDialog)
        {
            this._self = self;
            this._hidespots = new (Transform, DialogBlock)[3]
            {
                (ballpitHideSpot, ballpitDialog),
                (arcadeHideSpot, arcadeDialog),
                (foodcourtHideSpot, foodcourtDialog),
            };
            this._finalDialog = finalDialog;

            this._completedHidespots = new List<int>();
        }

        public void OnEnter()
        {
            this.MoveToNextHidespot();
            this._nextStage = EMeiStates.HideAndSeek;
        }

        public int OnUpdate()
        {
            return (int)this._nextStage;
        }

        public void OnExit()
        {

        }

        public DialogBlock OnInteract()
        {
            return this._hidespots[this._currentHidespot].Item2;
        }

        public void OnEndDialog(DialogBlock dialogBlock, int dialogOutcome)
        {
            bool hidespotDialogFlag = false;

            if (dialogBlock == this._hidespots[0].Item2)
            {
                this._completedHidespots.Add(0);
                hidespotDialogFlag = true;
            }

            if (dialogBlock == this._hidespots[1].Item2)
            {
                this._completedHidespots.Add(1);
                hidespotDialogFlag = true;
            }

            if (dialogBlock == this._hidespots[2].Item2)
            {
                this._completedHidespots.Add(2);
                hidespotDialogFlag = true;
            }

            if (hidespotDialogFlag)
            {
                if (this._completedHidespots.Count < this._hidespots.Length)
                {
                    this.MoveToNextHidespot();
                    return;
                }
                GameManager.Instance.StartDialog(this._finalDialog);
                return;
            }

            if (dialogBlock == this._finalDialog)
            {
                this._nextStage = EMeiStates.Roaming;
                return;
            }
        }

        private void MoveToNextHidespot()
        {
            if (this._completedHidespots.Count == this._hidespots.Length) { return; }

            int nextHidespot = -1;
            do
            {
                nextHidespot = UnityEngine.Random.Range(0, this._hidespots.Length);
            } while (this._completedHidespots.Contains(nextHidespot));

            this._currentHidespot = nextHidespot;
            this._self.position = this._hidespots[this._currentHidespot].Item1.position;
        }
    }

    private class Roaming : IState
    {
        private EMeiStates _nextStage;

        private Transform _self;
        private Transform _target;

        private DialogBlock _dialog;

        private float x = 1;
        private float xm = 1;
        private float y = 1;
        private float ym = 0;

        public Roaming(Transform self, Transform target, DialogBlock dialog)
        {
            this._self = self;
            this._target = target;

            this._dialog = dialog;
        }

        public void OnEnter()
        {
            this._nextStage = EMeiStates.Roaming;
        }

        public int OnUpdate()
        {
            int speed = 5;

            x = Math.Clamp(x + Time.deltaTime * xm * speed, -1, 1);
            y = Math.Clamp(y + Time.deltaTime * ym * speed, -1, 1);

            if (xm != 0 && Math.Abs(x) >= 1)
            {
                xm = 0;
                ym = y >= 1 ? -1 : 1;
            }
            else if (ym != 0 && Math.Abs(y) >= 1)
            {
                ym = 0;
                xm = x >= 1 ? -1 : 1;
            }

            Vector3 dir = new Vector3(x, 0, y);
            this._self.position = this._target.position + dir.normalized * 5f;

            return (int)this._nextStage;
        }

        public void OnExit()
        {

        }

        public DialogBlock OnInteract()
        {
            return null;
        }

        public void OnEndDialog(DialogBlock dialogBlock, int dialogOutcome)
        {
            if (dialogBlock != this._dialog) { return; }

            this._nextStage = EMeiStates.SecrectRoom;
        }
    }

    private class SecretRoom : IState
    {
        private EMeiStates _nextStage;

        private Transform _self;
        private Transform _secretRoom;
        private DialogBlock _dialog;

        public SecretRoom(Transform self, Transform secretRoom, DialogBlock dialog)
        {
            this._self = self;
            this._secretRoom = secretRoom;
            this._dialog = dialog;
        }

        public void OnEnter()
        {
            this._self.position = this._secretRoom.position;
            this._nextStage = EMeiStates.SecrectRoom;
        }

        public int OnUpdate()
        {
            return (int)this._nextStage;
        }

        public void OnExit()
        {

        }

        public DialogBlock OnInteract()
        {
            return this._dialog;
        }

        public void OnEndDialog(DialogBlock dialogBlock, int dialogOutcome)
        {

        }
    }
}