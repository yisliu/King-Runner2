using TMPro;
using UnityEngine;
using DG.Tweening;

public class gameManager : MonoBehaviour
{
    [SerializeField] playerCollison player;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private float startTime = 5f;
    [SerializeField] private float lowTimeThreshold = 10f;

    private float timeLeft;
    private bool gameover = false;
    private bool lowTimePulseActive = false;
    private Tweener pulseTween;

    public bool GameOverValues => gameover;

    void Start()
    {
        timeLeft = startTime;
        if (timeText != null) timeText.color = Color.white;
    }

    void Update()
    {
        timer();
    }

    public void IncreaseTime(float timeAdded)
    {
        timeLeft += timeAdded;

        // Cancel pulse if time was refilled above threshold
        if (timeLeft > lowTimeThreshold && lowTimePulseActive)
        {
            lowTimePulseActive = false;
            pulseTween?.Kill();
            if (timeText != null) timeText.color = Color.white;
        }
    }

    public void timer()
    {
        if (gameover) return;

        timeLeft -= Time.deltaTime;
        timeText.text = timeLeft.ToString("F1");

        if (timeLeft <= lowTimeThreshold && !lowTimePulseActive)
        {
            lowTimePulseActive = true;
            pulseTween = DOTween.To(() => timeText.color, x => timeText.color = x, Color.red, 0.4f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }

        if (timeLeft <= 0f)
            GameOver();
    }

    void GameOver()
    {
        gameover = true;
        pulseTween?.Kill();
        if (timeText != null) timeText.color = Color.red;
        if (player != null) player.enabled = false;
        Time.timeScale = 0f;
    }
}
