using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening; 
public class UIJuice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Scale Settings")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float scaleDuration = 0.2f;

    [Header("Hover Bobbing Settings")]
    [SerializeField] private float bobDistance = 10f; // How many UI pixels it moves up and down
    [SerializeField] private float bobCycleTime = 0.6f; // Time for one full up-and-down movement

    private Vector3 originalScale;
    private Vector2 originalAnchoredPosition;
    private RectTransform rectTransform;
    private Tween bobTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;
        
        if (rectTransform != null)
        {
            originalAnchoredPosition = rectTransform.anchoredPosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * hoverScale, scaleDuration)
                 .SetEase(Ease.OutBack)
                 .SetUpdate(true);

        if (rectTransform != null)
        {
            bobTween?.Kill(); 

            bobTween = rectTransform.DOAnchorPosY(originalAnchoredPosition.y + bobDistance, bobCycleTime / 2)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo) // -1 means loop infinitely, Yoyo means bounce back and forth
                .SetUpdate(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bobTween?.Kill();

        transform.DOScale(originalScale, scaleDuration)
                 .SetEase(Ease.OutCubic)
                 .SetUpdate(true);

        if (rectTransform != null)
        {
            rectTransform.DOAnchorPosY(originalAnchoredPosition.y, scaleDuration)
                         .SetEase(Ease.OutCubic)
                         .SetUpdate(true);
        }
    }

    private void OnDisable()
    {
        bobTween?.Kill();
        transform.localScale = originalScale;
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = originalAnchoredPosition;
        }
    }
}