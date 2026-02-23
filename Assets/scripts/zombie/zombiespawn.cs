using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie Prefabs")]
    public GameObject normalZombiePrefab;
    public GameObject runnerZombiePrefab;
    public GameObject tankZombiePrefab;
    public GameObject spitterZombiePrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;   // 會被 NightManager 改
    public bool canSpawn = true;      // 夜晚 = true，白天 = false

    [Header("Map Size (10x10)")]
    public float width = 10f;
    public float height = 10f;

    private float timer = 0f;

    void Update()
    {
        if (!canSpawn) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnZombieAtEdge();
            timer = 0f;
        }
    }

    void SpawnZombieAtEdge()
    {
        int type = Random.Range(0, 4);
        GameObject prefab = null;

        switch (type)
        {
            case 0: prefab = normalZombiePrefab; break;
            case 1: prefab = runnerZombiePrefab; break;
            case 2: prefab = tankZombiePrefab; break;
            case 3: prefab = spitterZombiePrefab; break;
        }

        float halfW = width / 2f;
        float halfH = height / 2f;

        Vector3 spawnPos = Vector3.zero;
        int side = Random.Range(1, 5);

        switch (side)
        {
            case 1: // 左
                spawnPos = new Vector3(-halfW, 0f, Random.Range(-halfH, halfH));
                break;
            case 2: // 右
                spawnPos = new Vector3(halfW, 0f, Random.Range(-halfH, halfH));
                break;
            case 3: // 上
                spawnPos = new Vector3(Random.Range(-halfW, halfW), 0f, halfH);
                break;
            case 4: // 下
                spawnPos = new Vector3(Random.Range(-halfW, halfW), 0f, -halfH);
                break;
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
