using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening; 
public class HubController : MonoBehaviour
{
    [Header("Flow Settings")]
    [SerializeField] private float idleDuration = 4.0f;

    [Header("Cinematic Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform shipTransform;
    [SerializeField] private Vector3 cameraStartPosition = new Vector3(-6f, 7.5f, -4.44f);
    [SerializeField] private Vector3 cameraEndPosition = new Vector3(-9.13f, 3.65f, -4.44f);
    [SerializeField] private Vector3 cameraStartRotation = new Vector3(50f, 60f, 0.024f);
    [SerializeField] private Vector3 cameraEndRotation = new Vector3(35.255f, 67.119f, 0.024f);

    private bool canInteract = false;

    public bool CanInteract => canInteract;

    void Start()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;

        cameraTransform.position = cameraStartPosition;
        cameraTransform.rotation = Quaternion.Euler(cameraStartRotation);

        StartCoroutine(HubFlowRoutine());
    }

    IEnumerator HubFlowRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < idleDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / idleDuration;
            
            t = Mathf.SmoothStep(0f, 1f, t); 

            cameraTransform.position = Vector3.Lerp(cameraStartPosition, cameraEndPosition, t);
            cameraTransform.rotation = Quaternion.Slerp(
                Quaternion.Euler(cameraStartRotation),
                Quaternion.Euler(cameraEndRotation),
                t
            );

            yield return null;
        }
        
        canInteract = true;
        Debug.Log("Hub Active: You can now hover and click on level nodes.");
    }

    public void SelectAndLoadLevel(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Target scene name is empty on this slot!");
            return;
        }
        
        canInteract = false; 

        Debug.Log($"Hyperjumping to: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}