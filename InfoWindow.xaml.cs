using System.Linq;
using System.Windows;

namespace TestedTask
{
    /// <summary>
    /// Окно информации для добавления, редактирования и отмены заявки
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow(int idRequest)
        {
            InitializeComponent();
            Request item = new();
            using DataBaseContext db = new();
            {
                item = db.Requests.Where(w => w.IdRequest == idRequest).FirstOrDefault() ?? new Request { RequestNextStep = "Создать", RequestStatus = "Создать" };
            }

            DataContext = item;
        }

        /// <summary>
        /// Изменение статуса и настроек отображения/изменения заявки/Создание записи новой заявки
        /// </summary>
        private void ChangedStatus(object sender, RoutedEventArgs e)
        {
            Request? item = DataContext as Request;

            using DataBaseContext db = new();
            {
                if (item != null && item.IdRequest != 0) item = db.Requests.Where(w => w.IdRequest == item.IdRequest).FirstOrDefault();
                if ((item != null && item.StgRequestMove) || item?.RequestStatus == "Создать")
                {
                    item.StgRequestEdit = true;
                    switch (item.RequestStatus)
                    {
                        case "Создать": //Параметры при создании
                            item.RequestNextStep = "На выполнение";
                            item.RequestStatus = "Новая";
                            item.StgRequestCancel = true;
                            item.StgRequestEdit = false;
                            db.Requests.Add(item);
                            break;
                        case "Новая": //Параметры при передаче на выполнение
                            item.RequestStatus = "На выполнении";
                            item.RequestNextStep = "Выполнено";
                            item.StgRequestCancel = false;
                            db.Requests.Update(item);
                            break;
                        case "На выполнении": //Параметры при доставке на место назначения
                            item.RequestStatus = "Выполнено";
                            item.RequestNextStep = "Удалить";
                            item.RequestCancelText = "Доставлено по назначению";
                            db.Requests.Update(item);
                            break;
                        case "Отмена" or "Выполнено": //Параметры при удалении
                            item.RequestNextStep = string.Empty;
                            item.StgRequestMove = false;
                            item.RequestStatus = "Удалено";
                            db.Requests.Update(item);
                            break;
                    }
                    db.SaveChanges();
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Отмена заявки
        /// </summary>
        private void CancelRequest(object sender, RoutedEventArgs e)
        {
            Request? item = DataContext as Request;

            using DataBaseContext db = new();
            {
                if (item != null && !string.IsNullOrEmpty(item.RequestCancelText) && db.Requests.Where(w => w.IdRequest == item.IdRequest).Select(s => s.StgRequestCancel).First())
                {
                    item.StgRequestEdit = true;
                    item.RequestStatus = "Отмена";
                    item.RequestNextStep = "Удалить";
                    item.StgRequestCancel = false;

                    db.Requests.Update(item);
                    db.SaveChanges();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Укажите причину отмены");
                }
            }
        }

        /// <summary>
        /// Сохранение изменений в заявке
        /// </summary>
        private void SaveRequest(object sender, RoutedEventArgs e)
        {
            Request? item = DataContext as Request;

            using DataBaseContext db = new();
            if (item != null && db.Requests.Where(w => w.IdRequest == item.IdRequest).Select(s => s.StgRequestCancel).First())
            {
                db.Requests.Update(item);
                db.SaveChanges();
                this.Close();

                MessageBox.Show("Изменения сохранены");
            }
            else
            {
                MessageBox.Show("Сохранение не возможно");
            }
        }
    }
}
