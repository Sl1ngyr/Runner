using System;
using UnityEngine;

namespace Player
{
    public class EndGameHandler : MonoBehaviour
    {
        public event Action onEndGameTriggered;
        
        private void EndDeathAnimation()
        {
            onEndGameTriggered?.Invoke();
        }
    }
}