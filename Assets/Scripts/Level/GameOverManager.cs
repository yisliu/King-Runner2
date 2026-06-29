using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private TextMeshProUGUI causeOfDeathText;

    private void Awake()
    {
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = false;
        gameOverCanvasGroup.blocksRaycasts = false;
        gameOverCanvasGroup.gameObject.SetActive(false);
    }

    public void TriggerPlayerDeath(string cause)
    {
        gameOverCanvasGroup.gameObject.SetActive(true);
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;

        causeOfDeathText.text = cause;
        causeOfDeathText.ForceMeshUpdate();

        if (CameraEffect.Instance != null) CameraEffect.Instance.TriggerShake(0.5f, 0.7f);

        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.1f, 0.6f)
            .SetUpdate(true)
            .OnComplete(() => Time.timeScale = 0f);

        gameOverCanvasGroup.DOKill();
        gameOverCanvasGroup.DOFade(1f, 0.5f).SetUpdate(true);
    }

}
