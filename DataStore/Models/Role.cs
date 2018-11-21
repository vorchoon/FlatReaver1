using System;
using System.Collections.Generic;
using System.Text;

namespace DataStore
{
    public class Role
    {
        /// <summary>
        /// ID Роли
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Название роли
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Нацигационное свойство для связи с пользователями
        /// </summary>
        public List<User> Users { get; set; }

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
