using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TestedTask
{
    public class RequestViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Переменная фильтра строки поиска
        /// </summary>
        string _filtertext = string.Empty;
        public string Filter
        {
            get => _filtertext;
            set
            {
                _filtertext = value;
                OnPropertyChanged(nameof(Requests));
            }
        }

        /// <summary>
        /// Переменная фильтра статуса заявок
        /// </summary>
        string _filterStatus = String.Empty;
        public string FilterStatus
        {
            get => _filterStatus;
            set
            {
                _filterStatus = value;
                OnPropertyChanged(nameof(Requests));
            }
        }

        /// <summary>
        /// Изменение отображаемого списка заявок по статусам
        /// </summary>
        private RelayCommand? _changedViewStatus;
        public RelayCommand ChangedViewStatus
        {
            get
            {
                return _changedViewStatus ??
                (_changedViewStatus = new RelayCommand((o) =>
                 {
                     string status = o?.ToString() ?? "Новая";
                     Filter = string.Empty;
                     FilterStatus = status;
                 }));
            }
        }    
        public Request OneItem { get; set; } = new Request(); //Переменная для создания и отмены заявки

        /// <summary>
        /// Открытие окна информации для добавления, редактирования и отмены заявки
        /// </summary>
        
        public event Action<Request> OpenInfoWindowEvent;

        private RelayCommand? _OpenInfoWindow;    
        public RelayCommand OpenInfoWindow
        {
            get
            {
                return _OpenInfoWindow ??
                (_OpenInfoWindow = new RelayCommand((o) =>
                {
                    if (o is Request) OneItem = (Request)o;
                    else OneItem = new Request { RequestNextStep = "Создать", RequestStatus = "Создать" };
                    
                    OpenInfoWindowEvent?.Invoke(OneItem);
                }));
            }
        }

        /// <summary>
        /// Фильтрация списка коллекции
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool FiltdedCheck(Request item)
        {
            bool accept = true;
            if (!string.IsNullOrEmpty(Filter))
            {
                string FindText = Filter.ToLower();
                accept = (item.RequestText.ToLower().Contains(FindText) || item.TransferStart.ToLower().Contains(FindText) || item.TransferEnd.ToLower().Contains(FindText) || item.RequestStatus.ToLower().Contains(FindText));
            }
            else if (FilterStatus != string.Empty && FilterStatus != "Все" && item.RequestStatus != FilterStatus) accept = false;

            return accept;
        }

        /// <summary>
        /// Изменение статуса и настроек отображения/изменения заявки/Создание записи новой заявки
        /// </summary>
        private RelayCommand? _changedStatus;
        public RelayCommand ChangedStatus
        {
            get
            {
                return _changedStatus ??
                (_changedStatus = new RelayCommand((obj) =>
                {
                    Request item = obj as Request;
                    if (item != null)
                    {
                        DataBaseContext db = new DataBaseContext();

                        if ((item != null && item.StgRequestMove) || item.RequestStatus == "Создать")
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
                            db.Dispose();

                            OnPropertyChanged(nameof(Requests));
                            OnPropertyChanged(nameof(OneItem));

                        }

                    }
                    else MessageBox.Show("Внесение изменений не возможно");

                }));
            }
        }

        /// <summary>
        /// Иохранение изменений в заявке
        /// </summary>

        private RelayCommand? _SaveRequest;
        public RelayCommand SaveRequest
        {
            get
            {
                return _SaveRequest ??
                (_SaveRequest = new RelayCommand((obj) =>
                {
                    try
                    {
                        Request item = obj as Request;
                        if (item != null)
                        {
                            DataBaseContext db = new DataBaseContext();

                            if (db.Requests.Where(w => w.IdRequest == OneItem.IdRequest).Select(s => s.StgRequestCancel).First())
                            {
                                db.Requests.Update(item);
                                db.SaveChanges();

                                OnPropertyChanged(nameof(Requests));
                                OnPropertyChanged(nameof(OneItem));
                                MessageBox.Show("Изменения сохранены");
                            }
                            else
                            {
                                
                                MessageBox.Show("Сохранение не возможно");
                            }

                            db.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


                }));
            }
        }


        /// <summary>
        /// Отмена заявки
        /// </summary>

        private RelayCommand? _CancelRequest;
        public RelayCommand CancelRequest
        {
            get
            {
                return _CancelRequest ??
                (_CancelRequest = new RelayCommand((obj) =>
                {
                    if (string.IsNullOrEmpty(OneItem.RequestCancelText)) MessageBox.Show("Укажите в комментарии причину отмены");
                    else
                    {
                        try
                        {
                            DataBaseContext db = new DataBaseContext();

                            if (db.Requests.Where(w => w.IdRequest == OneItem.IdRequest).Select(s => s.StgRequestCancel).First())
                            {
                                OneItem.StgRequestEdit = true;
                                OneItem.RequestStatus = "Отмена";
                                OneItem.RequestNextStep = "Удалить";
                                OneItem.StgRequestCancel = false;
                                db.Requests.Update(OneItem);
                                db.SaveChanges();
                                db.Dispose();

                                OnPropertyChanged(nameof(Requests));
                                OnPropertyChanged(nameof(OneItem));
                            }
                            else
                            {
                                MessageBox.Show("Отмена не возможна");
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }


                }));
            }
        }


        /// <summary>
        /// Пересоздать базу данных
        /// </summary>
        private RelayCommand? _DataBase_Remove;
        public RelayCommand DataBase_Remove
        {
            get
            {
                return _DataBase_Remove ??
                (_DataBase_Remove = new RelayCommand((o) =>
                {
                    try
                    {
                        MessageBoxResult result = MessageBox.Show("Пересоздать базу данных?", "Подтвердите действие", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {


                            new DataBaseContext().Database.EnsureDeleted();

                            DataBaseContext db = new DataBaseContext();

                            List<Request> NewItem = new List<Request>()
                            {
                                new Request {  RequestText = "Доставить груз_1 в Питер", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Санкт-Петербург", StgRequestCancel = true },
                                new Request {  RequestText = "Доставить груз_1 в Иваново", RequestStatus = "На выполнении", TransferStart = "Москва", TransferEnd = "Иваново", RequestNextStep = "Выполнено", StgRequestCancel = false, StgRequestEdit = true },
                                new Request {  RequestText = "Доставить груз_1 в Рязань", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Рязань", StgRequestCancel = true },
                                new Request {  RequestText = "Доставить груз_1 в Тула", RequestStatus = "Отмена", TransferStart = "М.О.", TransferEnd = "Тула", RequestNextStep = "Удалить", StgRequestCancel = false, StgRequestEdit = true },
                                new Request {  RequestText = "Доставить груз_1 в Звенигород", RequestStatus = "Выполнено", TransferStart = "Москва", TransferEnd = "Звенигород", RequestNextStep = "Удалить", StgRequestCancel = false, StgRequestEdit = true },

                                new Request {  RequestText = "Доставить груз_2 в Питер", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Санкт-Петербург", StgRequestCancel = true },
                                new Request {  RequestText = "Доставить груз_2 в Иваново", RequestStatus = "На выполнении", TransferStart = "Москва", TransferEnd = "Иваново", RequestNextStep = "Выполнено", StgRequestCancel = false, StgRequestEdit = true },
                                new Request {  RequestText = "Доставить груз_2 в Рязань", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Рязань", StgRequestCancel = true },
                                new Request {  RequestText = "Доставить груз_2 в Тула", RequestStatus = "Отмена", TransferStart = "М.О.", TransferEnd = "Тула", RequestNextStep = "Удалить", StgRequestCancel = false, StgRequestEdit = true },
                                new Request {  RequestText = "Доставить груз_2 в Звенигород", RequestStatus = "Выполнено", TransferStart = "Москва", TransferEnd = "Звенигород", RequestNextStep = "Удалить", StgRequestCancel = false, StgRequestEdit = true },

                                new Request {  RequestText = "Доставить груз_3 в Питер", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Санкт-Петербург", StgRequestCancel = true },
                                new Request {  RequestText = "Доставить груз_3 в Иваново", RequestStatus = "На выполнении", TransferStart = "Москва", TransferEnd = "Иваново", RequestNextStep = "Выполнено", StgRequestCancel = false, StgRequestEdit = true },
                                new Request {  RequestText = "Доставить груз_3 в Рязань", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Рязань", StgRequestCancel = true },
                                new Request {  RequestText = "Доставить груз_3 в Тула", RequestStatus = "Отмена", TransferStart = "М.О.", TransferEnd = "Тула", RequestNextStep = "Удалить", StgRequestCancel = false, StgRequestEdit = true },
                                new Request {  RequestText = "Доставить груз_3 в Звенигород", RequestStatus = "Удалено", TransferStart = "Москва", TransferEnd = "Звенигород", StgRequestMove = false, RequestNextStep = string.Empty, StgRequestCancel = false, StgRequestEdit = true },
                            };


                            db.AddRange(NewItem);
                            db.SaveChanges();
                            db.Dispose();

                            OnPropertyChanged(nameof(Requests));
                        }

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                }));
            }
        }




        /// <summary>
        /// Отображаемый список
        /// </summary>
        public ObservableCollection<Request> Requests
        {
            get
            {
                DataBaseContext db = new DataBaseContext();
                ObservableCollection<Request> item = new ObservableCollection<Request>(db.Requests.AsEnumerable().Where(w => FiltdedCheck(w)));
                db.Dispose();

                return item;
              
            }
        }

        /// <summary>
        /// Обновление изменений
        /// </summary>

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
