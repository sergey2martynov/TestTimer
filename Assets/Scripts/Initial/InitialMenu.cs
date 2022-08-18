using System;
using System.Collections.Generic;
using Buttons;
using DG.Tweening;
using UnityEngine;

namespace Initial
{
    public class InitialMenu : MonoBehaviour
    {
        [SerializeField] private List<TimerButtonView> _timerButtons;
        [SerializeField] private TimerButtonView _newTimerButton;
        [SerializeField] private Transform _panel;
        [SerializeField] private int _step;
        [SerializeField] private float _showStepDuration;
        [SerializeField] private float _hideStepDuration;
        [SerializeField] private float _hideDuration;
        [SerializeField] private int _defaultPosition;
        [SerializeField] private int _maxButtonsAmount;
        [SerializeField] private float _delayTimerDisplay;
        public List<TimerButtonView> TimerButtons => _timerButtons;
        public Transform Panel => _panel;

        public event Action<bool> NewTimerButtonClicked;
        public event Action<int> TimerSelected;
        public event Action Destroyed;

        private void Start()
        {
            ShowButton();

            _newTimerButton.ButtonClicked += NewTimerButtonClick;

            foreach (var button in _timerButtons)
            {
                button.ButtonClicked += OnButtonClick;
            }
        }

        private void OnButtonClick(TimerButtonView timerButtonView)
        {
            timerButtonView.Image.color = Color.white;

            for (int i = 0; i < _timerButtons.Count; i++)
            {
                if (timerButtonView == _timerButtons[i])
                {
                    var index = i;
                    HideButton();

                    DOTween.Sequence().AppendInterval(_delayTimerDisplay).OnComplete(() =>
                    {
                        TimerSelected?.Invoke(index);
                    });
                }
            }
        }

        public void ShowButton()
        {
            var duration = _showStepDuration;

            foreach (var button in _timerButtons)
            {
                button.GetComponent<RectTransform>().DOAnchorPosX(0, duration);
                duration += _showStepDuration;
            }
        }

        private void HideButton()
        {
            var duration = _hideStepDuration;

            foreach (var button in _timerButtons)
            {
                DOTween.Sequence().AppendInterval(duration).OnComplete(() =>
                {
                    button.GetComponent<RectTransform>().DOAnchorPosX(_defaultPosition, _hideDuration);
                });

                duration += _hideStepDuration;
            }
        }

        private void NewTimerButtonClick(TimerButtonView timerButtonView)
        {
            if (_timerButtons.Count < _maxButtonsAmount)
            {
                NewTimerButtonClicked?.Invoke(false);
            }
        }

        public Vector3 GetNewPosition()
        {
            return _timerButtons[_timerButtons.Count - 1].RectTransform.localPosition +
                   new Vector3(_timerButtons[_timerButtons.Count - 1].RectTransform.localPosition.x, -_step,
                       _timerButtons[_timerButtons.Count - 1].RectTransform.localPosition.z);
        }

        public void InitializeNewButton(TimerButtonView timerButtonView)
        {
            timerButtonView.ButtonClicked += OnButtonClick;
            _timerButtons.Add(timerButtonView);
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();

            _newTimerButton.ButtonClicked -= NewTimerButtonClick;

            foreach (var button in _timerButtons)
            {
                button.ButtonClicked -= OnButtonClick;
            }
        }
    }
}