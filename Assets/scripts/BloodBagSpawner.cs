using UnityEngine;

public class BloodBagSpawner : MonoBehaviour
{
    public GameObject BloodBagPrefab;

    [Header("Map Size (10x10)")]
    public float width = 60f;
    public float height = 60f;

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;   // 碭Ω
    public int maxAmmoOnField = 5;     // 初程碭紆

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Debug.Log("bullet spawn");
            TrySpawnBloodBag();
            timer = 0f;
        }
    }

    void TrySpawnBloodBag()
    {
        // 初紆计秖
        //int count = GameObject.FindGameObjectsWithTag("Ammo").Length;
        //if (count >= maxAmmoOnField) return;

        float halfW = width / 2f;
        float halfH = height / 2f;

        float x = Random.Range(-halfW, halfW);
        float z = Random.Range(-halfH, halfH);
        Vector3 pos = new Vector3(x, 0.5f, z);  // y=0.5 菠疊癬ㄓ

        GameObject bloodBag = Instantiate(BloodBagPrefab, pos, Quaternion.identity);
        //ammo.tag = "Ammo";   // 癘眔倒ウ Tagノㄓ计秖北
    }
}
