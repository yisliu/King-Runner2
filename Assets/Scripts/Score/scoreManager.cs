using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class scoreManager : MonoBehaviour
{
    [SerializeField] gameManager GameManager;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text coinText;
    [SerializeField] Slider coinProgressBar;
    [SerializeField] int coinGoal = 50;

    private int coinCount = 0;
    private int score = 0;
    private float displayScore = 0f;
    private bool hasThresholdFired = false;

    public int Score => score;
    public int CoinCount => coinCount;
    public int CoinGoal => coinGoal;

    public static event Action onThresholdReached;

	private void Start()
	{
    	if (coinText != null)
        	coinText.text = coinCount.ToString();

    	if (scoreText != null)
        	scoreText.text = score.ToString();

        if (coinProgressBar != null)
        {
            coinProgressBar.minValue = 0;
            coinProgressBar.maxValue = coinGoal;
            coinProgressBar.value = 0;
        }
	}

    public void increaseScore(int amount)
    {
        if (GameManager.GameOverValues) return;

        score += amount;
        if (scoreText != null)
        {
            DOTween.To(() => displayScore, x => { displayScore = x; scoreText.text = Mathf.CeilToInt(x).ToString(); },
                score, 0.35f).SetEase(Ease.OutQuad);
        }
    }

    public void collectCoin(int scoreAmount)
    {
        if (GameManager.GameOverValues) return;

        increaseScore(scoreAmount);

        coinCount++;
        if (coinText != null)
        {
            coinText.text = coinCount.ToString();
            coinText.rectTransform.DOKill();
            coinText.rectTransform.localScale = Vector3.one;
            coinText.rectTransform.DOPunchScale(Vector3.one * 0.35f, 0.3f, 5, 0.5f);
        }

        if (coinProgressBar != null)
            DOTween.To(() => coinProgressBar.value, x => coinProgressBar.value = x, coinCount, 0.25f)
                   .SetEase(Ease.OutQuad);

        if (coinCount >= coinGoal && !hasThresholdFired)
        {
            hasThresholdFired = true;
            onThresholdReached?.Invoke();
        }
    }

    public void PlayCoinSpendAnimation(float duration)
    {
        if (coinText == null) return;
        float startValue = coinCount;
        DOTween.To(() => startValue, x => coinText.text = Mathf.CeilToInt(x).ToString(), 0f, duration)
               .SetEase(Ease.InQuad);
    }
}