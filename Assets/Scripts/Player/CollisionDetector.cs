using Obstacles;
using Services;
using UnityEngine;

namespace Player
{
    public class CollisionDetector : MonoBehaviour
    {

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.TryGetComponent(out Obstacle obstacle))
            {
                EventBus.Instance.onPlayerCollideWithObstacle?.Invoke();
            }
        }
    }
}