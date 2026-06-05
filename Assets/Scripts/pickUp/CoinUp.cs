using UnityEngine;

public class CoinUp : pickUp
{
    [SerializeField] private int scoreAmount = 100;
    scoreManager sscoreManager;

    public void init(scoreManager scoreManager)
    {
        this.sscoreManager = scoreManager;
    }

    protected override void pickUpEffect()
    {
        if (sscoreManager != null)
        {
            // Call collectCoin so the manager knows it was a coin, not an apple
            sscoreManager.collectCoin(scoreAmount);
        }
    }
}