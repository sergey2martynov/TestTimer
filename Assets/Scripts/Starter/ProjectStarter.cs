using System;
using System.Collections.Generic;
using Buttons;
using Initial;
using Timer;
using UnityEngine;
using Updater;

namespace Starter
{
    public class ProjectStarter : MonoBehaviour
    {
        [SerializeField] private TimerView _timerView;
        [SerializeField] private InitialMenu _initialMenu;
        [SerializeField] private TimerUpdater _timerUpdater;
        [SerializeField] private TimerButtonView _timerButtonView;
        [SerializeField] private int _timerAmount;
        private int _newTimerAmount;
        private InitialMenuLogic _initialMenuLogic;

        private List<TimerData> _timersData = new List<TimerData>();

        private List<TimerLogic> _timerLogics = new List<TimerLogic>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            int count = 0;
            int newButtonCount = 0;

            if (PlayerPrefs.HasKey("Seconds" + count))
            {
                _newTimerAmount = PlayerPrefs.GetInt("NewTimerAmount");

                while (PlayerPrefs.HasKey("Seconds" + count))
                {
                    var timerData = new TimerData();
                    timerData.Seconds = PlayerPrefs.GetFloat("Seconds" + count);
                    _timersData.Add(timerData);
                    var timerLogic = new TimerLogic(_timerView, timerData, _timerUpdater);
                    timerLogic.Initialize();
                    _timerLogics.Add(timerLogic);
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < _timerAmount; i++)
                {
                    var timerData = new TimerData();
                    var timerLogic = new TimerLogic(_timerView, timerData, _timerUpdater);
                    timerLogic.Initialize();
                    _timerLogics.Add(timerLogic);
                    _timersData.Add(timerData);
                }
            }

            _initialMenuLogic = new InitialMenuLogic(_timerLogics, _initialMenu, _timerView);
            _initialMenuLogic.Initialize();

            var buttonSpawner = new ButtonSpawner(_timerButtonView, _initialMenu, this);
            buttonSpawner.Initialize();

            for (int i = 0; i < _newTimerAmount; i++)
            {
                buttonSpawner.SpawnButton(true);
            }
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            SaveData();
        }

        public void CreateTimer()
        {
            var timerData = new TimerData();
            var timerLogic = new TimerLogic(_timerView, timerData, _timerUpdater);
            timerLogic.Initialize();
            _timerLogics.Add(timerLogic);
            _timersData.Add(timerData);
            _initialMenuLogic.SubscribeToNewLogic(timerLogic);
        }

        public void IncreaseTimerAmount()
        {
            _newTimerAmount++;
        }

        private void SaveData()
        {
            int count = 0;
            foreach (var timerData in _timersData)
            {
                PlayerPrefs.SetFloat("Seconds" + count, timerData.Seconds);
                count++;
            }

            PlayerPrefs.SetInt("NewTimerAmount", _newTimerAmount);

            PlayerPrefs.Save();
        }
    }
}