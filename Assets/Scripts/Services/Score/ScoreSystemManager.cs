using System.Collections;
using TMPro;
using UnityEngine;

namespace Services.Score
{
    public class ScoreSystemManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private int _valuationСalculationModifier = 1;
        [SerializeField] private string _initialText = "Score: ";
        [SerializeField] private float _timeToAddModifier = 0.7f;

        private int _scoreCalculation;
        private Coroutine _startCalculationScoreCoroutine;

        private void StartCalculationScore()
        {
            ActiveScoreText();
            _startCalculationScoreCoroutine = StartCoroutine(CoroutineCalculationScore());
        }

        private void StopCalculationScore()
        {
            StopCoroutine(_startCalculationScoreCoroutine);
        }
        
        private IEnumerator CoroutineCalculationScore()
        {
            while (true)
            {
                CalculationScore();
                yield return new WaitForSeconds(_timeToAddModifier);
            }
        }

        private void CalculationScore()
        {
            _scoreCalculation += _valuationСalculationModifier;
            _scoreText.text = _initialText + _scoreCalculation;
        }

        private void ActiveScoreText()
        {
            _scoreText.gameObject.SetActive(true);
        }

        private void DisableScoreText()
        {
            _scoreText.gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            EventBus.Instance.onGameStarted += StartCalculationScore;
            EventBus.Instance.onPlayerCollideWithObstacle += StopCalculationScore;
            EventBus.Instance.onPlayerDead += DisableScoreText;
            EventBus.Instance.onAdToReviveCompleted += StartCalculationScore;
        }

        private void OnDisable()
        {
            EventBus.Instance.onGameStarted -= StartCalculationScore;
            EventBus.Instance.onPlayerCollideWithObstacle -= StopCalculationScore;
            EventBus.Instance.onPlayerDead -= DisableScoreText;
            EventBus.Instance.onAdToReviveCompleted -= StartCalculationScore;
        }
        
    }
}