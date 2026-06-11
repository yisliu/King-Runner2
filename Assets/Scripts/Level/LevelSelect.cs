using UnityEngine;
using DG.Tweening; // Import the DOTween namespace

public class HubLevelSlot : MonoBehaviour
{
    [Header("Level Configuration")]
    [SerializeField] private string targetSceneName;

    [Header("Juice & Animation Settings")]
    [SerializeField] private float hoverScaleMultiplier = 1.25f;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private float punchStrength = 0.3f;

    private Vector3 originalScale;
    private HubController hubController;

    void Start()
    {
        originalScale = transform.localScale;
        
        hubController = Object.FindFirstObjectByType<HubController>();
    }

    void OnMouseEnter()
    {
        if (hubController != null && !hubController.CanInteract) return;

        // Scale up smoothly on hover
        transform.DOScale(originalScale * hoverScaleMultiplier, animationDuration)
                 .SetEase(Ease.OutBack);
    }

    void OnMouseExit()
    {
        transform.DOScale(originalScale, animationDuration)
                 .SetEase(Ease.InOutQuad);
    }

    void OnMouseDown()
    {
        if (hubController != null && !hubController.CanInteract) return;

        transform.DOKill();

        transform.DOPunchScale(Vector3.one * punchStrength, animationDuration, 10, 1)
            .OnComplete(() =>
            {
                if (hubController != null)
                {
                    hubController.SelectAndLoadLevel(targetSceneName);
                }
            });
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}