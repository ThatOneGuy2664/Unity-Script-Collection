using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    public float baseIntensity = 1.5f;
    public float flickerAmount = 0.5f;
    public float flickerSpeed = 0.1f;

    private Light light;
    private float timer;

    void Awake()
    {
        light = GetComponent<Light>();
        timer = flickerSpeed;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            float flicker = Random.Range(-flickerAmount, flickerAmount);
            light.intensity = baseIntensity + flicker;
            timer = flickerSpeed;
        }
    }
}
