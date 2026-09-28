using UnityEngine;

namespace DefaultNamespace
{
    public class Hoop : MonoBehaviour
    {
        [SerializeField] private HoopSO hoopData;

        private HoopManager _hoopManager;
        private ScoreManager _scoreManager;
        private GameTimer _gameTimer;
        
        public void Initialize(
            ScoreManager scoreManager,
            GameTimer gameTimer,
            HoopManager hoopManager)
        {
            this._scoreManager = scoreManager;
            this._gameTimer = gameTimer;
            this._hoopManager = hoopManager;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Basketball"))
                return;

            if (_hoopManager.IsTarget(this))
            {
                CorrectHoop();
            }
            else
            {
                WrongHoop();
            }
        }

        private void CorrectHoop()
        {
            Debug.Log("CORRECT HOOP!");

            _scoreManager.AddScore(hoopData.ScoreGain);
            _gameTimer.AddExtraTime(hoopData.TimeGain);

            _hoopManager.SelectRandomTarget();
        }

        private void WrongHoop()
        {
            Debug.Log("WRONG HOOP!");
            
            _gameTimer.ReduceTime(hoopData.TimeLoss);
        }
        
        public HoopSO HoopData => hoopData;
    }
}