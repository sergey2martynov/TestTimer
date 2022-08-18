using System;
using DG.Tweening;
using UnityEngine;
using Updater;

namespace Timer
{
    public class TimerLogic
    {
        private TimerView _timerView;
        private TimerData _timerData;
        private TimerUpdater _timerUpdater;
        private float _elapsedTime = 1;
        private bool _isCountdown;
        private bool _isActive;
        private float _addedValue;
        private float _durationTick = 0.5f;
        private int _minSecondAmount = 5;

        public event Action TimerStarted;
        public event Action<TimerLogic> TimerCompleted;

        public TimerLogic(TimerView timerView, TimerData timerData, TimerUpdater timerUpdater)
        {
            _timerView = timerView;
            _timerData = timerData;
            _timerUpdater = timerUpdater;
        }

        public void Initialize()
        {
            _timerView.ButtonHold += IncreaseTimer;
            _timerView.ButtonHold += DecreaseTimer;
            _timerView.ButtonClicked += StartTimer;
            _timerView.ButtonRelease += StopIncrease;
            _timerUpdater.Updated += OnUpdate;

            if (_timerData.Seconds > 0)
            {
                _isCountdown = true;
            }
        }

        private void SetTime()
        {
            TimeSpan time = TimeSpan.FromSeconds(_timerData.Seconds);

            if (time.Hours < 10)
                _timerView.Hours.text = "0" + time.Hours + ":";
            else
                _timerView.Hours.text = time.Hours + ":";

            if (time.Minutes < 10)
                _timerView.Minutes.text = "0" + time.Minutes + ":";
            else
                _timerView.Minutes.text = time.Minutes + ":";

            if (time.Seconds < 10)
                _timerView.Seconds.text = "0" + time.Seconds;
            else
                _timerView.Seconds.text = time.Seconds.ToString();
        }

        private void IncreaseTimer(ButtonType buttonType)
        {
            if (_isActive)
            {
                _elapsedTime += _durationTick;
                _addedValue += 1 * _elapsedTime;

                if (_elapsedTime < _minSecondAmount)
                {
                    _addedValue = 1;
                }

                if (buttonType == ButtonType.Plus && !_isCountdown)
                {
                    _timerData.Seconds += _addedValue;
                    SetTime();
                }
            }
        }

        private void StopIncrease(ButtonType buttonType)
        {
            if ((buttonType == ButtonType.Minus || buttonType == ButtonType.Plus) && _isActive)
            {
                _elapsedTime = 0;
                _addedValue = 0;
            }
        }

        private void DecreaseTimer(ButtonType buttonType)
        {
            if (_isActive)
            {
                _elapsedTime += _durationTick;
                _addedValue += 1 * _elapsedTime;

                if (_elapsedTime < _minSecondAmount)
                {
                    _addedValue = 1;
                }

                if (buttonType == ButtonType.Minus && !_isCountdown)
                {
                    _timerData.Seconds -= _addedValue;

                    if (_timerData.Seconds < 0)
                    {
                        _timerData.Seconds = 0;
                    }

                    SetTime();
                }
            }
        }

        private void StartTimer(ButtonType buttonType)
        {
            if (buttonType == ButtonType.Start && _isActive)
            {
                if (_timerData.Seconds == 0)
                    _isCountdown = false;
                else
                    _isCountdown = true;

                _isActive = false;
                _timerView.HideButtons();

                DOTween.Sequence().AppendInterval(_timerView.ShowDuration).OnComplete(() =>
                {
                    TimerStarted?.Invoke();
                });
            }
        }

        private void OnUpdate()
        {
            if (_isCountdown)
            {
                _timerData.Seconds -= Time.deltaTime;

                if (_timerData.Seconds <= 0)
                {
                    _timerData.Seconds = 0;
                    _isCountdown = false;
                    TimerCompleted?.Invoke(this);
                }

                if (_isActive)
                    SetTime();
            }
        }

        public void Activate()
        {
            _isActive = true;
            SetTime();
        }
    }
}