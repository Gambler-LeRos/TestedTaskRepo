using System.Windows;

namespace TestedTask
{
    /// <summary>
    /// Окно информации для добавления, редактирования и отмены заявки
    /// </summary>
    public partial class InfoWindow : Window
    {      
        public InfoWindow(RequestViewModel request)
        {
            InitializeComponent();
            DataContext = request;
        }
    }
}
