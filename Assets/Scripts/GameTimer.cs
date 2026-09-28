using System;
using UnityEditor;
using UnityEngine;
using Menu = UI.Menu;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float timeRemaining;

    public float TimeRemaining => timeRemaining;
    public event Action<int> OnTimeChanged;
    public event Action OnTimerFinished;

    private int _lastDisplayedSecond;
    private bool _timerFinished;
    private bool _isPaused;

    private void OnEnable()
    {
        Menu.onResume += OnResume;
    }

    private void OnDisable()
    {
        Menu.onResume -= OnResume;
    }
    
    private void OnResume(bool resume)
    {
        _isPaused = resume;
    }

    private void Update()
    {
        if(_isPaused) return;
        if (_timerFinished)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            _timerFinished = true;

            OnTimeChanged?.Invoke(0);
            OnTimerFinished?.Invoke();

            return;
        }

        int currentSecond = Mathf.CeilToInt(timeRemaining);

        if (currentSecond != _lastDisplayedSecond)
        {
            _lastDisplayedSecond = currentSecond;
            OnTimeChanged?.Invoke(currentSecond);
        }
    }

    public void AddExtraTime(int extraTime)
    {
        timeRemaining += extraTime;
    }

    public void ReduceTime(int reduceTime)
    {
        timeRemaining -= reduceTime;
    }
}