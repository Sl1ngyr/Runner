using UnityEngine;
using EventBus = Services.EventBus;
using System;

namespace Player
{
    public class MotionHandler : MonoBehaviour
    {
        [SerializeField] private float _swipeResistance = 100;
        
        private const int RIGHT_BORDER_POSISITION_X = 1;
        private const int LEFT_BORDER_POSISITION_X = -1;

        private int _currentBorderPosition;
        
        private bool _isStartMoving = false;
        
        private Movement _movementInputAction;


        private Vector2 _initialPosition;
        private Vector2 _currentPosition => _movementInputAction.Input.Position.ReadValue<Vector2>();

        public event Action<float> onStartPlayAnimation;

        private void Awake()
        {
            _movementInputAction = new Movement();
            _movementInputAction.Input.Enable();

#if UNITY_EDITOR
            _movementInputAction.Input.Move.performed += _ => Move();
            _movementInputAction.Input.Move.performed += _ => PlayAnimationForKeyboard();
      
#elif UNITY_ANDROID
            _movementInputAction.Input.TouchPress.performed += _ => { _initialPosition = _currentPosition; };
            _movementInputAction.Input.TouchPress.canceled += _ => DetectSwipe();
#endif
        }

        private void DetectSwipe()
        {
            Vector2 delta = _currentPosition - _initialPosition;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y) && Mathf.Abs(delta.x) > _swipeResistance && _isStartMoving)
            {
                Move();
            }
            else if(Mathf.Abs(delta.y) > Mathf.Abs(delta.x) && Mathf.Abs(delta.y) > _swipeResistance && _isStartMoving)
            {
                onStartPlayAnimation?.Invoke(delta.y);
            }
        }

        private void Move()
        {
            Vector2 inputVector = _movementInputAction.Input.Move.ReadValue<Vector2>();

            if (_isStartMoving)
            {
                if (inputVector.x > 0 && _currentBorderPosition != RIGHT_BORDER_POSISITION_X)
                {
                    _currentBorderPosition += RIGHT_BORDER_POSISITION_X;

                    transform.position = new Vector3(_currentBorderPosition, transform.position.y, 0);
                }
                else if (inputVector.x < 0 && _currentBorderPosition != LEFT_BORDER_POSISITION_X)
                {
                    _currentBorderPosition += LEFT_BORDER_POSISITION_X;

                    transform.position = new Vector3(_currentBorderPosition, transform.position.y, 0);
                }
            }

        }

        private void PlayAnimationForKeyboard()
        {
            Vector2 inputVector = _movementInputAction.Input.Move.ReadValue<Vector2>();
            if (_isStartMoving)
            {
                onStartPlayAnimation?.Invoke(inputVector.y);
            }
        }
        
        private void StartMove()
        {
            _isStartMoving = true;
        }

        private void StopMove()
        {
            _isStartMoving = false;
        }

        private void OnEnable()
        {
            EventBus.Instance.onGameStarted += StartMove;
            EventBus.Instance.onPlayerCollideWithObstacle += StopMove;
            EventBus.Instance.onAdToReviveCompleted += StartMove;
        }

        private void OnDisable()
        {
            EventBus.Instance.onGameStarted -= StartMove;
            EventBus.Instance.onPlayerCollideWithObstacle -= StopMove;
            EventBus.Instance.onAdToReviveCompleted -= StartMove;
        }

    }
    
}
