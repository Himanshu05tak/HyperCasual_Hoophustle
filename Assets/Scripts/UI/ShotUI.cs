using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Converts a screen-space drag gesture into a shot force value, and optionally drives a
/// power-bar UI (fill image + indicator root) while the player is dragging.
///
/// Called by BallDragInput as:
///   StartDrag(dragStartPoint)         - drag began
///   UpdateDrag(currentPoint) -> force - called every frame while dragging (also on release)
///   EndDrag()                         - drag finished/cancelled
/// </summary>
public class ShotUI : MonoBehaviour
{
    [Header("Force Mapping")]
    [SerializeField] private float minDragDistance = 10f;   // pixels; below this, force is minForce
    [SerializeField] private float maxDragDistance = 300f;  // pixels; at/above this, force is maxForce
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxForce = 20f;
    [SerializeField] private AnimationCurve forceCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("UI (optional)")]
    [SerializeField] private GameObject powerIndicatorRoot; // shown while dragging, hidden otherwise
    [SerializeField] private Image powerFillImage;          // fillAmount driven by drag strength (0-1)
    [SerializeField] private RectTransform dragHandle;      // optional visual that follows the pointer

    private Vector2 _dragStartPoint;
    private float _lastNormalizedStrength;

    public float LastForce { get; private set; }
    public float LastNormalizedStrength => _lastNormalizedStrength;

    public void StartDrag(Vector2 screenStartPoint)
    {
        _dragStartPoint = screenStartPoint;
        _lastNormalizedStrength = 0f;
        LastForce = minForce;

        if (powerIndicatorRoot != null) powerIndicatorRoot.SetActive(true);
        UpdateVisuals(screenStartPoint);
    }

    /// <summary>Call every frame while dragging. Returns the current shot force.</summary>
    public float UpdateDrag(Vector2 currentScreenPoint)
    {
        float dragDistance = Vector2.Distance(_dragStartPoint, currentScreenPoint);

        float normalized = Mathf.InverseLerp(minDragDistance, maxDragDistance, dragDistance);
        normalized = Mathf.Clamp01(normalized);
        _lastNormalizedStrength = forceCurve.Evaluate(normalized);

        LastForce = Mathf.Lerp(minForce, maxForce, _lastNormalizedStrength);

        UpdateVisuals(currentScreenPoint);

        return LastForce;
    }

    public void EndDrag()
    {
        if (powerIndicatorRoot != null) powerIndicatorRoot.SetActive(false);
    }

    private void UpdateVisuals(Vector2 currentScreenPoint)
    {
        if (powerFillImage != null)
            powerFillImage.fillAmount = _lastNormalizedStrength;

        if (dragHandle != null)
            dragHandle.position = currentScreenPoint;
    }
}
