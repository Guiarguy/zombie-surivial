using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public TextMeshProUGUI hpText;
    public GameObject deathText;   // ← 顯示死亡訊息的 UI

    void Start()
    {
        currentHealth = maxHealth;

        if (deathText != null)
            deathText.gameObject.SetActive(false); // 開局關閉死亡文字
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        UpdateHPUI();
    }

    void UpdateHPUI()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + currentHealth.ToString();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");
        StartCoroutine(DeathSequence()); // 呼叫死亡流程
    }
    public void AddBlood(int amount)
    {
        currentHealth += amount;  // 不做 Clamp，等於沒有上限
        Debug.Log("Pickup ammo, now: " + currentHealth);
    }

    IEnumerator DeathSequence()
    {
        // 顯示死亡訊息
        if (deathText != null)
        {
            deathText.gameObject.SetActive(true);
        }

        // 停 3 秒
        yield return new WaitForSeconds(3f);

        // 呼叫遊戲結束
        GameManager.GameOver();
    }
}
