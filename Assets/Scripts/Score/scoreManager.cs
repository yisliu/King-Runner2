using UnityEngine;
using TMPro;
using System; // Required for Action

public class scoreManager : MonoBehaviour
{
    [SerializeField] gameManager GameManager;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text coinText;
    [SerializeField] int coinGoal = 50;

    private int coinCount = 0;
    private int score = 0;
    private bool hasThresholdFired = false;

    public static event Action onThresholdReached;

	private void Start()
	{
    	if (coinText != null)
    	{
        	coinText.text = coinCount.ToString(); 
    	}
    
    	if (scoreText != null)
    	{
        	scoreText.text = score.ToString();
    	}
	}

    public void increaseScore(int amount)
    {
        if (GameManager.GameOverValues) return;
        
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
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
        }

        if (coinCount >= coinGoal && !hasThresholdFired)
        {
            hasThresholdFired = true;
			Debug.Log(coinCount);
            Debug.Log("You can now board the ship! : )");
            
            onThresholdReached?.Invoke();
        }
    }
}