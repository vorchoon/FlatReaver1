using System;

namespace DataStore
{
    /// <summary>
    /// Модель риелтора 
    /// </summary>
    public class Rieltor
    {
        /// <summary>
        /// Имя риелтора
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Фамилия риелтора
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Ссылка на подразделение
        /// </summary>
        public Division Division { get; set; }

        /// <summary>
        /// ID Риелтора
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ВК Подразделения
        /// </summary>
        public long? DivisionId { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Дата модификации записи
        /// </summary>
        public DateTime? EditedDateTime { get; set; }

    }
}