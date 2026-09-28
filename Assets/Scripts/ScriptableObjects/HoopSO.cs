using UnityEngine;

namespace DefaultNamespace
{
    public enum HoopType
    {
        Normal,
        Bonus,
        Time,
        Penalty
    }

    [CreateAssetMenu(
        fileName = "HoopData",
        menuName = "Basketball/Hoop"
    )]
    public class HoopSO : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private HoopType hoopType;
        [SerializeField] private string hoopName;

        [Header("Reward")]
        [SerializeField] private int scoreGain = 10;
        [SerializeField] private int timeGain = 3;

        [Header("Penalty")]
        [SerializeField] private int scoreLoss = 0;
        [SerializeField] private int timeLoss = 0;

        public HoopType HoopType => hoopType;
        public string HoopName => hoopName;

        public int ScoreGain => scoreGain;
        public int TimeGain => timeGain;

        public int ScoreLoss => scoreLoss;
        public int TimeLoss => timeLoss;
    }
}