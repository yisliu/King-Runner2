using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] playerCollison  player;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] private float startTime = 5f;
    private float timeLeft;
    bool gameover = false;

    public bool GameOverValues
    {
        get{ return gameover;} 
    }
    void Start()
    {
        timeLeft = startTime;
        gameOverPanel.SetActive(false);

    }
    
    

    // Update is called once per frame
    void Update()
    {
        timer();
    }

    public void IncreaseTime(float timeAdded)
    {
        timeLeft += timeAdded;
    }

    public void timer()
    {
        if (gameover)
        {
            return;
        }
        timeLeft -= Time.deltaTime;
        timeText.text = timeLeft.ToString("F1");

        if (timeLeft <= 0f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameover = true;
        player.enabled = false;
        gameOverPanel.SetActive(true);
        Time.timeScale = .1f;
    }
}
