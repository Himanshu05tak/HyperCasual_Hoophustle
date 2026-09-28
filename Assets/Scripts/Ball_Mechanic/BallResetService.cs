using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Generic delayed-callback runner. Knows nothing about the ball itself — just "wait this long,
/// then invoke this action", with the ability to cancel/reschedule. Kept separate so the timing
/// mechanism (coroutines today) can change without touching BallLauncher.
/// </summary>
public class BallResetService : MonoBehaviour
{
    private Coroutine _resetRoutine;

    public void ScheduleReset(float delay, Action onReset)
    {
        if (_resetRoutine != null) StopCoroutine(_resetRoutine);
        _resetRoutine = StartCoroutine(ResetAfterDelay(delay, onReset));
    }

    public void CancelScheduledReset()
    {
        if (_resetRoutine == null) return;
        StopCoroutine(_resetRoutine);
        _resetRoutine = null;
    }

    private IEnumerator ResetAfterDelay(float delay, Action onReset)
    {
        yield return new WaitForSeconds(delay);
        _resetRoutine = null;
        onReset?.Invoke();
    }
}
