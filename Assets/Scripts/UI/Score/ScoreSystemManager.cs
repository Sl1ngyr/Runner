using System.Collections;
using Ad;
using Player;
using TMPro;
using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace UI.Score
{
    public class ScoreSystemManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private int _valuationСalculationModifier = 1;
        [SerializeField] private string _initialText = "Score: ";
        [SerializeField] private float _timeToAddModifier = 0.7f;

        private int _scoreCalculation;
        private Coroutine _startCalculationScoreCoroutine;
        
        [Inject] private MainMenuHandler _mainMenuHandler;
        [Inject] private CollisionDetector _collisionDetector;
        [Inject] private EndGameHandler _endGameHandler;
        [Inject] private ReviveManager _reviveManager;
        
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
            _mainMenuHandler.onGameStarted += StartCalculationScore;
            _collisionDetector.onObstacleDetected += StopCalculationScore;
            _endGameHandler.onEndGameTriggered += DisableScoreText;
            _reviveManager.onAdToReviveCompleted += StartCalculationScore;
        }

        private void OnDisable()
        {
            _mainMenuHandler.onGameStarted -= StartCalculationScore;
            _collisionDetector.onObstacleDetected -= StopCalculationScore;
            _endGameHandler.onEndGameTriggered -= DisableScoreText;
            _reviveManager.onAdToReviveCompleted -= StartCalculationScore;
        }
        
    }
}