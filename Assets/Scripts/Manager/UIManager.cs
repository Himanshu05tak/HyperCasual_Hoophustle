using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameTimer gameTimer;
    
    private void OnEnable()
    {
        scoreManager.OnScoreChanged += UpdateScore;
        scoreManager.OnHighScoreChanged += UpdateHighScore;
        gameTimer.OnTimeChanged += UpdateTimer;
    }

    private void OnDisable()
    {
        scoreManager.OnScoreChanged -= UpdateScore;
        scoreManager.OnHighScoreChanged -= UpdateHighScore;
        gameTimer.OnTimeChanged -= UpdateTimer;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    private void UpdateHighScore(int highScore)
    {
        highScoreText.text = highScore.ToString();
    }

    private void UpdateTimer(int time)
    {
        timerText.text = time.ToString();
    }
}
