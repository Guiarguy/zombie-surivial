using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class rush_zombie : MonoBehaviour
{
    public Transform target;

    [Header("Speed Settings")]
    public float fastSpeed = 10f;  // 衝刺速度
    public float walkSpeed = 2f;  // 慢走速度

    public float rotationSpeed = 5f;

    public float gravity = -9.81f;
    private float verticalVelocity = 0f;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    public int damage = 10;         // 殭屍碰到玩家造成的傷害
    private float attackTimer = 0f;

    private CharacterController controller;

    // ===== 衝刺/走路計時器 =====
    private float stateTimer = 0f;
    private bool isSprinting = false;  // true=衝刺，false=慢走

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null) return;

        // =====================================================
        // 1. 狀態切換：衝刺 1 秒 → 慢走 2 秒 → 循環
        // =====================================================
        stateTimer += Time.deltaTime;
        attackTimer -= Time.deltaTime;

        if (isSprinting && stateTimer >= 1f)
        {
            // 1 秒後，切換成慢走
            isSprinting = false;
            stateTimer = 0f;
        }
        else if (!isSprinting && stateTimer >= 3f)
        {
            // 3 秒後，切換成衝刺
            isSprinting = true;
            stateTimer = 0f;
        }

        float currentSpeed = isSprinting ? fastSpeed : walkSpeed;

        // =====================================================
        // 2. 轉向面對玩家（水平）
        // =====================================================
        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // =====================================================
        // 3. 水平移動
        // =====================================================
        Vector3 move = transform.forward * currentSpeed;

        // =====================================================
        // 4. 重力
        // =====================================================
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("rush hit :" + other.tag);
        if (other.CompareTag("Player") && attackTimer <= 0f)
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
            attackTimer = attackCooldown; // 冷卻避免一瞬間判定多次
        }
    }
}
