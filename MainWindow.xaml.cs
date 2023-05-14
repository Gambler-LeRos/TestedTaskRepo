using System.Windows;
using System.Windows.Controls;

namespace TestedTask
{    
    public partial class MainWindow : Window
    {    
        public MainWindow()
        {
            InitializeComponent();           
            DataContext = new RequestViewModel();

        }
     
    }
}
