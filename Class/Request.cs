using System;

namespace TestedTask
{
    class Request
    {
        public int IdRequest { get; set; }  
        public string RequestDate { get; set; } = DateTime.Now.ToShortDateString();
        public string RequestText { get; set; } = string.Empty;
        public string RequestStatus { get; set; } = "Новая";
        public string RequestCancelText { get; set; } = string.Empty;
        public string TransferStart { get; set; } = string.Empty;
        public string TransferEnd { get; set; } = string.Empty;



        //Параметры отображения и возможности изменения данных
        public string RequestNextStep { get; set; } = "На выполнение"; //Следующий этап обработки заявки
        public bool StgRequestMove { get; set; } = true; //Возможность перехода на следующий этап обработки
        public bool StgRequestEdit { get; set; } = false; //Подвязывается к свойству ReadOnly по этому по умолчанию False
        public bool StgRequestCancel { get; set; } = false; //Возможность отмены заяки
        public bool StgRequestDelete { get; set; } = false; //Статус удаления
    }

}
        

