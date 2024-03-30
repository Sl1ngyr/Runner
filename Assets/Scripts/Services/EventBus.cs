using System;

namespace Services
{
    public class EventBus
    {

        private static EventBus _instance;

        public static EventBus Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventBus();
                
                return _instance;
            }
        }
        
        
        public Action onGameStarted;

        public Action onPlayerCollideWithObstacle;

        public Action onPlayerDead;

        public Action onAdToReviveCompleted;
    }
}