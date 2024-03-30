using Ad;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.EndGame
{
    public class EndGameMenu : MonoBehaviour
    {
        [Header("Menu data")]
        [SerializeField] private TextMeshProUGUI _endGameText;
        [SerializeField] private GameObject _endGameMenuObject;
        [SerializeField] private ReviveManager _reviveManager;
        
        [Space]
        [Header("Animation text data")]
        [SerializeField] private float duration;
        [SerializeField] private float _fadeValue = 0;
        [SerializeField] private float _appearValue = 1;
        
        [Inject] private EndGameHandler _endGameHandler;
        
        private void AnimationEndGameText()
        {
            var seq = DOTween.Sequence();
            seq.Append(_endGameText.DOFade(_fadeValue, duration));
            seq.Append(_endGameText.DOFade(_appearValue, duration));
        }
        
        private void EnableEndGameMenu()
        {
            _endGameMenuObject.gameObject.SetActive(true);
            AnimationEndGameText();
        }

        private void DisableEndGameMenu()
        {
            _endGameMenuObject.gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            _endGameHandler.onEndGameTriggered += EnableEndGameMenu;
            _reviveManager.onAdToReviveCompleted += DisableEndGameMenu;
        }

        private void OnDisable()
        {
            _endGameHandler.onEndGameTriggered -= EnableEndGameMenu;
            _reviveManager.onAdToReviveCompleted -= DisableEndGameMenu;
        }
        
    }
}