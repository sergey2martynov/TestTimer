using System;
using System.Collections.Generic;
using Buttons;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Timer
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private List<ButtonView> _buttons;
        [SerializeField] private TextMeshProUGUI _hours;
        [SerializeField] private TextMeshProUGUI _minutes;
        [SerializeField] private TextMeshProUGUI _seconds;
        [SerializeField] private Vector3 _showScale;
        [SerializeField] private Vector3 _hideScale;
        [SerializeField] private float _showDuration;

        public TextMeshProUGUI Hours => _hours;
        public TextMeshProUGUI Minutes => _minutes;
        public TextMeshProUGUI Seconds => _seconds;
        public float ShowDuration => _showDuration;

        public event Action<ButtonType> ButtonClicked;
        public event Action<ButtonType> ButtonHold;
        public event Action<ButtonType> ButtonRelease;

        private void Start()
        {
            foreach (var button in _buttons)
            {
                button.ButtonClicked += OnButtonClick;
                button.ButtonHold += OnButtonHold;
                button.ButtonRelease += OnButtonRelease;
            }
        }

        private void OnButtonClick(ButtonType buttonType)
        {
            ButtonClicked?.Invoke(buttonType);
        }

        private void OnButtonHold(ButtonType buttonType)
        {
            ButtonHold?.Invoke(buttonType);
        }

        private void OnButtonRelease(ButtonType buttonType)
        {
            ButtonRelease?.Invoke(buttonType);
        }

        public void ShowButtons()
        {
            foreach (var button in _buttons)
            {
                button.RectTransform.localScale = new Vector3(0, 0, 0);
                button.RectTransform.DOScale(_showScale, _showDuration);
            }
        }

        public void HideButtons()
        {
            foreach (var button in _buttons)
            {
                button.RectTransform.DOScale(_hideScale, _showDuration);
            }
        }
    }
}