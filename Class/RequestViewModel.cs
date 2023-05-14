using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TestedTask
{
    class RequestViewModel : INotifyPropertyChanged
    {

        string _filtertext = string.Empty;
        public string Filter //Переменная фильтра строки поиска
        {
            get => _filtertext;
            set
            {
                _filtertext = value;
                OnPropertyChanged(nameof(Requests));
            }
        }

        string _filterStatus = String.Empty;
        public string FilterStatus //Переменная фильтра статуса заявок
        {
            get => _filterStatus;
            set
            {
                _filterStatus = value;
                OnPropertyChanged(nameof(Requests));
            }
        }



        private RelayCommand? _changedViewStatus;
        public RelayCommand ChangedViewStatus //Изменение отображаемого списка заявок по статусам
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

        public Visibility BorderVisibility { get; set; } = Visibility.Hidden; //Отображение окна добавления и отмены заявки
        public Visibility CancelReqVisibility { get; set; } = Visibility.Hidden; //Отображение строки комментария при отмене заявки
        public Visibility NewReqVisibility { get; set; } = Visibility.Visible; //Отображение кнопки создания заявки
        public Request OneItem { get; set; } = new Request(); //Переменная для создания и отмены заявки

        private RelayCommand? _changedVisibility;
        public RelayCommand ChangedVisibility //Изменение видимости окна добавления и отмены заявки
        {
            get
            {
                return _changedVisibility ??
                (_changedVisibility = new RelayCommand((o) =>
                {


                    if (o is Request)
                    {
                        OneItem = (Request)o;
                        CancelReqVisibility = Visibility.Visible;
                        NewReqVisibility = Visibility.Hidden;
                    }
                    else
                    {
                        OneItem = new Request { RequestNextStep = "Создать", RequestStatus = "Создать" };
                        CancelReqVisibility = Visibility.Hidden;
                        NewReqVisibility = Visibility.Visible;                      
                    }

                    OnPropertyChanged(nameof(NewReqVisibility));
                    OnPropertyChanged(nameof(CancelReqVisibility));
                    OnPropertyChanged(nameof(OneItem));



                    if (BorderVisibility == Visibility.Hidden) BorderVisibility = Visibility.Visible;
                    else BorderVisibility = Visibility.Hidden;

                    OnPropertyChanged(nameof(BorderVisibility));
                }));
            }
        }




        public bool FiltdedCheck(Request item) //Фильтрация списка коллекции
        {
            bool accept = true;
            if (!string.IsNullOrEmpty(Filter))
            {
                string FindText = Filter.ToLower();
                accept = (item.RequestText.ToLower().Contains(FindText) || item.TransferStart.ToLower().Contains(FindText) || item.TransferEnd.ToLower().Contains(FindText) || item.RequestStatus.ToLower().Contains(FindText));
            }
            else if (FilterStatus != string.Empty && item.RequestStatus != FilterStatus) accept = false;

            return accept;
        }


        private RelayCommand? _changedStatus;
        public RelayCommand ChangedStatus //Изменение статуса и настроек отображения/изменения заявки
        {
            get
            {
                return _changedStatus ??
                (_changedStatus = new RelayCommand((obj) =>
                {
                    Request item = obj as Request;
                    if (item != null)
                    {
                        item.StgRequestEdit = true;

                        switch (item.RequestStatus)
                        {
                            case "Создать":
                                item.RequestNextStep = "На выполнение";
                                item.RequestStatus = "Новая";
                                item.StgRequestCancel = true;
                                item.StgRequestEdit = false;
                                Requests_db.Add(item);
                                break;
                            case "Новая":                               
                                item.RequestStatus = "Выполнение";
                                item.RequestNextStep = "В выполненые";
                                item.StgRequestCancel = false;
                                break;
                            case "Выполнение":                          
                                item.RequestStatus = "Выполнено";
                                item.RequestNextStep = "Удалить";
                                break;
                            case "Отмена" or "Выполнено":                            
                                item.RequestNextStep = string.Empty;
                                item.StgRequestMove = false;
                                item.RequestStatus = "Удалено";
                                break;
                        }
               

                        var item_db = Requests_db.Where(w => w.IdRequest == item.IdRequest).FirstOrDefault();
                        if (item_db != null)
                        {
                            item_db = item;
                        }

                        OnPropertyChanged(nameof(OneItem));
                        OnPropertyChanged(nameof(Requests));
                    }
                }));
            }
        }


        private RelayCommand? _CancelRequest;
        public RelayCommand CancelRequest //Отмена заявки
        {
            get
            {
                return _CancelRequest ??
                (_CancelRequest = new RelayCommand((obj) =>
                {
                    if (string.IsNullOrEmpty(OneItem.RequestCancelText)) MessageBox.Show("Укажите в комментарии причину отмены");
                    else
                    {
                        OneItem.StgRequestEdit = true;
                        OneItem.RequestStatus = "Отмена";
                        OneItem.RequestNextStep = "Удалить";
                        OneItem.StgRequestCancel = false;

                        BorderVisibility = Visibility.Hidden;
                        OnPropertyChanged(nameof(BorderVisibility));
                    }
                    OnPropertyChanged(nameof(Requests));

                }));
            }
        }





        public ObservableCollection<Request> Requests_db = new ObservableCollection<Request>(DataContext_db.GetRequests()); //Эмуляция запроса к базе данных
        public ObservableCollection<Request> Requests //Отображаемый список
        {
            get { return new ObservableCollection<Request>(Requests_db.Where(w => FiltdedCheck(w))); }
        }
     

     
        //Обновление изменений
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
