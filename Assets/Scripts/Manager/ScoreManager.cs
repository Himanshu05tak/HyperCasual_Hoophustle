using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }
    public int HighScore { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnHighScoreChanged;

    public void AddScore(int amount)
    {
        Score += amount;

        OnScoreChanged?.Invoke(Score);

        if (Score > HighScore)
        {
            HighScore = Score;
            OnHighScoreChanged?.Invoke(HighScore);
        }
    }
}