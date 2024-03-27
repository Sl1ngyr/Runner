using MainMenu;
using Player.AnimationStates;
using UnityEngine;
using Zenject;
using System.Collections;

namespace Player.Animation
{
    public class AnimationController : MonoBehaviour
    {
        [Inject] private MainMenuHandler _mainMenuHandler;

        private Animator _animator;
        private Movement _inputAction;
        private AnimationBehavior _currentAnimationBehavior;

        private Coroutine _animationCoroutine;

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
            while (true)
            {
                PlayAnimation();
                yield return new WaitForFixedUpdate();
            }
        }
        
        private void PlayAnimation()
        {
            Vector2 inputVector = _inputAction.Input.Move.ReadValue<Vector2>();
            
            if (inputVector.y > 0)
            {
                _currentAnimationBehavior.Exit();
                _currentAnimationBehavior = new AnimationBehaviorJump(_animator);
                _currentAnimationBehavior.Enter();
            }
            else if (inputVector.y < 0)
            {
                _currentAnimationBehavior.Exit();
                _currentAnimationBehavior = new AnimationBehaviorSlide(_animator);
                _currentAnimationBehavior.Enter();
            }
            else
            {
                _currentAnimationBehavior.Exit();
                _currentAnimationBehavior = new AnimationBehaviorRun(_animator);
                _currentAnimationBehavior.Enter();
            }
        }
        
        private void OnEnable()
        {
            _mainMenuHandler.onStateMenuChanged += StartPlayAnimation;
        }

        private void OnDisable()
        {
            _mainMenuHandler.onStateMenuChanged -= StartPlayAnimation;
        }
        
    }
}