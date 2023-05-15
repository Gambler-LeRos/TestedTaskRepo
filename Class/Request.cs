using System;

namespace TestedTask
{
    class Request
    {
        public int IdRequest { get; set; }
        private DateTime _RequestDate = DateTime.Now; //Дата создания заявки
        private string _RequestText = string.Empty; //Описание заявки
        private string _RequestCancelText = string.Empty; //Комментарий
        private string _TransferStart = string.Empty; //Пункт отправления
        private string _TransferEnd = string.Empty; //Место назначения

        public DateTime RequestDate
        {
            get => _RequestDate;
            set
            {
                _RequestDate = value;
            }
        }

        public string RequestText
        {
            get => _RequestText;
            set
            {
                _RequestText = TextVerify(_RequestText, value);
            }
        }
        public string RequestCancelText
        {
            get => _RequestCancelText;
            set
            {
                _RequestCancelText = TextVerify(_RequestText, value);
            }
        }
        public string TransferStart
        {
            get => _TransferStart;
            set
            {
                _TransferStart = TextVerify(_RequestText, value);
            }
        }
        public string TransferEnd
        {
            get => _TransferEnd;
            set
            {
                _TransferEnd = TextVerify(_RequestText, value);
            }
        }



        //Параметры отображения и возможности изменения данных
        public string RequestStatus { get; set; } = "Новая"; //Статус заявки
        public string RequestNextStep { get; set; } = "На выполнение"; //Следующий этап обработки заявки
        public bool StgRequestMove { get; set; } = true; //Возможность перехода на следующий этап обработки
        public bool StgRequestEdit { get; set; } = false; //Подвязывается к свойству ReadOnly по этому по умолчанию False
        public bool StgRequestCancel { get; set; } = false; //Возможность отмены заяки
        public bool StgRequestDelete { get; set; } = false; //Статус удаления



        private string TextVerify(string OldText, string NewText)
        {

            if (!string.IsNullOrEmpty(NewText))
            {
                OldText = NewText.Substring(0, 1).ToUpper() + NewText.Substring(1, NewText.Length - 1);
            }

            return OldText;
        }
    }

}


