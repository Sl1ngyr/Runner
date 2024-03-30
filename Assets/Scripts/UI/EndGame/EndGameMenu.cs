using DG.Tweening;
using Services;
using Services.Ad;
using TMPro;
using UnityEngine;

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
            EventBus.Instance.onPlayerDead += EnableEndGameMenu;
            EventBus.Instance.onAdToReviveCompleted += DisableEndGameMenu;
        }

        private void OnDisable()
        {
            EventBus.Instance.onPlayerDead -= EnableEndGameMenu;
            EventBus.Instance.onAdToReviveCompleted -= DisableEndGameMenu;
        }
        
    }
}