using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SpitterAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float keepDistance = 10f;      // 想和玩家保持的距離
    public float minDistance = 6f;        // 太近會後退
    public float rotationSpeed = 6f;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float bulletattackCooldown = 2f;
    public float spreadAngle = 8f;        // 發射角度隨機偏移
    private float cooldownTimer = 0f;

    [Header("Gravity")]
    public float gravity = -9.81f;
    float verticalVelocity;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    public int damage = 10;         // 殭屍碰到玩家造成的傷害
    private float attackTimer = 0f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null) return;

        FaceTarget();
        MovementLogic();

        attackTimer -= Time.deltaTime;
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = bulletattackCooldown;
        }
    }

    // ============================================================
    // 1. 面向玩家
    // ============================================================
    void FaceTarget()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }
    }

    // ============================================================
    // 2. 保持距離 AI（太遠接近，太近後退）
    // ============================================================
    void MovementLogic()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        Vector3 moveDir = Vector3.zero;

        if (dist > keepDistance)
        {
            // 太遠 → 靠近玩家
            moveDir = (target.position - transform.position).normalized;
        }
        else if (dist < minDistance)
        {
            // 太近 → 往後退
            moveDir = -(target.position - transform.position).normalized;
        }
        else
        {
            // 在合適距離 → 停下來攻擊
            moveDir = Vector3.zero;
        }

        moveDir.y = 0;

        // 重力
        if (controller.isGrounded)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        moveDir.y = verticalVelocity;

        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    // ============================================================
    // 3. 拋物線射擊（含散射角度）
    // ============================================================
    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = p.GetComponent<Rigidbody>();
        rb.useGravity = true;

        Vector3 start = firePoint.position;
        Vector3 end = target.position;

        // 加水平散射角度
        float randomAngle = Random.Range(-spreadAngle, spreadAngle);
        Vector3 dir = (end - start);
        dir = Quaternion.Euler(0, randomAngle, 0) * dir;

        // 使用偏移方向
        Vector3 newEnd = start + dir;
        float arcHeight = 2.5f;

        rb.linearVelocity = CalculateLaunchVelocity(start, newEnd, arcHeight);
    }

    // ============================================================
    // 4. 拋物線初速度計算
    // ============================================================
    Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float height)
    {
        float g = Mathf.Abs(Physics.gravity.y);

        Vector3 horizontal = new Vector3(end.x - start.x, 0, end.z - start.z);
        float horizDist = horizontal.magnitude;

        float verticalDist = end.y - start.y;

        float timeUp = Mathf.Sqrt(2 * height / g);
        float timeDown = Mathf.Sqrt(2 * (height - verticalDist) / g);
        float totalTime = timeUp + timeDown;

        Vector3 horizVel = horizontal / totalTime;
        float vertVel = Mathf.Sqrt(2 * g * height);

        Vector3 finalVel = horizVel;
        finalVel.y = vertVel;

        return finalVel;
    }

    private void OnTriggerStay(Collider other)
    {
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
