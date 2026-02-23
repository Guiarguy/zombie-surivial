using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TPSMovement_UsingPlayerInput : MonoBehaviour
{
    private CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;

    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private Vector3 velocity;

    [Header("Dash Settings")]
    public float dashDuration = 0.5f;      // 衝刺持續時間
    public float dashSpeedMultiplier = 3f; // 衝刺速度倍率
    public float dashCooldown = 1f;        // 衝刺冷卻 1 秒

    private bool isDashing = false;
    private float dashTimer = 0f;

    private float dashCooldownTimer = 0f;  // 冷卻計時



    void Awake()
    {
        controller = GetComponent<CharacterController>();

        
    }

    // 🚀 PlayerInput 自動呼叫
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        // 新輸入系統會在按下和放開都呼叫一次
        if (!value.isPressed) return;

        StartDash();
    }

    void StartDash()
    {
        // 若正在衝刺，或冷卻中，則不觸發
        if (isDashing) return;
        if (dashCooldownTimer > 0f) return;

        isDashing = true;
        dashTimer = dashDuration;

        // 重新開始冷卻（先設為冷卻時間，衝刺結束後會持續倒數）
        dashCooldownTimer = dashCooldown;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        ApplyGravity();
        HandleDash();
        UpdateDashCooldown();
    }

    void HandleDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            // 在衝刺期間以高速向前移動
            Vector3 dashMove = transform.forward * moveSpeed * dashSpeedMultiplier * Time.deltaTime;
            controller.Move(dashMove);

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
    }

    void UpdateDashCooldown()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer < 0f)
                dashCooldownTimer = 0f;
        }
    }

    void HandleMovement()
    {
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        transform.Rotate(0, lookInput.x * rotationSpeed * Time.deltaTime, 0);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
