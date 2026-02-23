using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Input Action")]
    public InputAction shootAction;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootForce = 800f;

    [Header("Ammo")]
    public int currentAmmo = 30;   // 一開始有幾發子彈，想設多少自己改
    public bool requireAmmo = true; // 如果你想暫時無限子彈可以設 false

    [Header("UI")]
    public TextMeshProUGUI bulletAmountText;   // ← UI Text 放這裡

    private void OnEnable()
    {
        shootAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    void Update()
    {
        UpdateBulletAmountUI();
        if (shootAction.WasPressedThisFrame())
        {
            Shoot();
        }
    }
    void UpdateBulletAmountUI()
    {
        if (bulletAmountText != null)
        {
            bulletAmountText.text = "Bullet: " + currentAmmo.ToString();
        }
    }

    void Shoot()
    {
        if (requireAmmo && currentAmmo <= 0)
        {
            Debug.Log("No ammo!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * shootForce);
        }

        if (requireAmmo)
        {
            currentAmmo--;    // 只有往下扣，沒有 max，等於沒有上限
            Debug.Log("Ammo: " + currentAmmo);
        }
    }

    // 給彈匣撿起來用
    public void AddAmmo(int amount)
    {
        currentAmmo += amount;  // 不做 Clamp，等於沒有上限
        Debug.Log("Pickup ammo, now: " + currentAmmo);
    }
}
