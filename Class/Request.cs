using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestedTask
{
    public class Request
    {

        [Key, Column, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdRequest { get; set; }
        [Required, Column] public DateTime RequestDate { get; set; } = DateTime.Now; // Время создания заявки

        private string _RequestText { get; set; } = string.Empty; //Переменная описания заявки
        [Required, Column] public string RequestText { get => _RequestText; set { _RequestText = TextVerify(_RequestText, value); } }
        private string _RequestCancelText { get; set; } = string.Empty; //Комментарий
        [Required, Column] public string RequestCancelText { get => _RequestCancelText; set { _RequestCancelText = TextVerify(_RequestCancelText, value); } }
        private string _TransferStart { get; set; } = string.Empty; //Место отправления
        [Required, Column] public string TransferStart { get => _TransferStart; set { _TransferStart = TextVerify(_TransferStart, value); } }
        private string _TransferEnd { get; set; } = string.Empty; //Пункт Назначения
        [Required, Column] public string TransferEnd { get => _TransferEnd; set { _TransferEnd = TextVerify(_TransferEnd, value);} }

        [Required, Column] public string RequestStatus { get; set; } = "Новая"; //Статус заявки
        [Required, Column] public string RequestNextStep { get; set; } = "На выполнение"; //Следующий этап обработки заявки
        [Required, Column] public bool StgRequestMove { get; set; } = true; //Возможность перехода на следующий этап обработки
        [Required, Column] public bool StgRequestEdit { get; set; } = false; //Подвязывается к свойству ReadOnly по этому по умолчанию False
        [Required, Column] public bool StgRequestCancel { get; set; } = false; //Возможность отмены заяки
        [Required, Column] public bool StgRequestDelete { get; set; } = false; //Статус удаления


        /// <summary>
        /// Модификация входящих данных
        /// </summary>
        /// <param name="OldText"></param>
        /// <param name="NewText"></param>
        /// <returns></returns>
        private string TextVerify(string OldText, string NewText)
        {
            if (!string.IsNullOrEmpty(NewText)) OldText = NewText.Substring(0, 1).ToUpper() + NewText.Substring(1, NewText.Length - 1);

            return OldText;
        }
      


    }

}


