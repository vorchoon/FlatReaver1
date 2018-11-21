using System;
using System.Collections.Generic;

namespace DataStore
{
    /// <summary>
    /// Модель Пользователя
    /// </summary>
    public class User
    {

        /// <summary>
        /// ID Пользователя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ID Пользователя
        /// </summary>
        public long? RoleId { get; set; }

        /// <summary>
        /// Логин Пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Хэш Пароля Пользователя
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Ссылка на роль
        /// </summary>
        public Role Role { get; set; }

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