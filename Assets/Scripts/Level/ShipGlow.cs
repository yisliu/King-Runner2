using UnityEngine;

public class ShipGlow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Light thrustLight;
    private Vector3 ogScale;
    [SerializeField] private float minIntensity = 2.0f;
    [SerializeField] private float maxIntensity = 3.5f;
    [SerializeField] private float flickerSpeed = 15.0f;
    
    
    void Start()
    {
        thrustLight = GetComponent<Light>();
        ogScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float sound = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        if (thrustLight != null)
        {
            thrustLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, sound);
        }
        float pulseScale = Mathf.Lerp(0.95f, 1.05f, sound);
        transform.localScale = ogScale * pulseScale;
    }
}
