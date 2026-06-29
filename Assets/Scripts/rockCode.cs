/*
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
*/
using UnityEngine;
using Unity.Cinemachine;

public class rockCode : MonoBehaviour
{
    [SerializeField] private float shakeModifer = 10f;
    [SerializeField] private float interval = 1f;

    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioSource boulderSound;
    
    CinemachineImpulseSource impulseSource;
    private float shakeTimer = 0f;
    private Transform mainCameraTransform;

    void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Start()
    {
        // Cache the camera transform on start so we aren't calling Camera.main every collision frame
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        shakeTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        // FIX: Only apply screen shake if ENOUGH time has passed since the last shake
        if (shakeTimer < interval) return; 
        
        FireImpulse();
        CollisionFx(other);
        shakeTimer = 0f;
    }

    void FireImpulse()
    {
        if (mainCameraTransform == null) return;

        float distance = Vector3.Distance(transform.position, mainCameraTransform.position);
        
        // Prevent division by zero errors if the rock is right on top of the camera
        if (distance < 0.1f) distance = 0.1f; 

        float shakeIntensity = (1f / distance) * shakeModifer;
        shakeIntensity = Mathf.Clamp(shakeIntensity, 0f, 1f);
        
        impulseSource.GenerateImpulse(shakeIntensity);
    }

    void CollisionFx(Collision other)
    {
        if (other.contacts.Length == 0) return;

        ContactPoint contactP = other.contacts[0];
        
        if (particles != null)
        {
            particles.transform.position = contactP.point;
            particles.Play();
        }

        if (boulderSound != null)
        {
            boulderSound.Play();
        }
    }
}