using System.Collections;
using Ad;
using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Player
{
    public class MotionHandler : MonoBehaviour
    {
        [SerializeField] private int _swipeDistance = 1;
        [SerializeField] private float _speed = 2;
        
        private bool _waitingForReachPoint = true;
        private Vector3 _distance = Vector3.zero;
        private int _finalValueOfPosition;
        
        private Rigidbody _capsuleRigidbody;
        private Movement _inputAction;
        private CollisionDetector _collisionDetector;
        private Coroutine _moveCoroutine;
        
        [Inject] private MainMenuHandler _mainMenuHandler;
        [Inject] private ReviveManager _reviveManager; 
        
        private void Awake()
        {
            _capsuleRigidbody = GetComponent<Rigidbody>();
            _collisionDetector = GetComponent<CollisionDetector>();
            
            _inputAction = new Movement();
            _inputAction.Input.Enable();
        }
        
        private void StartMove()
        {
            _moveCoroutine = StartCoroutine(CoroutineMove());
        }

        private void StopMove()
        {
            StopCoroutine(_moveCoroutine);
        }
        
        private IEnumerator CoroutineMove()
        {
            while (true)
            {
                Move();
                yield return new WaitForFixedUpdate();
            }
        }

        private void Move()
        {
            if (_waitingForReachPoint)
            {
                Vector2 inputVector = _inputAction.Input.Move.ReadValue<Vector2>();
                
                _distance = Vector3.zero;
                
                if (inputVector.x > 0 && transform.position.x < _swipeDistance)
                {
                    _distance.x += _swipeDistance;
                    _finalValueOfPosition = (int)transform.position.x + (int)_distance.x;
                    MoveDirectionTo(_distance);
                }
                else if (inputVector.x < 0 && transform.position.x  > -_swipeDistance)
                {
                    _distance.x -= _swipeDistance;
                    _finalValueOfPosition = (int)transform.position.x + (int)_distance.x;
                    MoveDirectionTo(_distance);
                }
            }
            else
            {
                MoveDirectionTo(_distance);
            }
            
        }

        private void MoveDirectionTo(Vector3 position)
        {
            _capsuleRigidbody.MovePosition(transform.position + (position * _speed * Time.fixedDeltaTime));
            if (_finalValueOfPosition > transform.position.x && position.x == _swipeDistance)
            {
                _waitingForReachPoint = false;
            }
            else if (_finalValueOfPosition < transform.position.x && position.x == -_swipeDistance)
            {
                _waitingForReachPoint = false;
            }
            else _waitingForReachPoint = true;
        }
        
        private void OnEnable()
        {
            _mainMenuHandler.onGameStarted += StartMove;
            _collisionDetector.onObstacleDetected += StopMove;
            _reviveManager.onAdToReviveCompleted += StartMove;
        }

        private void OnDisable()
        {
            _mainMenuHandler.onGameStarted -= StartMove;
            _collisionDetector.onObstacleDetected -= StopMove;
            _reviveManager.onAdToReviveCompleted -= StartMove;
        }

    }
    
}
