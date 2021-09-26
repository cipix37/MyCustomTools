using System;
using System.Windows;

namespace MyCustomTools1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestSerialization tb = new TestSerialization();
            tb.Show();
        }
    }
}
