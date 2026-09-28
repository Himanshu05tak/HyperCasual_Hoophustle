using System;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class HoopManager : MonoBehaviour
{
    [SerializeField] private Hoop[] hoops;
    public ScoreManager ScoreManager { get; private set; }
    public GameTimer GameTimer { get; private set; }
    
    public Hoop currentTarget;

    public event Action<Hoop> OnHoopTargetChanged;
    private void Awake()
    {
        ScoreManager = GetComponentInChildren<ScoreManager>();
        GameTimer = GetComponentInChildren<GameTimer>();

        foreach (var hoop in hoops)
        {
            hoop.Initialize(ScoreManager, GameTimer, this);
        }
    }
    private void Start()
    {
        SelectRandomTarget();
    }

    public void SelectRandomTarget()
    {
        int randomIndex = Random.Range(0, hoops.Length);

        currentTarget = hoops[randomIndex];
        OnHoopTargetChanged?.Invoke(currentTarget);
        Debug.Log("Target Hoop: " + currentTarget.name);
    }

    public bool IsTarget(Hoop hoop)
    {
        return hoop == currentTarget;
    }
}