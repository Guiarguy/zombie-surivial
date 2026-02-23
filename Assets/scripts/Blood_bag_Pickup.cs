using UnityEngine;

public class BloodBagPickup : MonoBehaviour
{
    public int healthAmount = 20;   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 往上找 PlayerShooting（避免掛在子物件找不到）
            PlayerHealth playerhealth = other.GetComponent<PlayerHealth>();
            if (playerhealth == null)
                playerhealth = other.GetComponentInParent<PlayerHealth>();

            if (playerhealth != null)
            {
                playerhealth.AddBlood(healthAmount);
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 稍微轉轉看起來比較像補給
        transform.Rotate(0f, 80f * Time.deltaTime, 0f);
    }
}
