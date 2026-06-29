using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using System.Collections;

public class WinScreenManager : MonoBehaviour
{
    [Header("Trophy & Title")]
    [SerializeField] private RectTransform trophyIcon;       // Image or large TMP text "🏆"
    [SerializeField] private TextMeshProUGUI victoryText;    // "VICTORY!"
    [SerializeField] private RectTransform victoryContainer; // holds title + trophy

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;       // "50 / 50 COINS"
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private RectTransform statsPanelGroup;

    [Header("Banners")]
    [SerializeField] private RectTransform newRecordBanner;  // "NEW RECORD!" overlay

    [Header("Buttons")]
    [SerializeField] private RectTransform buttonsContainer;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeOverlay;        // full-screen black Image CanvasGroup

    [Header("Timing")]
    [SerializeField] private float trophyDelay = 0.3f;
    [SerializeField] private float trophyPopDuration = 0.6f;
    [SerializeField] private float statsFadeDuration = 0.5f;
    [SerializeField] private float buttonsSlideDuration = 0.4f;

    [Header("Button Juice")]
    [SerializeField] private float hoverScaleMultiplier = 1.08f;
    [SerializeField] private float hoverDuration = 0.18f;
    [SerializeField] private Ease hoverEase = Ease.OutCubic;

    private void Start()
    {
        SetupInitialState();
        PopulateStats();
        AddButtonJuice();

        // Fade in from black if overlay is present
        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 1f;
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.DOFade(0f, 0.6f).SetEase(Ease.Linear)
                       .OnComplete(() => fadeOverlay.gameObject.SetActive(false));
        }

        StartCoroutine(PlayEntrance());
    }

    private void SetupInitialState()
    {
        if (trophyIcon != null) trophyIcon.localScale = Vector3.zero;
        if (victoryText != null)
        {
            var cg = victoryText.GetComponent<CanvasGroup>();
            if (cg == null) cg = victoryText.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
        }
        if (statsPanelGroup != null)
        {
            var cg = statsPanelGroup.GetComponent<CanvasGroup>();
            if (cg == null) cg = statsPanelGroup.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
        }
        if (newRecordBanner != null) newRecordBanner.localScale = Vector3.one;
        if (buttonsContainer != null) buttonsContainer.anchoredPosition = new Vector2(0f, -600f);
    }

    private void PopulateStats()
    {
        if (scoreText != null) scoreText.text = "0";

        int coins = RunData.CoinsCollected;
        int goal = RunData.CoinGoal > 0 ? RunData.CoinGoal : 50;
        if (coinsText != null) coinsText.text = $"{coins} / {goal}";

        if (timeText != null)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(RunData.TimeSurvived);
            timeText.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }
    }

    private void AddButtonJuice()
    {
        if (buttonsContainer == null) return;
        foreach (Button btn in buttonsContainer.GetComponentsInChildren<Button>(true))
        {
            if (btn.GetComponent<UIButtonJuice>() == null)
            {
                UIButtonJuice juice = btn.gameObject.AddComponent<UIButtonJuice>();
                juice.Setup(hoverScaleMultiplier, hoverDuration, hoverEase);
            }
        }
    }

    private IEnumerator PlayEntrance()
    {
        yield return new WaitForSeconds(trophyDelay);

        // 1. Victory text fades in
        if (victoryText != null)
        {
            var cg = victoryText.GetComponent<CanvasGroup>();
            if (cg != null) cg.DOFade(1f, 0.35f).SetEase(Ease.OutQuad);
        }

        yield return new WaitForSeconds(0.25f);

        // 2. Trophy bounces in
        if (trophyIcon != null)
        {
            yield return trophyIcon.DOScale(1f, trophyPopDuration)
                                   .SetEase(Ease.OutBack)
                                   .WaitForCompletion();
        }

        // 3. Stats panel fades in + score counts up
        if (statsPanelGroup != null)
        {
            var cg = statsPanelGroup.GetComponent<CanvasGroup>();
            if (cg != null) cg.DOFade(1f, statsFadeDuration).SetEase(Ease.OutQuad);
        }

        if (scoreText != null)
        {
            yield return DOTween.To(() => 0, x => scoreText.text = x.ToString(), RunData.FinalScore, 1.2f)
                                .SetEase(Ease.OutQuad)
                                .WaitForCompletion();
        }
        else
        {
            yield return new WaitForSeconds(statsFadeDuration);
        }

        // 4. New record banner — always visible, no animation needed

        // 5. Buttons slide up
        if (buttonsContainer != null)
        {
            yield return buttonsContainer.DOAnchorPosY(0f, buttonsSlideDuration)
                                         .SetEase(Ease.OutCubic)
                                         .WaitForCompletion();
        }
    }

    public void ActionRetry()
    {
        RunData.IsWinState = false;
        string scene = !string.IsNullOrEmpty(RunData.LevelSceneName) ? RunData.LevelSceneName : "ShipHub";
        SceneManager.LoadScene(scene);
    }

    public void ActionMainMenu()
    {
        RunData.IsWinState = false;
        SceneManager.LoadScene("StartScreen");
    }

    public void ActionNextLevel()
    {
        RunData.IsWinState = false;
        SceneManager.LoadScene("ShipHub");
    }
}