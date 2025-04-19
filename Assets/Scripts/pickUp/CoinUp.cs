using UnityEngine;

public class CoinUp : pickUp
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            sscoreManager.increaseScore(scoreAmount);
        }
    }
}
