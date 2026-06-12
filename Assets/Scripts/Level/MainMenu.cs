using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; 

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private RectTransform mainMenuPanel;
    [SerializeField] private RectTransform settingsPanel;

    [Header("Core Elements")]
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Image fadeOverlay;

    [Header("Animation Settings")]
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Ease transitionEase = Ease.InOutCubic;
    [SerializeField] private float fadeDuration = 1.0f;

    [Header("Settings Panel Elements")]
    [SerializeField] private TMP_Text audioButtonText; // Drag your AudioButton's Text component here

    // We only declare this variable ONCE here so the entire script can share it safely
    private bool isMuted = false;

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
        if (settingsPanel != null) settingsPanel.anchoredPosition = new Vector2(2000f, 0f);
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

    public void OpenSettings()
    {
        mainMenuPanel.DOAnchorPos(new Vector2(-2000f, 0f), transitionDuration)
                     .SetEase(transitionEase);

        settingsPanel.DOAnchorPos(Vector2.zero, transitionDuration)
                     .SetEase(transitionEase);
    }

    public void CloseSettings()
    {
        mainMenuPanel.DOAnchorPos(Vector2.zero, transitionDuration)
                     .SetEase(transitionEase);

        settingsPanel.DOAnchorPos(new Vector2(2000f, 0f), transitionDuration)
                     .SetEase(transitionEase);
    }

    public void ToggleAudio()
    {
        isMuted = !isMuted;
    
        // Mute project audio listener
        AudioListener.pause = isMuted;

        // Update the button text to show the current state to the player
        if (audioButtonText != null)
        {
            audioButtonText.text = isMuted ? "MUTED" : "ACTIVE";
        }

        Debug.Log(isMuted ? "Audio Muted" : "Audio Unmuted");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game requested.");
        Application.Quit();
    }
}