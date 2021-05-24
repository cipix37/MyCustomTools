using System.Windows;
using System.Timers;
using System;
using System.Windows.Threading;

namespace MyCustomTools1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* issues:
         * image stretch - default is ok
         * property binding
         * sesize text
         * cell size as percentage - Width="2*" - 20% and/or such
         */

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
