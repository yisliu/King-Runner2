using UnityEngine;
using TMPro;

public class scoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] gameManager GameManager;
    [SerializeField] TMP_Text scoreText;
    int score = 0;

    public void increaseScore(int amount)
    {
        if(GameManager.GameOverValues) return;
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
