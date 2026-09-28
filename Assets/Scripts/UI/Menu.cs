using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private UIFadeController menuUI;
        [SerializeField] private Button playBtn;
        [SerializeField] private Button quitBtn;

        [Header("InGameHUD")]
        [SerializeField] private UIFadeController gameplayUI;
        [SerializeField] private Button pauseBtn;

        [Header("Pause")]
        [SerializeField] private UIFadeController pauseUI;
        [SerializeField] private Button resumeBtn;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button backToMenuBtn;
        
        [Header("GameOver")]
        [SerializeField] private UIFadeController gameOverUI;
        [SerializeField] private Button replayBtn;
        [SerializeField] private Button backToMenuBtnFromGameOver;

        [SerializeField] private GameTimer gameTimer;
        public static Action<bool> onResume;

        private bool isPaused;
        private void OnEnable()
        {
            GameManager.OnGameStateChanged += HandleGameStateChanged;
            gameTimer.OnTimerFinished += GameTimer_OnTimerFinished;
        }

        private void GameTimer_OnTimerFinished()
        {
            GameManager.Instance.ChangeState(GameState.GameOver);
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= HandleGameStateChanged;
            gameTimer.OnTimerFinished -= GameTimer_OnTimerFinished;
        }

        private void Start()
        {
            playBtn.onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeState(GameState.Playing);
            });
            
            quitBtn.onClick.AddListener(Application.Quit);
            
            resumeBtn.onClick.AddListener(() =>
            {
                isPaused = false;
                GameManager.Instance.ChangeState(GameState.Playing);
                onResume?.Invoke(isPaused);
            });
            closeBtn.onClick.AddListener(() =>
            {
                isPaused = false;
                onResume?.Invoke(isPaused);
                GameManager.Instance.ChangeState(GameState.Playing);
            });
            backToMenuBtn.onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeState(GameState.Menu);
            });
            replayBtn.onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeState(GameState.Playing);
            });
            backToMenuBtnFromGameOver.onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeState(GameState.Menu);
            });
            pauseBtn.onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeState(GameState.Paused);
                isPaused = true;
                onResume?.Invoke(isPaused);
            });
        }

        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:
                    ShowMenu();
                    break;

                case GameState.Playing:
                    ShowGameplay();
                    break;

                case GameState.GameOver:
                    ShowGameOver();
                    break;

                case GameState.Paused:
                    ShowPause();
                    break;
            }
        }

        public void ShowMenu()
        {
            menuUI.FadeIn();
            gameplayUI.FadeOut();
            pauseUI.FadeOut();
            gameOverUI.FadeOut();
        }

        public void ShowGameplay()
        {
            menuUI.FadeOut();
            gameplayUI.FadeIn();
            pauseUI.FadeOut();
            gameOverUI.FadeOut();
        }

        public void ShowPause()
        {
            pauseUI.FadeIn();
        }

        public void ShowGameOver()
        {
            gameplayUI.FadeOut();
            gameOverUI.FadeIn();
        }
    }
}