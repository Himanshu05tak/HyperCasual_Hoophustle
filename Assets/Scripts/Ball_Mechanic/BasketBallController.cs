using UnityEngine;

/// <summary>
/// Orchestrator only: wires BallDragInput -> TrajectoryPredictor / BallLauncher, and drives
/// per-frame ticks for the other components. No physics, math, or Unity-input code lives here.
/// </summary>
[RequireComponent(typeof(BallDragInput))]
[RequireComponent(typeof(BallLauncher))]
public class BasketBallController : MonoBehaviour
{
    [SerializeField] private BallDragInput dragInput;
    [SerializeField] private TrajectoryPredictor trajectoryPredictor;
    [SerializeField] private BallLauncher ballLauncher;
    [SerializeField] private HoopProximitySlowMotion hoopSlowMotion; // optional
    [SerializeField] private CameraFollowRig cameraFollowRig;         // optional
    [SerializeField] private BallPhysicsProfile ballPhysicsProfile; 

    private void Reset()
    {
        dragInput = GetComponent<BallDragInput>();
        ballLauncher = GetComponent<BallLauncher>();
    }

    private void OnEnable()
    {
        dragInput.DragUpdated += HandleDragUpdated;
        dragInput.DragReleased += HandleDragReleased;
        ballLauncher.Reset += HandleBallReset;
    }

    private void OnDisable()
    {
        dragInput.DragUpdated -= HandleDragUpdated;
        dragInput.DragReleased -= HandleDragReleased;
        ballLauncher.Reset -= HandleBallReset;
    }

    private void Update()
    {
        dragInput.Tick();
        hoopSlowMotion?.Tick(ballLauncher.HasBeenThrown);
        cameraFollowRig?.Tick();
    }

    private void HandleDragUpdated(Vector3 worldLaunchVelocity)
    {
        trajectoryPredictor.Show(transform.position, worldLaunchVelocity);
    }

    private void HandleDragReleased(Vector3 worldLaunchVelocity)
    {
        trajectoryPredictor.Clear();
        ballLauncher.Launch(worldLaunchVelocity);
    }

    private void HandleBallReset()
    {
        hoopSlowMotion?.ResetState();
        //ballPhysicsProfile.ApplyRandomProfile();
    }
}
