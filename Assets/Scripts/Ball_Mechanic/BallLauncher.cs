using System;
using UnityEngine;

/// <summary>
/// Owns the ball's Rigidbody and its "thrown" state. Applies launch impulses and, via
/// BallResetService, restores the ball to its start position after a delay.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BallResetService))]
public class BallLauncher : MonoBehaviour
{
    [SerializeField] private float resetTime = 3f;

    // Static so external systems (UI, avatar, audio) can subscribe without a scene reference,
    // same as the original script's public static Action<bool> OnThrow.
    public static event Action<bool> OnThrow;

    /// <summary>Raised after the ball has been returned to its start position.</summary>
    public event Action Reset;

    private Rigidbody _rigidbody;
    private BallResetService _resetService;
    private Vector3 _initialPosition;

    public bool HasBeenThrown { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _resetService = GetComponent<BallResetService>();
        _rigidbody.useGravity = false;
        _initialPosition = transform.position;
    }

    public void Launch(Vector3 worldLaunchVelocity)
    {
        _rigidbody.AddForce(worldLaunchVelocity, ForceMode.Impulse);
        _rigidbody.useGravity = true;
        HasBeenThrown = true;

        OnThrow?.Invoke(true);

        _resetService.ScheduleReset(resetTime, ResetBall);
    }

    private void ResetBall()
    {
        transform.position = _initialPosition;
        _rigidbody.useGravity = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        HasBeenThrown = false;

        OnThrow?.Invoke(false);
        Reset?.Invoke();
    }
}
