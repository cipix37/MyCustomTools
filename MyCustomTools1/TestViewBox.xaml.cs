using System.Windows;

namespace MyCustomTools1
{
    /// <summary>
    /// Interaction logic for TestViewBox.xaml
    /// </summary>
    public partial class TestViewBox : Window
    {
        /* scroll not working in viewbox
         * tb in vb - se lungeste si micsoreaza
         * grid in vb - la fel
         * 
         * vb stretch:
         * - none - vb larger than col/row when too much text
         * - uniform - starts big, becomes infinitely small with too much text
         * - fill - starts big and becomes small separately in each direction
         * - uniform to fill - starts big and becomes larger than row/col with too much text
         * 
         * vb needs to resize itself to resize content
         */
        public TestViewBox()
        {
            InitializeComponent();
        }
    }
}
