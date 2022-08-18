using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Buttons
{
    public class TimerButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _buttonText;
        public Image Image => _image;
        public RectTransform RectTransform => _rectTransform;
        public TextMeshProUGUI ButtonText => _buttonText;

        public event Action<TimerButtonView> ButtonClicked;

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            ButtonClicked?.Invoke(this);
        }
    }
}