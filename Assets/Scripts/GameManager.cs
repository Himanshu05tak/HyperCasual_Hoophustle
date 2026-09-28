using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private GameState _currentGameState;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.Menu);
    }

    public void ChangeState(GameState newGameState)
    {
        _currentGameState = newGameState;
        OnGameStateChanged?.Invoke(_currentGameState);
    }
    public void StartGame()
    {
        
    }

    public void GameOver()
    {
        
    }

    public void RestartGame()
    {
        
    }

    public void ReturnToMenu()
    {
        
    }
    
}
