using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            ZombieHealth zh = other.GetComponentInParent<ZombieHealth>();

            if (zh != null)
            {
                // 計算碰撞點
                Vector3 hitPoint = transform.position;
                Vector3 hitNormal = -transform.forward; // 粒子朝向子彈來的方向

                zh.TakeDamage(damage, hitPoint, hitNormal);
            }
        }

        Destroy(gameObject);
    }

}
