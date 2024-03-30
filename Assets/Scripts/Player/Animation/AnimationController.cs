using UnityEngine;
using System.Collections;
using Player.Animation.AnimationStates;
using Services;

namespace Player.Animation
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Movement _inputAction;
        private AnimationBehavior _currentAnimationBehavior;
        

        private Coroutine _animationCoroutine;
        private bool _isEndAnimation = true;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _inputAction = new Movement();
            _inputAction.Input.Enable();
            
            _currentAnimationBehavior = new AnimationBehaviorIdle(_animator);
            _currentAnimationBehavior.Enter();
        }


        private void StartPlayAnimation()
        {
            _animationCoroutine = StartCoroutine(CoroutineAnimation());
        }

        private IEnumerator CoroutineAnimation()
        {
            _isEndAnimation = true;
            while (true)
            {
                PlayAnimation();
                yield return new WaitForFixedUpdate();
            }
        }
        
        private void PlayAnimation()
        {
            Vector2 inputVector = _inputAction.Input.Move.ReadValue<Vector2>();
            
            if (inputVector.y > 0 && _isEndAnimation)
            {
                _currentAnimationBehavior.Exit();
                _currentAnimationBehavior = new AnimationBehaviorJump(_animator);
                _currentAnimationBehavior.Enter();
                _isEndAnimation = false;
            }
            else if (inputVector.y < 0 && _isEndAnimation)
            {
                _currentAnimationBehavior.Exit();
                _currentAnimationBehavior = new AnimationBehaviorSlide(_animator);
                _currentAnimationBehavior.Enter();
                _isEndAnimation = false;
            }
            else if(_isEndAnimation)
            {
                _currentAnimationBehavior.Exit();
                _currentAnimationBehavior = new AnimationBehaviorRun(_animator);
                _currentAnimationBehavior.Enter();
            }
        }
        
        private void DeathPlayer()
        {
            StopCoroutine(_animationCoroutine);
            _currentAnimationBehavior.Exit();
            _currentAnimationBehavior = new AnimationBehaviorDeath(_animator);
            _currentAnimationBehavior.Enter();
        }
        
        private void OnEnable()
        {
            EventBus.Instance.onGameStarted += StartPlayAnimation;
            EventBus.Instance.onPlayerCollideWithObstacle += DeathPlayer;
            EventBus.Instance.onAdToReviveCompleted += StartPlayAnimation;
        }

        private void OnDisable()
        {
            EventBus.Instance.onGameStarted -= StartPlayAnimation;
            EventBus.Instance.onPlayerCollideWithObstacle -= DeathPlayer;
            EventBus.Instance.onAdToReviveCompleted -= StartPlayAnimation;
        }

        private void StartAnimationAction()
        {
            _isEndAnimation = true;
        }
        
    }
}