using System;
using UnityEngine;

namespace Player
{
    public class CollisionDetector : MonoBehaviour
    {
        public event Action onObstacleDetected;

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == "Obstacle")
            {
                onObstacleDetected?.Invoke();
            }
        }
    }
}