using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Buttons
{
    public class ButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler
    {
        [SerializeField] private UnityEngine.UI.Button _button;
        [SerializeField] private ButtonType _buttonType;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _tickDuration;

        private float _elapsedTime;
        private bool _isPointerDown;

        public RectTransform RectTransform => _rectTransform;

        public event Action<ButtonType> ButtonClicked;
        public event Action<ButtonType> ButtonHold;
        public event Action<ButtonType> ButtonRelease;

        private void Update()
        {
            if (_isPointerDown)
            {
                _elapsedTime += Time.deltaTime;
            
                if (_elapsedTime > _tickDuration)
                {
                    ButtonHold?.Invoke(_buttonType);
                    _elapsedTime = 0;
                }
            }
            
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerDown = true;
            ButtonHold?.Invoke(_buttonType);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPointerDown = false;
            ButtonRelease?.Invoke(_buttonType);
            _elapsedTime = 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ButtonClicked?.Invoke(_buttonType);
        }
    }
}