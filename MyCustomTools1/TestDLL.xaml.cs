using System.Windows;
using System.Runtime.InteropServices;

namespace MyCustomTools1
{
    /// <summary>
    /// Interaction logic for TestDLL.xaml
    /// </summary>
    public partial class TestDLL : Window
    {
        [DllImport("MyTestDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Function1(int a);

        [DllImport("MyTestDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Function2(int[,] t);

        public TestDLL()
        {
            InitializeComponent();

            tb.Text += Function1(5) + "\n";

            int[,] sbArr1 = { { 1, 2 }, { 3, 4 } };
            Function2(sbArr1);
            tb.Text += sbArr1[0, 0].ToString() + "\n";
        }
    }
}
