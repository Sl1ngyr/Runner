using Services;
using UnityEngine;

namespace Player
{
    public class EndGameHandler : MonoBehaviour
    {

        private void EndDeathAnimation()
        {
            EventBus.Instance.onPlayerDead?.Invoke();
        }
    }
}