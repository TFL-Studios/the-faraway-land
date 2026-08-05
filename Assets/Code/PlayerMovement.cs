using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _height = 1.5f;

    [Header("Speed")]
    [SerializeField] private float _defaultSpeed = 2f;

    [Header("Sprint")]
    [SerializeField] private float _sprintSpeedMultiplier = 1.5f;
    [SerializeField] private float _sprintEnergyDrainPerSecond = 10f;
    [SerializeField] private float _maxEnergy = 100f;

    [Header("Crouch")]
    [SerializeField] private float _crouchSpeedMultiplier = 0.75f;
    [SerializeField] private float _crouchHeightMultiplier = 0.66f;

    [Header("Gravity")]
    [SerializeField] private float _gravity = -20f;

    private CharacterController _controller;

    // Movement state
    private Vector2 _moveInput;
    private Vector3 _verticalVelocity;
    private bool _isSprinting;
    private bool _isCrouching;
    private float _currentEnergy;

    private void Awake()
    {
        this._controller = this.GetComponent<CharacterController>();
        this._currentEnergy = this._maxEnergy;
    }

    private void Update()
    {
        this.ReadInput();
        this.HandleSprint();
        this.HandleMovement();
        this.HandleGravity();
    }

    private void ReadInput()
    {
        this._moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (InputHandler.Instance.EntradaCorrida.FoiPressionada) { this.TryStartSprint(); }
        if (InputHandler.Instance.EntradaCorrida.FoiSolta) { this._isSprinting = false; }
        if (InputHandler.Instance.EntradaAgachamento.FoiPressionada) { this.ToggleCrouch(); }
    }

    private void TryStartSprint()
    {
        if (this._isCrouching) { this.ExitCrouch(); }
        this._isSprinting = true;
    }

    private void HandleSprint()
    {
        if (!this._isSprinting) { return; }
        this._currentEnergy -= this._sprintEnergyDrainPerSecond * Time.deltaTime;
        if (this._currentEnergy > 0f) { return; }
        this._currentEnergy = 0f;
        this._isSprinting = false;
    }

    private void HandleMovement()
    {
        float speed = this.GetCurrentSpeed();

        // Normalize diagonal movement so it doesn't exceed max speed
        Vector2 normalizedInput = this._moveInput.magnitude > 1f ? this._moveInput.normalized : this._moveInput;
        Vector3 moveDirection = this.transform.forward * normalizedInput.y + this.transform.right * normalizedInput.x;
        Vector3 totalMovement = (moveDirection * speed + this._verticalVelocity) * Time.deltaTime;
        this._controller.Move(totalMovement);
    }

    private void HandleGravity()
    {
        if (this._controller.isGrounded && this._verticalVelocity.y < 0f)
        {
            this._verticalVelocity.y = -2f;
            return;
        }

        this._verticalVelocity.y += this._gravity * Time.deltaTime;
    }

    private float GetCurrentSpeed()
    {
        if (!this._isCrouching && !this._isSprinting) { return this._defaultSpeed; }
        return this._isCrouching ? this._defaultSpeed * this._crouchSpeedMultiplier : this._defaultSpeed * this._sprintSpeedMultiplier;
    }

    private void ToggleCrouch()
    {
        if (this._isCrouching)
        {
            this.TryExitCrouch();
            return;
        }

        this.EnterCrouch();
    }

    private void EnterCrouch()
    {
        this._isCrouching = true;
        this._isSprinting = false;
        this.ApplyCrouchHeight(this._height * this._crouchHeightMultiplier);
    }

    private void ExitCrouch()
    {
        this._isCrouching = false;
        this.ApplyCrouchHeight(this._height);
    }

    private void TryExitCrouch()
    {
        // Cast a ray upward to check if something is blocking standing up
        Vector3 rayOrigin = this.transform.position + Vector3.up * (this._height * this._crouchHeightMultiplier);
        float rayDistance = this._height - (this._height * this._crouchHeightMultiplier);
        bool blocked = Physics.Raycast(rayOrigin, Vector3.up, rayDistance);
        if (blocked) { return; }
        this.ExitCrouch();
    }

    private void ApplyCrouchHeight(float height)
    {
        this._controller.height = height;
    }

    // Public getters for UI / other systems
    public float GetEnergy() => this._currentEnergy;
    public float GetMaxEnergy() => this._maxEnergy;
    public bool IsSprinting() => this._isSprinting;
    public bool IsCrouching() => this._isCrouching;
}