using UnityEngine;

/// <summary>
/// Preserves the original script's debug-only OnTriggerEnter logging.
/// Safe to delete once you no longer need it — it has no gameplay effect.
/// </summary>
public class TriggerDebugLogger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by {other.name}");
    }
}
