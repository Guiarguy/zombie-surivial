using UnityEngine;
using TMPro;

public class NightManager : MonoBehaviour
{
    public ZombieSpawner spawner;            // 拖場上的 ZombieSpawner 進來
    public TextMeshProUGUI timerText;        // UI 計時文字

    [Header("Time Settings")]
    public float nightDuration = 60f;        // 每個夜晚 60 秒
    public float breakDuration = 10f;        // 夜晚之間休息 10 秒
    public int totalNights = 3;              // 三個晚上

    [Header("Spawn Interval Per Night")]
    public float night1SpawnInterval = 2.0f; // 第一晚生成間隔
    public float night2SpawnInterval = 1.0f; // 第二晚生成更快
    public float night3SpawnInterval = 0.5f; // 第三晚再更快

    [Header("Light")]
    public LightController lightController;
    public Light playerFlashlight;


    private int currentNight = 0;
    public float timer = 0f;

    private enum State { Night, Break, Finished }
    private State state = State.Night;

    void Start()
    {
        StartNight(1); // 一開始直接進入第一晚
    }

    void Update()
    {
        if (state == State.Finished) return;

        timer -= Time.deltaTime;
        if (timer < 0) timer = 0;

        Cursor.lockState = CursorLockMode.Locked; // 鎖在畫面中央
        Cursor.visible = false;

        UpdateUI();

        switch (state)
        {
            case State.Night:
                if (timer <= 0f)
                {
                    EndNight();
                }
                break;

            case State.Break:
                if (timer <= 0f)
                {
                    if (currentNight < totalNights)
                    {
                        StartNight(currentNight + 1);
                    }
                    else
                    {
                        FinishAllNights();
                    }
                }
                break;
        }
    }

    void StartNight(int nightIndex)
    {
        currentNight = nightIndex;
        state = State.Night;
        timer = nightDuration;

        lightController.SetNight();
        // 開啟生怪
        spawner.canSpawn = true;

        // 根據晚上設定生成速度
        switch (currentNight)
        {
            case 1:
                spawner.spawnInterval = night1SpawnInterval;
                playerFlashlight.intensity = 50f;
                playerFlashlight.range = 60f;
                break;

            case 2:
                spawner.spawnInterval = night2SpawnInterval;
                playerFlashlight.intensity = 25f;
                playerFlashlight.range = 23f;
                break;

            case 3:
                spawner.spawnInterval = night3SpawnInterval;
                playerFlashlight.intensity = 15f;
                playerFlashlight.range = 16f;
                break;
        }


        Debug.Log("Start Night " + currentNight);
    }

    void EndNight()
    {
        // 停止生怪＋清除所有殭屍
        spawner.canSpawn = false;
        ClearAllZombies();

        // 第一、第二晚：進入休息 10 秒
        if (currentNight < totalNights)
        {
            state = State.Break;
            timer = breakDuration;

            lightController.SetBreak();

            Debug.Log("Night " + currentNight + " finished. Break time.");
        }
        else
        {
            // 第三晚結束 → 直接結束遊戲
            FinishAllNights();
        }
    }

    void FinishAllNights()
    {
        state = State.Finished;
        spawner.canSpawn = false;
        ClearAllZombies();

        Debug.Log("All nights finished! Game Clear.");
        GameManager.GameOver(); // 或你改成 Victory()
    }

    void ClearAllZombies()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject z in zombies)
        {
            Destroy(z);
        }
    }

    void UpdateUI()
    {
        int seconds = Mathf.CeilToInt(timer);

        if (state == State.Night)
        {
            timerText.text = $"Night {currentNight} - {seconds}s";
        }
        else if (state == State.Break)
        {
            // 休息時間顯示下一晚倒數
            int nextNight = Mathf.Min(currentNight + 1, totalNights);
            timerText.text = $"Break {seconds}s - Next: Night {nextNight}";
        }
        else
        {
            timerText.text = "Finished";
        }
    }
}
