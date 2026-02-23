using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    [Header("Hit Effect")]
    public GameObject bloodEffectPrefab; // ← 粒子系特效

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        currentHealth -= amount;

        // 播粒子特效
        PlayBloodEffect(hitPoint, hitNormal);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void PlayBloodEffect(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (bloodEffectPrefab != null)
        {
            Quaternion rot = Quaternion.LookRotation(hitNormal);
            GameObject effect = Instantiate(bloodEffectPrefab, hitPoint, rot);

            Destroy(effect, 1f); // 自動清除特效
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
