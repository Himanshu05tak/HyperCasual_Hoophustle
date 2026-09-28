using UnityEngine;

/// <summary>
/// Smoothly moves a camera-rig target to trail behind/above a followed transform and look at it.
/// NOTE: in the original script this logic existed as FollowBall() but was never called from
/// Update, so it was dead code. It's wired up here — remove the Tick() call in
/// BasketBallController if you didn't actually want the camera following the ball.
/// </summary>
public class CameraFollowRig : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float followHeight = 2f;

    public void Tick()
    {
        if (cameraTarget == null || target == null) return;

        Vector3 followPos = target.position + Vector3.up * followHeight - target.forward * 2f;
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, followPos, Time.deltaTime * followSpeed);
        cameraTarget.LookAt(target);
    }
}
