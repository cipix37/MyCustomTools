using System;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyCustomTools1
{
    //Dispatcher timer merge fara invoke dar foarte prost/incet
    //timer(default different thread) - pt sah
    //sau stopwatch(default same thread) - pt cub

    public class PlayerTimer : TextBox
    {
        private TimeSpan Time, Increment, Interval, RedTextThreshold;
        private readonly bool CountDown;

        /// <summary>
        /// Method for extracting actual time from the string input
        /// </summary>
        private void SpecialParse(object sender, RoutedEventArgs e)
        {
            // keep only specified characters
            string FirstResult = System.Text.RegularExpressions.Regex.Replace(Text, "[^0-9:,+]", "");
            // split string into time and increment
            string[] SecondResult = FirstResult.Split('+'); // 0-Time 1-Increment
            string Errors = "";
            if (SecondResult.Length > 2) Errors = "Too many '+' characters in time format.";
            else if (SecondResult.Length == 0) Errors = "No string found in time format.";
            else
            {
                if (SecondResult.Length == 1)
                {
                    Increment = new TimeSpan();
                }
                else
                {
                    // extract increment from Firstresult[1];
                }
                // extract time from Firstresult[0];
            }
            MessageBox.Show(Errors);
        }

        /// <summary>
        /// Sets the TextBox text value to this.ToString(), but asynchronously
        /// </summary>
        private void SetTextAsync()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (Time < RedTextThreshold) Foreground = Brushes.Red;
                else Foreground = Brushes.Black;
                Text = ToString();
            }));
        }

        /// <summary>
        /// Instantiate a new instance of the PlayerTimer class, specifying the count direction
        /// </summary>
        public PlayerTimer(bool countDown)
        {
            Time = new TimeSpan();
            Increment = new TimeSpan();
            Interval = new TimeSpan(0, 0, 0, 0, 100);
            RedTextThreshold = new TimeSpan(0, 0, 30);
            CountDown = countDown;


            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;
            FontWeight = FontWeights.Bold;
            LostFocus += new RoutedEventHandler(SpecialParse);
        }

        /// <summary>
        /// Returns true if the time is zero (or lower, for certainty)
        /// </summary>
        public bool IsZero
        {
            get
            {
                return Time.TotalMilliseconds < 0;
            }
        }

        /// <summary>
        /// Returns the current number of ticks, necesarry for AI algorithm
        /// </summary>
        public int CurrentNumberOfTicks
        {
            get
            {
                return Convert.ToInt32(Time.TotalMilliseconds / Interval.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Actualizes Time with the value of one Interval
        /// </summary>
        public void Tick()
        {
            Time.Add(CountDown ? -Interval : Interval);
            SetTextAsync();
        }

        /// <summary>
        /// Sets the values for Time and Increment
        /// </summary>
        public void Set(TimeSpan time, TimeSpan increment)
        {
            Time = time;
            Increment = increment;
            SetTextAsync();
        }

        /// <summary>
        /// Increases internal time by increment.
        /// </summary>
        public void AddIncrement()
        {
            Time.Add(Increment);
            SetTextAsync();
        }

        /// <summary>
        /// Returns Time and Increment in clasic string format
        /// </summary>
        public override string ToString()
        {
            string ToStringHelper(TimeSpan time)
            {
                string result = "";
                if (time.Hours > 0) 
                    result += time.Hours.ToString("D2") + ":";
                if (time.Hours > 0 || time.Minutes > 0) 
                    result += time.Minutes.ToString("D2") + ":";
                if (time.Hours > 0 || time.Minutes > 0 || time.Seconds > 0)
                    result += time.Seconds.ToString("D2") + "," + (time.Milliseconds / 100).ToString();
                return result;
            }
            string increment = ToStringHelper(Increment);
            if (increment == "") return ToStringHelper(Time);
            return ToStringHelper(Time) + "+" + increment;
        }
    }

    public class TimeControl
    {
        private readonly Timer MyTimer;
        private readonly PlayerTimer WhiteTimer, BlackTimer;
        private bool CurrentPlayer;
        public PlayerTimer Clock(bool player)
        {
            if (player) return WhiteTimer;
            return BlackTimer;
        }

        public PlayerTimer ActiveTimer
        {
            get
            {
                if (CurrentPlayer) return WhiteTimer;
                return BlackTimer;
            }
        }

        public TimeControl()
        {
            CurrentPlayer = true;
            MyTimer = new Timer(100);
            MyTimer.Elapsed += new ElapsedEventHandler(Tick);
            WhiteTimer = new PlayerTimer(true);
            BlackTimer = new PlayerTimer(true);
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
