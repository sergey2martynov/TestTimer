using Buttons;
using Starter;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Initial
{
    public class ButtonSpawner
    {
        private TimerButtonView _timerButtonView;
        private InitialMenu _initialMenu;
        private ProjectStarter _projectStarter;
        private int _numberTimer = 4;

        public ButtonSpawner(TimerButtonView timewButtonView, InitialMenu initialMenu, ProjectStarter projectStarter)
        {
            _timerButtonView = timewButtonView;
            _initialMenu = initialMenu;
            _projectStarter = projectStarter;
        }

        public void Initialize()
        {
            _initialMenu.NewTimerButtonClicked += SpawnButton;
            _initialMenu.Destroyed += OnDestroy;
        }

        public void SpawnButton(bool isOnStart)
        {
            var button = Object.Instantiate(_timerButtonView, Vector3.zero, Quaternion.identity,
                _initialMenu.Panel);

            var newPosition = _initialMenu.GetNewPosition();

            if (isOnStart)
                button.RectTransform.localPosition =
                    new Vector3(newPosition.x, newPosition.y, 0);
            else
            {
                button.RectTransform.localPosition = new Vector3(0, newPosition.y, 0);
                _projectStarter.IncreaseTimerAmount();
            }

            button.ButtonText.text = "Timer" + _numberTimer;
            _numberTimer++;


            _projectStarter.CreateTimer();
            _initialMenu.InitializeNewButton(button);
        }

        private void OnDestroy()
        {
            _initialMenu.NewTimerButtonClicked -= SpawnButton;
            _initialMenu.Destroyed -= OnDestroy;
        }
    }
}