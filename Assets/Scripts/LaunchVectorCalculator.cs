using UnityEngine;

/// <summary>
/// Pure math: turns a screen-space drag gesture into a world-space launch velocity.
/// No Unity lifecycle, no state — safe to call from anywhere (input preview, release, tests).
/// </summary>
public static class LaunchVectorCalculator
{
    public static Vector3 ComputeWorldLaunchVelocity(
        Vector2 dragStart,
        Vector2 dragCurrent,
        float dragForce,
        Transform cameraTransform)
    {
        Vector2 dragVector = dragCurrent - dragStart;
        Vector3 launchDirection = new Vector3(dragVector.x, dragVector.y, dragVector.magnitude).normalized;
        return cameraTransform.TransformDirection(launchDirection) * dragForce;
    }
}
