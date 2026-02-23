using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public Light sceneLight;
    public float nightIntensity = 0.2f;   // 夜晚亮度（暗）
    public float breakIntensity = 1.2f;   // 休息亮度（亮）
    public float changeDuration = 1.5f;   // 燈光漸變時間

    Coroutine routine;

    void Awake()
    {
        if (sceneLight == null)
            sceneLight = GetComponent<Light>();
    }

    public void SetNight()
    {
        StartChange(nightIntensity);
    }

    public void SetBreak()
    {
        StartChange(breakIntensity);
    }

    void StartChange(float target)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ChangeIntensity(target));
    }

    IEnumerator ChangeIntensity(float target)
    {
        float start = sceneLight.intensity;
        float t = 0f;

        while (t < changeDuration)
        {
            t += Time.deltaTime;
            float p = t / changeDuration;
            sceneLight.intensity = Mathf.Lerp(start, target, p);
            yield return null;
        }

        sceneLight.intensity = target;
    }
}
