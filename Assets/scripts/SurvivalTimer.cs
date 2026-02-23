using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    public float timeLeft = 60f;   // 生存 60 秒
    public TextMeshProUGUI timerText;

    private bool isFinished = false;

    void Update()
    {
        if (isFinished) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            timeLeft = 0;
            isFinished = true;
            OnSurvivalSuccess();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        int seconds = Mathf.CeilToInt(timeLeft);
        timerText.text = "Time: " + seconds.ToString();
    }

    void OnSurvivalSuccess()
    {
        Debug.Log("Survival Success!");
        //GameManager.GameOver();   // ★ 回主畫面
    }
}
