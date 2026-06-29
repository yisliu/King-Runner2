using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; 

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private RectTransform mainMenuPanel;
    [SerializeField] private RectTransform characterSelectPanel;

    [Header("Core Elements")]
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Image fadeOverlay;

    [Header("Animation Settings")]
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Ease transitionEase = Ease.InOutCubic;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("BestScore", 0);
        if (highScoreText != null)
        {
            highScoreText.text = "Best: " + highScore;
        }

        if (fadeOverlay != null)
        {
            fadeOverlay.color = Color.black;
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.DOFade(0f, fadeDuration)
                       .SetEase(Ease.Linear)
                       .OnComplete(() => fadeOverlay.gameObject.SetActive(false));
        }

        if (mainMenuPanel != null) mainMenuPanel.anchoredPosition = Vector2.zero;
        if (characterSelectPanel != null) characterSelectPanel.anchoredPosition = new Vector2(2000f, 0f);
    }

    public void PlayGame()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.DOFade(1f, fadeDuration)
                       .SetEase(Ease.Linear)
                       .OnComplete(() => SceneManager.LoadScene("ShipHub"));
        }
        else
        {
            SceneManager.LoadScene("ShipHub");
        }
    }

    public void OpenCharacterSelect()
    {
        mainMenuPanel.DOAnchorPos(new Vector2(-2000f, 0f), transitionDuration)
                     .SetEase(transitionEase);

        if (characterSelectPanel != null)
            characterSelectPanel.DOAnchorPos(Vector2.zero, transitionDuration)
                                .SetEase(transitionEase);
    }

    public void CloseCharacterSelect()
    {
        mainMenuPanel.DOAnchorPos(Vector2.zero, transitionDuration)
                     .SetEase(transitionEase);

        if (characterSelectPanel != null)
            characterSelectPanel.DOAnchorPos(new Vector2(2000f, 0f), transitionDuration)
                                .SetEase(transitionEase);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game requested.");
        Application.Quit();
    }
}