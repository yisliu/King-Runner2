using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;
using DG.Tweening;

public class EndScreenManager : MonoBehaviour
{
    [Header("UI Text Displays")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private RectTransform highScoreBanner;

    [Header("Animate Group Containers")]
    [SerializeField] private CanvasGroup mainPanelGroup;
    [SerializeField] private RectTransform buttonsContainer;

    [Header("Button Hover Animation Settings")]
    [SerializeField] private float hoverScaleMultiplier = 1.1f;
    [SerializeField] private float animationDuration = 0.2f; 
    [SerializeField] private Ease hoverEase = Ease.OutCubic;

    private void Start()
    {
        if (mainPanelGroup != null) mainPanelGroup.alpha = 0f;
        if (buttonsContainer != null) buttonsContainer.anchoredPosition = new Vector2(0f, -500f);
        if (highScoreBanner != null) highScoreBanner.localScale = Vector3.zero;

        if (coinsText != null) coinsText.text = "Coins: " + RunData.CoinsCollected;

        if (timeText != null)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(RunData.TimeSurvived);
            timeText.text = string.Format("Time: {0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        if (scoreText != null) scoreText.text = "0";

        if (buttonsContainer != null)
        {
            Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>(true);
            foreach (Button btn in buttons)
            {
                UIButtonJuice juice = btn.gameObject.AddComponent<UIButtonJuice>();
                juice.Setup(hoverScaleMultiplier, animationDuration, hoverEase);
            }
        }

        Sequence endSequence = DOTween.Sequence();

        if (mainPanelGroup != null)
            endSequence.Append(mainPanelGroup.DOFade(1f, 0.4f));

        if (scoreText != null)
            endSequence.Append(DOTween.To(() => 0, x => scoreText.text = x.ToString(), RunData.FinalScore, 1.2f).SetEase(Ease.OutQuad));

        if (RunData.IsNewHighScore && highScoreBanner != null)
            endSequence.Append(highScoreBanner.DOScale(1f, 0.5f).SetEase(Ease.OutBack));

        if (buttonsContainer != null)
            endSequence.Append(buttonsContainer.DOAnchorPosY(0f, 0.4f).SetEase(Ease.OutCubic));
    }

    public void ActionRetry() => SceneManager.LoadScene(RunData.LevelSceneName);
    public void ActionMainMenu() => SceneManager.LoadScene("StartScreen");
    public void ActionNextLevel() => SceneManager.LoadScene("ShipHub");
}


public class UIButtonJuice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private float targetScale;
    private float duration;
    private Ease easeType;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void Setup(float scaleMultiplier, float animDuration, Ease ease)
    {
        targetScale = scaleMultiplier;
        duration = animDuration;
        easeType = ease;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * targetScale, duration).SetEase(easeType).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, duration).SetEase(easeType).SetUpdate(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(originalScale * (targetScale * 0.9f), duration * 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(originalScale * targetScale, duration * 0.5f).SetEase(Ease.InQuad).SetUpdate(true);
    }

    private void OnDisable()
    {
        transform.DOKill();
        transform.localScale = originalScale;
    }
}