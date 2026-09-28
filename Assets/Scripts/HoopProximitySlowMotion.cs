using UnityEngine;

/// <summary>
/// Watches the ball's distance to the hoop while it's in flight and triggers a brief
/// Time.timeScale dip ("hit-stop") once it's been close for long enough.
/// </summary>
public class HoopProximitySlowMotion : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private Transform hoopTarget;
    [SerializeField] private float distanceThreshold = 1f;
    [SerializeField] private float nearHoopDuration = 0.25f;
    [SerializeField] private float slowMotionDuration = 0.5f;
    [SerializeField] private float slowMotionTimeScale = 0.2f;

    private float _timeNearHoop;
    private bool _isInSlowMotion;

    /// <param name="ballIsInPlay">Pass the launcher's HasBeenThrown so this only runs mid-flight.</param>
    public void Tick(bool ballIsInPlay)
    {
        if (!ballIsInPlay || _isInSlowMotion)
        {
            _timeNearHoop = 0f;
            return;
        }

        float distanceToHoop = Vector3.Distance(ballTransform.position, hoopTarget.position);
        if (distanceToHoop < distanceThreshold)
        {
            _timeNearHoop += Time.deltaTime;
            if (_timeNearHoop >= nearHoopDuration)
            {
                TriggerSlowMotion();
            }
        }
        else
        {
            _timeNearHoop = 0f;
        }
    }

    /// <summary>Call when the ball is reset so leftover state doesn't leak into the next shot.</summary>
    public void ResetState()
    {
        _timeNearHoop = 0f;
    }

    private void TriggerSlowMotion()
    {
        _isInSlowMotion = true;
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        Invoke(nameof(ResetTimeScale), slowMotionDuration);
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        _isInSlowMotion = false;
        _timeNearHoop = 0f;
    }
}
