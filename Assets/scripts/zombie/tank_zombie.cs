using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class tank_zonbie: MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    public float gravity = -9.81f;
    private float verticalVelocity = 0f;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    public int damage = 10;         // 殭屍碰到玩家造成的傷害
    public float attackTimer = 0f;


    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // 自動找玩家（可選）
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        // =============================
        // 1. 計算方向（水平面，不要抬頭）
        // =============================

        attackTimer -= Time.deltaTime;
        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.1f) // 防止除以 0
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // =============================
        // 2. 水平移動（靠 CharacterController）
        // =============================
        Vector3 move = transform.forward * moveSpeed;

        // =============================
        // 3. 重力
        // =============================
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // 貼地小負值
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        // =============================
        // 4. 移動
        // =============================
        controller.Move(move * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("tank");
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
