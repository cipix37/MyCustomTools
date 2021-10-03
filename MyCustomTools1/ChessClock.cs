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
                    Increment = TimeSpan.Zero;
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
            Time = TimeSpan.Zero;
            Increment = TimeSpan.Zero;
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

    public class TimeControl : Grid
    {
        private const int ButtonSize = 200, TextBoxSize = 140, FreeSpaceSize = 25, GapSize = 5, fontSize = 20, RowHeight = 35;
        private readonly Timer MyTimer;
        private readonly PlayerTimer WhiteTimer, BlackTimer, WhiteMoveTimer, BlackMoveTimer;
        private readonly ComboBox IncrementType; // start and stop are managed by changing the main tabular menu selection

        private string PlayerString(bool player)
        {
            return player ? "White" : "Black";
        }
        private PlayerTimer GetTimer(bool player)
        {
            return player ? WhiteTimer : BlackTimer;
        }
        private PlayerTimer GetMoveTimer(bool player)
        {
            return player ? WhiteMoveTimer : BlackMoveTimer;
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            PlayerTimer playerTimer = GetTimer(CurrentPlayer), currentMoveTimer = GetMoveTimer(CurrentPlayer);
            playerTimer.Tick();
            currentMoveTimer.Tick();
            if (playerTimer.IsZero)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    PauseTimer();
                    MessageBox.Show(PlayerString(CurrentPlayer) + "'s time finished!\n" +
                        "Winner is " + PlayerString(!CurrentPlayer) + "!");
                }));
            }
        }
        private void InitializeGrid()
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(RowHeight, GridUnitType.Pixel);
            RowDefinitions.Add(rowDefinition);

            rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(RowHeight, GridUnitType.Pixel);
            RowDefinitions.Add(rowDefinition);

            ColumnDefinition columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(FreeSpaceSize, GridUnitType.Pixel);
            ColumnDefinitions.Add(columnDefinition);

            columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(TextBoxSize, GridUnitType.Pixel);
            ColumnDefinitions.Add(columnDefinition);

            columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(GapSize, GridUnitType.Pixel);
            ColumnDefinitions.Add(columnDefinition);

            columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(ButtonSize, GridUnitType.Pixel);
            ColumnDefinitions.Add(columnDefinition);

            columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(GapSize, GridUnitType.Pixel);
            ColumnDefinitions.Add(columnDefinition);

            columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(TextBoxSize, GridUnitType.Pixel);
            ColumnDefinitions.Add(columnDefinition);

            columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(FreeSpaceSize, GridUnitType.Pixel);
            ColumnDefinitions.Add(columnDefinition);
        }

        public double FontSize
        {
            get
            {
                return WhiteTimer.FontSize;
            }
            set
            {
                foreach (FrameworkElement element in Children)
                {
                    if (element is Control)
                    {
                        (element as Control).FontSize = value;
                    }
                }
            }
        }
        public bool CurrentPlayer { get; private set; }
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

            InitializeGrid();
            Background = new SolidColorBrush(Color.FromArgb(0xff, 0xef, 0xef, 0xef));

            WhiteTimer = new PlayerTimer(true);
            WhiteTimer.Margin = new Thickness(0, 5, 0, 0);
            SetColumn(WhiteTimer, 1);
            SetRow(WhiteTimer, 0);
            Children.Add(WhiteTimer);
            WhiteTimer.Set(new TimeSpan(0, 5, 0), TimeSpan.Zero);

            IncrementType = new ComboBox
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 15, 0, 15)
            };

            string[] increments = { "Fisher Increment", "Simple Delay", "Bronstein Delay" };
            foreach (string s in increments)
            {
                IncrementType.Items.Add(s);
            }
            IncrementType.SelectedIndex = 0;
            SetColumn(IncrementType, 3);
            SetRow(IncrementType, 0);
            SetRowSpan(IncrementType, 2);
            Children.Add(IncrementType);

            BlackTimer = new PlayerTimer(true);
            BlackTimer.Margin = new Thickness(0, 5, 0, 0);
            SetColumn(BlackTimer, 5);
            SetRow(BlackTimer, 0);
            Children.Add(BlackTimer);
            BlackTimer.Set(new TimeSpan(0, 5, 0), TimeSpan.Zero);

            WhiteMoveTimer = new PlayerTimer(false);
            WhiteMoveTimer.Margin = new Thickness(0, 5, 0, 0);
            SetColumn(WhiteMoveTimer, 1);
            SetRow(WhiteMoveTimer, 1);
            Children.Add(WhiteMoveTimer);
            WhiteMoveTimer.Set(TimeSpan.Zero, TimeSpan.Zero);

            BlackMoveTimer = new PlayerTimer(false);
            BlackMoveTimer.Margin = new Thickness(0, 5, 0, 0);
            SetColumn(BlackMoveTimer, 5);
            SetRow(BlackMoveTimer, 1);
            Children.Add(BlackMoveTimer);
            BlackMoveTimer.Set(TimeSpan.Zero, TimeSpan.Zero);

            FontSize = fontSize;
        }

        public void ChangeCurrentPlayer()
        {
            GetTimer(CurrentPlayer).AddIncrement();
            CurrentPlayer = !CurrentPlayer;
            GetMoveTimer(CurrentPlayer).Set(TimeSpan.Zero, TimeSpan.Zero);
        }

        public void StartTimer()
        {
            string errors = "";
            if (WhiteTimer.IsZero)
            {
                errors += "Cannot start because White player has invalid time!";
            }
            if (BlackTimer.IsZero)
            {
                errors += "Cannot start because Black player has invalid time!";
            }
            if (errors == "")
            {
                MyTimer.Start();
                IncrementType.IsEnabled = false;
                WhiteTimer.IsReadOnly = true;
                BlackTimer.IsReadOnly = true;
            }
            else
            {
                MessageBox.Show(errors);
            }
        }

        public void PauseTimer()
        {
            MyTimer.Stop();
            IncrementType.IsEnabled = true;
            WhiteTimer.IsReadOnly = false;
            BlackTimer.IsReadOnly = false;
        }
    }

    /*Dispatcher.Invoke(new Action(() =>
    {
    ticks++;
    Tb1.Text = ticks.ToString();
    }));*/
}
