using UnityEngine;
using Player.Animation.AnimationStates;
using Services;

namespace Player.Animation
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private MotionHandler _motionHandler;
        
        private Animator _animator;
        private AnimationBehavior _currentAnimationBehavior;

        private Coroutine _animationCoroutine;
        private bool _isEndAnimation = true;
        
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _currentAnimationBehavior = new AnimationBehaviorIdle(_animator);
            _currentAnimationBehavior.Enter();
        }
        
        private void PlayAnimation(float inputPositionY)
        {
            if (inputPositionY > 0 && _isEndAnimation)
            {
                _currentAnimationBehavior.Exit();
                _currentAnimationBehavior = new AnimationBehaviorJump(_animator);
                _currentAnimationBehavior.Enter();
                _isEndAnimation = false;
            }
            else if (inputPositionY < 0 && _isEndAnimation)
            {
                _currentAnimationBehavior.Exit();
                _currentAnimationBehavior = new AnimationBehaviorSlide(_animator);
                _currentAnimationBehavior.Enter();
                _isEndAnimation = false;
            }
        }

        private void StartRun()
        {
            _currentAnimationBehavior.Exit();
            _currentAnimationBehavior = new AnimationBehaviorRun(_animator);
            _currentAnimationBehavior.Enter();
        }
        
        private void DeathPlayer()
        {
            _currentAnimationBehavior.Exit();
            _currentAnimationBehavior = new AnimationBehaviorDeath(_animator);
            _currentAnimationBehavior.Enter();
        }
        
        private void StartAnimationAction()
        {
            _isEndAnimation = true;

            StartRun();
        }
        
        private void OnEnable()
        {
            _motionHandler.onStartPlayAnimation += PlayAnimation;
            
            EventBus.Instance.onGameStarted += StartRun;
            EventBus.Instance.onPlayerCollideWithObstacle += DeathPlayer;
            EventBus.Instance.onAdToReviveCompleted += StartRun;
        }

        private void OnDisable()
        {
            _motionHandler.onStartPlayAnimation -= PlayAnimation;
            
            EventBus.Instance.onGameStarted -= StartRun;
            EventBus.Instance.onPlayerCollideWithObstacle -= DeathPlayer;
            EventBus.Instance.onAdToReviveCompleted -= StartRun;
        }
    }
}