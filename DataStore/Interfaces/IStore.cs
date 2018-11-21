using System.Collections.Generic;

namespace DataStore
{
    public interface IStore
    {
        /// <summary>
        /// Создает сущность
        /// </summary>
        /// <typeparam name="T">Тип сущности (Не обязателен)</typeparam>
        /// <param name="item">Сущность</param>
        void Create<T>(T item) where T : class;

        /// <summary>
        /// Удаляет сущность
        /// </summary>
        /// <typeparam name="T">Тип сущности (Не обязателен)</typeparam>
        /// <param name="item">Сущность</param>
        void Delete<T>(T item) where T : class;

        /// <summary>
        /// Считывает коллекцию сущностей, чьи свойства совпадают со свойствами образца
        /// </summary>
        /// <typeparam name="T">Тип считываемой сущности</typeparam>
        /// <param name="prototype">Объект - образец</param>
        /// <param name="isStrict">Требовать точного совпадения значений</param>
        /// <param name="needMatchAll">Требовать совпадения каждого значения</param>
        /// <param name="pageSize">Размер страницы при пагинации</param>
        /// <param name="pageIndex">Индекс страницы (с 0) при пагинации</param>
        /// <returns>Коллекция сущностей</returns>
        List<T> Read<T>(object prototype = null, bool isStrict = true, bool needMatchAll = true, int? pageSize = null, int? pageIndex = null) where T : class;

        /// <summary>
        /// Считывает сущность, чьи свойства совпадают со свойствами образца, или возвращает null
        /// </summary>
        /// <typeparam name="T">Тип считываемой сущности</typeparam>
        /// <param name="prototype">Объект - образец</param>
        /// <param name="isStrict">Требовать точного совпадения значений</param>
        /// <param name="needMatchAll">Требовать совпадения каждого значения</param>
        /// <returns>Сущность или null</returns>
        T ReadSingle<T>(object prototype, bool isStrict = true, bool needMatchAll = true) where T : class;

        /// <summary>
        /// Обновляет сущность
        /// </summary>
        /// <typeparam name="T">Тип сущности (Не обязателен)</typeparam>
        /// <param name="item">Сущность</param>
        void Update<T>(T item) where T : class;
    }
}