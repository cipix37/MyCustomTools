
using System.Timers;

namespace MyCustomTools1
{
    //Dispatcher timer merge fara invoke dar foarte prost/incet
    //timer(default different thread) - pt sah
    //sau stopwatch(default same thread) - pt cub

    public class ChessTimer
    {
        private readonly int DefaultHours, DefaultMinutes, DefaultSeconds; // DefaultSecondsPer10=0
        private int Hours, Minutes, Seconds, SecondsPer10;

        public ChessTimer(int hours, int minutes, int seconds) //can handle overflow
        {
            DefaultSeconds = seconds % 60;
            minutes += seconds / 60;
            DefaultMinutes = minutes % 60;
            DefaultHours = hours + minutes / 60;
            Reset();
        }

        /// <summary>
        /// Actualizes Hours, Minutes, Seconds, SecondsPer10
        /// </summary>
        public void Tick()
        {
            SecondsPer10--;
            if (SecondsPer10 < 0)
            {
                SecondsPer10 = 9;
                Seconds--;
            }
            if (Seconds < 0)
            {
                Seconds = 59;
                Minutes--;
            }
            if (Minutes < 0)
            {
                Minutes = 59;
                Hours--;
            }
        }

        public void Set(int hours, int minutes, int seconds) //can handle overflow
        {
            SecondsPer10 = 0;
            Seconds = seconds % 60;
            minutes += seconds / 60;
            Minutes = minutes % 60;
            Hours = hours + minutes / 60;
        }

        public void Reset()
        {
            Hours = DefaultHours;
            Minutes = DefaultMinutes;
            Seconds = DefaultSeconds;
            SecondsPer10 = 0;
        }

        public void Add(int hours, int minutes, int seconds) //can handle overflow
        {
            Seconds += seconds;
            Minutes += minutes + Seconds / 60;
            Hours += hours + Minutes / 60;
            Seconds %= 60;
            Minutes %= 60;
        }

        public string PrintDefaultValues()
        {
            return $"{DefaultHours}:{DefaultMinutes}:{DefaultSeconds}  ";
        }

        public override string ToString()
        {
            return $"{Hours}:{Minutes}:{Seconds},{SecondsPer10}";
        }
    }

    public class ChessClock
    {
        private readonly Timer MyTimer;
        private readonly ChessTimer WhiteTimer, BlackTimer;
        private bool CurrentPlayer;
        public ChessTimer Clock(bool player)
        {
            if (player) return WhiteTimer;
            return BlackTimer;
        }

        public ChessTimer ActiveTimer
        {
            get
            {
                if (CurrentPlayer) return WhiteTimer;
                return BlackTimer;
            }
        }

        public ChessClock()
        {
            CurrentPlayer = true;
            MyTimer = new Timer(100);
            MyTimer.Elapsed += new ElapsedEventHandler(Tick);
            WhiteTimer = new ChessTimer(1, 5, 0);
            BlackTimer = new ChessTimer(1, 5, 0);
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            if (CurrentPlayer) WhiteTimer.Tick();
            else BlackTimer.Tick();
        }
    }

    /*Dispatcher.Invoke(new Action(() =>
    {
    ticks++;
    Tb1.Text = ticks.ToString();
    }));*/
}
