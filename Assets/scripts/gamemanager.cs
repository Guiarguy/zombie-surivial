using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //void Awake()
    //{
    //    // 單例
    //    if (Instance == null) Instance = this;
    //    else Destroy(gameObject);

    //    DontDestroyOnLoad(gameObject);
    //}

    // 呼叫：開始遊戲
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // 你的遊戲場景名字
    }

    // 呼叫：玩家死亡 → 回主選單
    public static void GameOver()
    {
        Cursor.lockState = CursorLockMode.None; // 鎖在畫面中央
        Cursor.visible = true;

        SceneManager.LoadScene("MainScene");
    }
}
