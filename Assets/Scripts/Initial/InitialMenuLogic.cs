using System.Collections.Generic;
using Timer;
using UnityEngine;

namespace Initial
{
    public class InitialMenuLogic
    {
        private InitialMenu _initialMenu;
        private TimerView _timerView;
        private List<TimerLogic> _timerLogics;


        public InitialMenuLogic(List<TimerLogic> timerLogics, InitialMenu initialMenu, TimerView timerView)
        {
            _timerLogics = timerLogics;
            _initialMenu = initialMenu;
            _timerView = timerView;
        }

        public void Initialize()
        {
            _initialMenu.TimerSelected += ActivateTimer;
            _initialMenu.Destroyed += OnDestroy;

            foreach (var timer in _timerLogics)
            {
                timer.TimerStarted += ActiveSelf;
                timer.TimerCompleted += ChangeColorButton;
            }
        }

        public void SubscribeToNewLogic(TimerLogic timerLogic)
        {
            timerLogic.TimerCompleted += ChangeColorButton;
            timerLogic.TimerStarted += ActiveSelf;
        }

        private void ActivateTimer(int index)
        {
            for (int i = 0; i < _timerLogics.Count; i++)
            {
                if (i == index)
                {
                    _timerLogics[i].Activate();
                    _timerView.gameObject.SetActive(true);
                    _initialMenu.gameObject.SetActive(false);
                    _timerView.ShowButtons();
                }
            }
        }

        private void ActiveSelf()
        {
            _timerView.gameObject.SetActive(false);
            _initialMenu.gameObject.SetActive(true);
            _initialMenu.ShowButton();
        }

        private void ChangeColorButton(TimerLogic timerLogic)
        {
            for (int i = 0; i < _timerLogics.Count; i++)
            {
                if (_timerLogics[i] == timerLogic)
                {
                    _initialMenu.TimerButtons[i].Image.color = Color.yellow;
                }
            }
        }

        private void OnDestroy()
        {
            _initialMenu.TimerSelected -= ActivateTimer;

            foreach (var timer in _timerLogics)
            {
                timer.TimerStarted -= ActiveSelf;
                timer.TimerCompleted -= ChangeColorButton;
            }
        }
    }
}