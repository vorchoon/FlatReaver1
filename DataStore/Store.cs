using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace DataStore
{

    /// <summary>
    /// Хранилище Данных
    /// </summary>
    public sealed class Store : IStore
    {
        /// <summary>
        /// строка подключения
        /// </summary>
        private string ConnectionString;

        /// <summary>
        /// Конструктор Хранилища
        /// </summary>
        /// <param name="connectionString">Строка подключеняи к БД</param>
        /// <param name="logger">Логгер операций</param>
        public Store(string connectionString, ILogger logger = null)
        {
            Logger = logger == null ? new SimpleLogger() : logger;

            ConnectionString = connectionString;
        }

        /// <summary>
        /// Содержит логгер. По умолчанию - консольный.
        /// </summary>
        private static ILogger Logger;


        /// <summary>
        /// Готовит первичную выборку сущностей, удовлетворяющих условиям поиска
        /// </summary>
        /// <param name="db">Контекст данных</param>
        /// <returns>Возвращает коллекцию, управляемую EntityFramework</returns>
        private IOrderedQueryable<T> SelectEntities<T>(StorageContext db, object prototype, bool isStrict = true, bool needMatchAll = true) where T : class
        {
            return db.Set<T>()
               .Where(e => IsMatched(e, prototype, isStrict, needMatchAll)
                      )
                      .OrderBy(e => (long)e.GetType()
                                  .GetProperty("Id")
                                  .GetValue(e)
                              );

        }

        /// <summary>
        /// Проверяет попадание элемента последовательности в указанные условия
        /// </summary>
        private bool IsMatched<T>(T e, object prototype, bool isStrict, bool needMatchAll) where T : class
        {
            var strictMatches = 0;
            var unstrictMatches = 0;

            var testingProperties = e.GetType().GetProperties();

            var prototypeProperties = prototype.GetType().GetProperties();

            foreach (var protoProperty in prototypeProperties)
            {
                var testingProperty = testingProperties.Single(p => p.Name == protoProperty.Name);

                dynamic testingValue = testingProperty.GetValue(e);
                dynamic protoValue = Convert.ChangeType(protoProperty.GetValue(prototype), testingValue.GetType());

                if (testingValue == protoValue)
                {
                    strictMatches++;
                }
                else
                {
                    try
                    {
                        if (
                                ((string)testingProperty.GetValue(e))
                                .ToLower()
                                .Contains(
                                            ((string)protoProperty.GetValue(prototype))
                                            .ToLower()
                                         )
                            )
                        {
                            unstrictMatches++;
                        }
                    }
                    catch
                    {

                    }
                }



            }
            if (isStrict && needMatchAll)
            {
                return strictMatches == prototypeProperties.Count() ? true : false;
            }
            else

            if (!isStrict && needMatchAll)
            {
                return (unstrictMatches + strictMatches) == prototypeProperties.Count() ? true : false;
            }
            else

            if (isStrict && !needMatchAll)
            {
                return strictMatches > 0 ? true : false;
            }
            else

            {
                return (unstrictMatches + strictMatches) > 0 ? true : false;
            }
        }

        /// <summary>
        /// Готовит первичную выборку сущностей без проверки условий поиска
        /// </summary>
        /// <param name="db">Контекст данных</param>
        /// <returns>Возвращает коллекцию, управляемую EntityFramework</returns>
        private IOrderedQueryable<T> SelectEntities<T>(StorageContext db) where T : class
        {

            return db.Set<T>()
                .OrderBy(e => (long)e.GetType()
                        .GetProperty("Id")
                        .GetValue(e)
                         );

        }

        public void Update<T>(T item) where T : class
        {
            try
            {
                using (var db = new StorageContext(ConnectionString))
                {
                    try
                    {
                        item.GetType()
                            .GetProperty("EditedDateTime")
                            .SetValue(item, DateTime.Now);
                    }
                    catch { }

                    db.Set<T>().Update(item);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                throw new Exception("Error");
            }

        }

        public void Delete<T>(T item) where T : class
        {
            try
            {
                using (var db = new StorageContext(ConnectionString))
                {
                    db.Remove(item);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                throw new Exception("Error");
            }

        }


        public void Create<T>(T item) where T : class
        {
            try
            {
                using (var db = new StorageContext(ConnectionString))
                {
                    db.Add(item);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                throw new Exception("Error");
            }

        }

        public T ReadSingle<T>(object prototype, bool isStrict = true, bool needMatchAll = true) where T : class
        {
            try
            {
                var entities = Read<T>(prototype, isStrict, needMatchAll);

                return entities.Count == 0 ? null : entities.Single();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                throw new Exception("Error");
            }

        }

        public List<T> Read<T>(object prototype = null, bool isStrict = true, bool needMatchAll = true, int? pageSize = null, int? pageIndex = null) where T : class
        {
            try
            {
                using (var db = new StorageContext(ConnectionString))
                {
                    var entities = prototype != null ? SelectEntities<T>(db, prototype, isStrict, needMatchAll) : SelectEntities<T>(db);

                    return pageSize == null || pageIndex == null ? entities.ToList() : entities.Skip((int)pageIndex * (int)pageSize)
                                                                                               .Take((int)pageSize)
                                                                                               .ToList();

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                throw new Exception("Error");
            }
        }

    }
}
