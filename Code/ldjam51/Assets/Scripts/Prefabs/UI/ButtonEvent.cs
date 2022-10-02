using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Prefabs.UI
{
    public class ButtonEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent OnPressed;

        bool isPressed = false;
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
        }

        void Update()
        {
            if (isPressed)
            {
                OnPressed?.Invoke();
            }
        }
    }
}
