using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainMenu
{
    public class MainMenuHandler : MonoBehaviour, IPointerDownHandler
    {

        public event Action onStateMenuChanged;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            onStateMenuChanged?.Invoke();
        }
    }
}