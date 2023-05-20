using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestedTask
{
    public class Request
    {
        /// <summary>
        /// Идентификатор заяки
        /// </summary>
        [Key, Column, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int IdRequest { get; set; }
        /// <summary>
        /// Время создания заявки
        /// </summary>
        [Required, Column] public DateTime RequestDate { get; set; } = DateTime.Now; 

        private string _RequestText { get; set; } = string.Empty; 
        /// <summary>
        /// Переменная описания заявки
        /// </summary>
        [Required, Column] public string RequestText { get => _RequestText; set { _RequestText = TextVerify(_RequestText, value); } }
        private string _RequestCancelText { get; set; } = string.Empty;
        /// <summary>
        /// Комментарий
        /// </summary>
        [Required, Column] public string RequestCancelText { get => _RequestCancelText; set { _RequestCancelText = TextVerify(_RequestCancelText, value); } }
        private string _TransferStart { get; set; } = string.Empty; 
        /// <summary>
        /// Место отправления
        /// </summary>
        [Required, Column] public string TransferStart { get => _TransferStart; set { _TransferStart = TextVerify(_TransferStart, value); } }
        private string _TransferEnd { get; set; } = string.Empty; 
        /// <summary>
        /// Пункт Назначения
        /// </summary>
        [Required, Column] public string TransferEnd { get => _TransferEnd; set { _TransferEnd = TextVerify(_TransferEnd, value);} }
        /// <summary>
        /// Статус заявки
        /// </summary>
        [Required, Column] public string RequestStatus { get; set; } = string.Empty; 
        /// <summary>
        /// Следующий этап обработки заявки
        /// </summary>
        [Required, Column] public string RequestNextStep { get; set; } = "На выполнение"; 
        /// <summary>
        /// Возможность перехода на следующий этап обработки
        /// </summary>
        [Required, Column] public bool StgRequestMove { get; set; } = true; 
        /// <summary>
        /// Подвязывается к свойству ReadOnly по этому по умолчанию False
        /// </summary>
        [Required, Column] public bool StgRequestEdit { get; set; } = false;
        /// <summary>
        /// Возможность отмены заяки
        /// </summary>
        [Required, Column] public bool StgRequestCancel { get; set; } = false; 
        /// <summary>
        /// Статус удаления
        /// </summary>
        [Required, Column] public bool StgRequestDelete { get; set; } = false; 


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


