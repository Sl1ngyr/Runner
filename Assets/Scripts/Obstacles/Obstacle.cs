using UnityEngine;

namespace Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float _endPositionZToDeactivate;
        
        private float _speed;
        
        private static bool _isStopMoveObject = false;

        public static bool StopMoveObstacles
        {
            get => _isStopMoveObject;
            set => _isStopMoveObject = value;
        }
        
        public float Speed
        {
            set => _speed = value;
        }

        private void Update()
        {
            if (!_isStopMoveObject)
            {
                Move();
            }
        }

        private void StopMoving()
        {
            _isStopMoveObject = true;
        }

        private void StartMoving()
        {
            _isStopMoveObject = false;
        }
        
        private void Move()
        {
            if (gameObject.transform.position.z <= _endPositionZToDeactivate)
            {
                Deactivate();
            }
            transform.Translate(Vector3.back * Time.deltaTime * _speed);
        }

        private void Deactivate()
        {
            this.gameObject.SetActive(false);
        }

    }
}