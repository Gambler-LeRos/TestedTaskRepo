using System.Windows;

namespace TestedTask
{
    public partial class MainWindow : Window
    {
        private RequestViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = new RequestViewModel();
            DataContext = _vm;
            _vm.OpenInfoWindowEvent += Vm_OpenInfoWindowEvent;
        }

        private void Vm_OpenInfoWindowEvent(Request item)
        {
            new InfoWindow(_vm).ShowDialog();           
            _vm.DialogClosed();
        }
        
    }
}
