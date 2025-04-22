using UnityEngine;
using Unity.Cinemachine;

public class rockCode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float shakeModifer = 10f;
    [SerializeField] private float interval = 1f;

    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioSource boulderSound;

    
    CinemachineImpulseSource impulseSource;
    private float shakeTimer = 0f;

    void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        shakeTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (shakeTimer > interval) return;
        FireImpulse();
        CollisionFx(other);
        shakeTimer = 0f;
    }

    void FireImpulse()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float shakeIntensity = (1f/distance)*shakeModifer;
        shakeIntensity = Mathf.Min(shakeIntensity, 1f);
        impulseSource.GenerateImpulse(shakeIntensity);
    }

    void CollisionFx(Collision other)
    {
        ContactPoint contactP = other.contacts[0];
        particles.transform.position = contactP.point;
        particles.Play();
        boulderSound.Play();
    }
}
