using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 15;   // 撿一次加幾發

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 往上找 PlayerShooting（避免掛在子物件找不到）
            PlayerShooting shooting = other.GetComponent<PlayerShooting>();
            if (shooting == null)
                shooting = other.GetComponentInParent<PlayerShooting>();

            if (shooting != null)
            {
                shooting.AddAmmo(ammoAmount);
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
