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
        string _filtertext = string.Empty;

        /// <summary>
        /// Переменная фильтра строки поиска
        /// </summary>
        public string Filter
        {
            get => _filtertext;
            set
            {
                _filtertext = value;
                OnPropertyChanged(nameof(Requests));
            }
        }
             
        string _filterStatus = String.Empty;

        /// <summary>
        /// Переменная фильтра статуса заявок
        /// </summary>
        public string FilterStatus
        {
            get => _filterStatus;
            set
            {
                _filterStatus = value;
                OnPropertyChanged(nameof(Requests));
            }
        }
     
        private RelayCommand? _changedViewStatus;

        /// <summary>
        /// Изменение отображаемого списка заявок по статусам
        /// </summary>
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

        public event Action<int> OpenInfoWindowEvent;

        private RelayCommand? _OpenInfoWindow;

        /// <summary>
        /// Открытие окна информации для добавления, редактирования и отмены заявки
        /// </summary>
        public RelayCommand OpenInfoWindow
        {
            get
            {
                return _OpenInfoWindow ??
                (_OpenInfoWindow = new RelayCommand((item) =>
                {
                    OpenInfoWindowEvent?.Invoke(Convert.ToInt32(item));
                }));
            }
        }

        /// <summary>
        /// Закрытие окна информации
        /// </summary>
        public void DialogClosed()
        {
            OnPropertyChanged(nameof(Requests));
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

     
        private RelayCommand? _changedStatus; 

        /// <summary>
        /// Изменение статуса и настроек отображения/изменения заявки/Создание записи новой заявки
        /// </summary>
        public RelayCommand ChangedStatus
        {
            get
            {
                return _changedStatus ??
                (_changedStatus = new RelayCommand((obj) =>
                {
                    Request? item = obj as Request;
                    if (item != null)
                    {
                        using DataBaseContext db = new();
                        if ((item != null && item.StgRequestMove) )
                        {
                            item.StgRequestEdit = true;
                            switch (item.RequestStatus)
                            {
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

                            OnPropertyChanged(nameof(Requests));

                        }

                    }
                    else MessageBox.Show("Внесение изменений не возможно");

                }));
            }
        }


        private RelayCommand? _DataBase_Remove;

        /// <summary>
        /// Пересоздать базу данных
        /// </summary>
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
                            using (DataBaseContext bdel = new()) bdel.Database.EnsureDeleted();

                            using DataBaseContext db = new();
                            {
                                List<Request> NewItem = new()
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

                                OnPropertyChanged(nameof(Requests));
                            }
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
                using DataBaseContext db = new();
                return new(db.Requests.AsEnumerable().Where(w => FiltdedCheck(w)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// Обновление изменений
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
