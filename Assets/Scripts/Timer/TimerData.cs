namespace Timer
{
    public class TimerData
    {
        private float _seconds;

        public float Seconds
        {
            get => _seconds;
            set
            {
                if (value < 0)
                    _seconds = 0;
                
                _seconds = value;
            }
        }
    }
}