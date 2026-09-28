// using System.Collections;
// using UnityEngine;
//
// /// <summary>
// /// Owns the trajectory LineRenderer. Given a start position/velocity it predicts a projectile
// /// path (stopping early on a backboard/rim hit) and either sets it instantly or animates it in.
// /// </summary>
// public class TrajectoryPredictor : MonoBehaviour
// {
//     [SerializeField] private LineRenderer trajectoryLine;
//     [SerializeField] private int trajectoryPoints = 30;
//     [SerializeField] private float timeStep = 0.05f;
//     [SerializeField] private LayerMask obstacleLayerMask;
//     [SerializeField] private bool animateTrajectory;
//     [SerializeField] private float animationStepDelay = 0.01f;
//
//     private Coroutine _animationRoutine;
//
//     private void Awake()
//     {
//         trajectoryLine.enabled = false;
//     }
//
//     public void Show(Vector3 startPos, Vector3 startVelocity)
//     {
//         trajectoryLine.enabled = true;
//         Vector3[] points = BuildPoints(startPos, startVelocity, out int pointCount);
//
//         if (animateTrajectory)
//         {
//             if (_animationRoutine != null) StopCoroutine(_animationRoutine);
//             _animationRoutine = StartCoroutine(AnimatePoints(points, pointCount));
//         }
//         else
//         {
//             trajectoryLine.positionCount = pointCount;
//             for (int i = 0; i < pointCount; i++)
//                 trajectoryLine.SetPosition(i, points[i]);
//         }
//     }
//
//     public void Clear()
//     {
//         if (_animationRoutine != null)
//         {
//             StopCoroutine(_animationRoutine);
//             _animationRoutine = null;
//         }
//
//         trajectoryLine.positionCount = 0;
//         trajectoryLine.enabled = false;
//     }
//
//     private Vector3[] BuildPoints(Vector3 startPos, Vector3 startVelocity, out int pointCount)
//     {
//         var points = new Vector3[trajectoryPoints];
//         points[0] = startPos;
//         Vector3 previousPoint = startPos;
//
//         for (int i = 1; i < trajectoryPoints; i++)
//         {
//             float t = i * timeStep;
//             Vector3 newPoint = startPos + startVelocity * t + Physics.gravity * (0.5f * t * t);
//
//             if (Physics.Linecast(previousPoint, newPoint, out RaycastHit hit, obstacleLayerMask))
//             {
//                 points[i] = hit.point;
//                 pointCount = i + 1;
//                 return points;
//             }
//
//             points[i] = newPoint;
//             previousPoint = newPoint;
//         }
//
//         pointCount = trajectoryPoints;
//         return points;
//     }
//
//     private IEnumerator AnimatePoints(Vector3[] points, int pointCount)
//     {
//         trajectoryLine.positionCount = 0;
//         for (int i = 0; i < pointCount; i++)
//         {
//             trajectoryLine.positionCount = i + 1;
//             trajectoryLine.SetPosition(i, points[i]);
//             yield return new WaitForSeconds(animationStepDelay);
//         }
//         _animationRoutine = null;
//     }
// }
using System.Collections;
using UnityEngine;

/// <summary>
/// Owns the trajectory dots. Given a start position/velocity it predicts a projectile
/// path (stopping early on a backboard/rim hit) and either sets it instantly or animates it in.
/// </summary>
public class TrajectoryPredictor : MonoBehaviour
{
    [Header("Trajectory Dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;
    [SerializeField] private int trajectoryPoints = 30;
    [SerializeField] private float timeStep = 0.05f;

    [Header("Collision")]
    [SerializeField] private LayerMask obstacleLayerMask;

    [Header("Animation")]
    [SerializeField] private bool animateTrajectory;
    [SerializeField] private float animationStepDelay = 0.01f;

    [Header("Dot Appearance")]
    [SerializeField] private float startDotScale = .1f;
    [SerializeField] private float endDotScale = 0.01f;
    [SerializeField] private float startAlpha = 1f;
    [SerializeField] private float endAlpha = 0.15f;
    
    private GameObject[] _dots;
    private Coroutine _animationRoutine;
    private MaterialPropertyBlock _propertyBlock;
    private void Awake()
    {
        CreateDots();
        HideAllDots();
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void CreateDots()
    {
        _dots = new GameObject[trajectoryPoints];

        Transform parent = dotParent != null ? dotParent : transform;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            _dots[i] = Instantiate(dotPrefab,parent);
            _dots[i].transform.localScale = dotPrefab.transform.localScale;
  
            _dots[i].SetActive(false);
        }
    }

    public void Show(Vector3 startPos, Vector3 startVelocity)
    {
        Vector3[] points = BuildPoints(startPos, startVelocity, out int pointCount);

        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
            _animationRoutine = null;
        }

        if (animateTrajectory)
        {
            _animationRoutine = StartCoroutine(
                AnimatePoints(points, pointCount)
            );
        }
        else
        {
            SetPoints(points, pointCount);
        }
    }

    public void Clear()
    {
        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
            _animationRoutine = null;
        }

        HideAllDots();
    }

    private Vector3[] BuildPoints(
        Vector3 startPos,
        Vector3 startVelocity,
        out int pointCount)
    {
        var points = new Vector3[trajectoryPoints];

        points[0] = startPos;
        Vector3 previousPoint = startPos;

        for (int i = 1; i < trajectoryPoints; i++)
        {
            float t = i * timeStep;

            Vector3 newPoint =
                startPos +
                startVelocity * t +
                Physics.gravity * (0.5f * t * t);

            if (Physics.Linecast(
                previousPoint,
                newPoint,
                out RaycastHit hit,
                obstacleLayerMask))
            {
                points[i] = hit.point;

                pointCount = i + 1;

                return points;
            }

            points[i] = newPoint;
            previousPoint = newPoint;
        }

        pointCount = trajectoryPoints;

        return points;
    }

    private void SetPoints(Vector3[] points, int pointCount)
    {
        for (int i = 0; i < _dots.Length; i++)
        {
            if (i < pointCount)
            {
                _dots[i].transform.position = points[i];

                UpdateDotAppearance(i, pointCount);

                _dots[i].SetActive(true);
            }
            else
            {
                _dots[i].SetActive(false);
            }
        }
    }

    private IEnumerator AnimatePoints(
        Vector3[] points,
        int pointCount)
    {
        HideAllDots();

        for (int i = 0; i < pointCount; i++)
        {
            _dots[i].transform.position = points[i];

            UpdateDotAppearance(i, pointCount);

            _dots[i].SetActive(true);

            yield return new WaitForSeconds(animationStepDelay);
        }

        _animationRoutine = null;
    }

    private void UpdateDotAppearance(int index, int pointCount)
    {
        float normalizedPosition =
            pointCount <= 1
                ? 0f
                : index / (float)(pointCount - 1);

        // Scale
        float scale = Mathf.Lerp(
            startDotScale,
            endDotScale,
            normalizedPosition
        );
        
        _dots[index].transform.localScale =
            Vector3.one * scale;

        //Alpha
        SpriteRenderer sprite =
            _dots[index].GetComponent<SpriteRenderer>();
        
        if (sprite != null)
        {
            Color color = sprite.color;
        
            color.a = Mathf.Lerp(
                startAlpha,
                endAlpha,
                normalizedPosition
            );
        
            sprite.color = color;
        }
    }

    private void HideAllDots()
    {
        if (_dots == null)
            return;

        for (int i = 0; i < _dots.Length; i++)
        {
            if (_dots[i] != null)
                _dots[i].SetActive(false);
        }
    }
}