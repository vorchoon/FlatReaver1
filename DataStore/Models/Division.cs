using System;
using System.Collections.Generic;

namespace DataStore
{
    /// <summary>
    /// Модель Отделения
    /// </summary>
    public class Division
    {
        /// <summary>
        /// Наименование отделения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID Отделения
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Дата модификации записи
        /// </summary>
        public DateTime? EditedDateTime { get; set; }

        /// <summary>
        /// Нацигационное свойство для связи с риелторами
        /// </summary>
        public List<Rieltor> Rieltors { get; set; }

    }
}