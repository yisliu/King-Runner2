using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class playerCamara : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float minFov = 35f;
    [SerializeField] ParticleSystem speedupParticles;
    [SerializeField] private float maxFov = 85f;
    
    [SerializeField] private float zoomDuration = 1f;
    [SerializeField] private float zoomSpeedModifier = 5f;

    
    CinemachineCamera cinemachineCamera;

    void Awake()
    {
        if (cinemachineCamera == null)
        {
            
        }
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public void changeCameraFOV(float speedAmount)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeFOVRoutine(speedAmount));

        if (speedAmount>0)
        {
            StartCoroutine(particleUp());
        }

    }

    public IEnumerator particleUp()
    {
        speedupParticles.Play();
        float timeLeft = 2f;

        while (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        } 
        speedupParticles.Stop();
    }

    IEnumerator ChangeFOVRoutine(float speedAmount)
    {
        float startFov = cinemachineCamera.Lens.FieldOfView;
        float targetFov = Mathf.Clamp(startFov + speedAmount * zoomSpeedModifier, minFov, maxFov);
        
        float elapsedTime = 0;

        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime/zoomDuration;
            elapsedTime += Time.deltaTime;

            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFov, targetFov, t);
            yield return null;
        }
        cinemachineCamera.Lens.FieldOfView = targetFov;
    }

}
