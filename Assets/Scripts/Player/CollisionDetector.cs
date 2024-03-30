using Services;
using UnityEngine;

namespace Player
{
    public class CollisionDetector : MonoBehaviour
    {

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == "Obstacle")
            {
                EventBus.Instance.onPlayerCollideWithObstacle?.Invoke();
            }
        }
    }
}