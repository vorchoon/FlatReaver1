using System;

namespace DataStore
{
    /// <summary>
    /// Интерфейс логгера
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Вносит ошибку в лог
        /// </summary>
        /// <param name="exception">Исключение</param>
        void ErrorLog(Exception exception);

        /// <summary>
        /// Вносит ошибку в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        void ErrorLog(string message);

        /// <summary>
        /// Вносит информационное сообщение в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Log(string message);
    }
}