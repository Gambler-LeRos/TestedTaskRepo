using System.Collections.ObjectModel;

namespace TestedTask
{
    class DataContext_db
    {

        public static ObservableCollection<Request> GetRequests()
        {

            return new ObservableCollection<Request>
            {
                new Request { IdRequest = 1, RequestText = "Доставить груз_1 в Питер", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Санкт-Петербург", StgRequestCancel = true },
                new Request { IdRequest = 2, RequestText = "Доставить груз_1 в Иваново", RequestStatus = "Выполнение", TransferStart = "Москва", TransferEnd = "Иваново", RequestNextStep = "В выполненые", StgRequestCancel = false, StgRequestEdit = true },
                new Request { IdRequest = 3, RequestText = "Доставить груз_1 в Рязань", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Рязань" , StgRequestCancel = true},
                new Request { IdRequest = 4, RequestText = "Доставить груз_1 в Тула", RequestStatus = "Отмена", TransferStart = "М.О.", TransferEnd = "Тула", RequestNextStep = "Удалить", StgRequestCancel = false, StgRequestEdit = true},
                new Request { IdRequest = 5, RequestText = "Доставить груз_1 в Звенигород", RequestStatus = "Выполнено", TransferStart = "Москва", TransferEnd = "Звенигород", RequestNextStep =  "Удалить", StgRequestCancel = false, StgRequestEdit = true},

                new Request { IdRequest = 1, RequestText = "Доставить груз_2 в Питер", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Санкт-Петербург" , StgRequestCancel = true},
                new Request { IdRequest = 2, RequestText = "Доставить груз_2 в Иваново", RequestStatus = "Выполнение", TransferStart = "Москва", TransferEnd = "Иваново", RequestNextStep = "В выполненые", StgRequestCancel = false, StgRequestEdit = true },
                new Request { IdRequest = 3, RequestText = "Доставить груз_2 в Рязань", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Рязань", StgRequestCancel = true },
                new Request { IdRequest = 4, RequestText = "Доставить груз_2 в Тула", RequestStatus = "Отмена", TransferStart = "М.О.", TransferEnd = "Тула", RequestNextStep = "Удалить", StgRequestCancel = false, StgRequestEdit = true},
                new Request { IdRequest = 5, RequestText = "Доставить груз_2 в Звенигород", RequestStatus = "Выполнено", TransferStart = "Москва", TransferEnd = "Звенигород", RequestNextStep =  "Удалить", StgRequestCancel = false, StgRequestEdit = true},

                new Request { IdRequest = 1, RequestText = "Доставить груз_3 в Питер", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Санкт-Петербург" , StgRequestCancel = true},
                new Request { IdRequest = 2, RequestText = "Доставить груз_3 в Иваново", RequestStatus = "Выполнение", TransferStart = "Москва", TransferEnd = "Иваново", RequestNextStep = "В выполненые", StgRequestCancel = false, StgRequestEdit = true },
                new Request { IdRequest = 3, RequestText = "Доставить груз_3 в Рязань", RequestStatus = "Новая", TransferStart = "Москва", TransferEnd = "Рязань", StgRequestCancel = true },
                new Request { IdRequest = 4, RequestText = "Доставить груз_3 в Тула", RequestStatus = "Отмена", TransferStart = "М.О.", TransferEnd = "Тула", RequestNextStep = "Удалить", StgRequestCancel = false, StgRequestEdit = true},
                new Request { IdRequest = 5, RequestText = "Доставить груз_3 в Звенигород", RequestStatus = "Удалено", TransferStart = "Москва", TransferEnd = "Звенигород", StgRequestMove = false, RequestNextStep =  string.Empty, StgRequestCancel = false, StgRequestEdit = true},

            };

        }

    }
}
