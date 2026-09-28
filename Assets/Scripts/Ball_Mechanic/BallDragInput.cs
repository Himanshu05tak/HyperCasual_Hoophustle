using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Owns mouse/drag detection only. Knows nothing about physics, trajectories or launching —
/// it just reports "here's the world-space launch velocity the player is dragging out" via events.
/// </summary>
public class BallDragInput : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private ShotUI shotUI;
    [SerializeField] private LayerMask ballLayerMask;
    [SerializeField] private float rayDistance = 100f;

    public event Action<Vector3> DragStarted;      // ball world position
    public event Action<Vector3> DragUpdated;       // preview world launch velocity
    public event Action<Vector3> DragReleased;      // final world launch velocity

    private Camera _camera;
    private Vector2 _dragStartPoint;
    private bool _isDragging;

    public bool IsDragging => _isDragging;

    private void Awake()
    {
        _camera = Camera.main;
        if (ballTransform == null) ballTransform = transform;
    }

    /// <summary>Call once per frame from the orchestrator's Update.</summary>
    public void Tick()
    {
        HandlePressStart();
        HandleDragging();
        HandleRelease();
    }

    private void HandlePressStart()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out var hit, rayDistance, ballLayerMask)) return;
        if (hit.collider.gameObject != ballTransform.gameObject) return;

        _dragStartPoint = Mouse.current.position.ReadValue();
        _isDragging = true;
        shotUI.StartDrag(_dragStartPoint);
        DragStarted?.Invoke(ballTransform.position);
    }

    private void HandleDragging()
    {
        if (!_isDragging || !Mouse.current.leftButton.isPressed) return;

        Vector2 currentDrag = Mouse.current.position.ReadValue();
        float dragForce = shotUI.UpdateDrag(currentDrag);

        Vector3 worldLaunchVelocity = LaunchVectorCalculator.ComputeWorldLaunchVelocity(
            _dragStartPoint, currentDrag, dragForce, _camera.transform);

        DragUpdated?.Invoke(worldLaunchVelocity);
    }

    private void HandleRelease()
    {
        if (!_isDragging || !Mouse.current.leftButton.wasReleasedThisFrame) return;

        Vector2 dragEndPoint = Mouse.current.position.ReadValue();
        float dragForce = shotUI.UpdateDrag(dragEndPoint);
        shotUI.EndDrag();

        Vector3 worldLaunchVelocity = LaunchVectorCalculator.ComputeWorldLaunchVelocity(
            _dragStartPoint, dragEndPoint, dragForce, _camera.transform);

        _isDragging = false;
        DragReleased?.Invoke(worldLaunchVelocity);
    }
}
