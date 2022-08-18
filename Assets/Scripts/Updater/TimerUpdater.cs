using System;
using UnityEngine;

namespace Updater
{
    public class TimerUpdater : MonoBehaviour
    {
        public event Action Updated;

        private void Update()
        {
            Updated?.Invoke();
        }
    }
}