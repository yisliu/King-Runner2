using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShootingStarEffect : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 800f; 
    
    [Header("Screen Bounds (1920x1080 Reference)")]
    private float screenWidth = 1920f;
    private float screenHeight = 1080f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        TriggerShootingStar();
    }

    private void TriggerShootingStar()
    {
        float startX = Random.Range(screenWidth * 0.2f, screenWidth * 1.2f);
        float startY = (screenHeight / 2f) + 100f; 

        rectTransform.anchoredPosition = new Vector2(startX, startY);

        float distanceX = screenWidth * 1.2f; 
        float targetX = startX - distanceX;
        float targetY = startY - (distanceX * 0.5f); 
        Vector2 targetPosition = new Vector2(targetX, targetY);

        float duration = distanceX / speed;

        rectTransform.localRotation = Quaternion.Euler(0, 0, 25f);

        rectTransform.DOKill();
        canvasGroup.DOKill();

        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(0.8f, 0.2f); 

        rectTransform.DOAnchorPos(targetPosition, duration)
            .SetEase(Ease.Linear)
            .SetUpdate(true);

        canvasGroup.DOFade(0f, 0.4f)
            .SetDelay(duration - 0.4f)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                Destroy(gameObject);
            });
    }

    private void OnDestroy()
    {
        rectTransform.DOKill();
        canvasGroup.DOKill();
    }
}