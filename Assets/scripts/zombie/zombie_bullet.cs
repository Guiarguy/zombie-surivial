using UnityEngine;

public class SpitProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damage = 10;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }

        Destroy(gameObject); // 打到任何東西後消失
    }
}
