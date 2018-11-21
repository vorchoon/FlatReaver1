using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DataStore
{

    public class StorageContext : DbContext
    {
        /// <summary>
        /// Стркоа подключения
        /// </summary>
        private string ConnectionString;

        /// <summary>
        /// Контекст БД
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public StorageContext(string connectionString)
        {
            ConnectionString = connectionString == null ? throw new Exception("Need Connection string!") : connectionString;
            Database.EnsureCreated();
        }

        /// <summary>
        /// Датасет БД Риелторов
        /// </summary>
        public DbSet<Rieltor> Rieltors { get; set; }

        /// <summary>
        /// Датасет БД Подразделений
        /// </summary>
        public DbSet<Division> Divisions { get; set; }

        /// <summary>
        /// Датасет БД Пользователей
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Датасет БД Ролей
        /// </summary>
        public DbSet<Role> Roles { get; set; }


        /// <summary>
        /// Конфигурирование контекста БД
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        /// <summary>
        /// Конфигурируем модели с помощью Fluent API
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rieltor>()
                .Property(item => item.Firstname)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Rieltor>()
                .Property(item => item.Lastname)
                .IsRequired()
                .HasMaxLength(200);
            
            modelBuilder.Entity<Rieltor>()
               .Property(item => item.CreatedDateTime)
               .HasDefaultValueSql("GETDATE()")
               .IsRequired();

            modelBuilder.Entity<Division>()
                .Property(item => item.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Division>()
                .Property(item => item.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(item => item.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Role>()
                .Property(item => item.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(item => item.Login)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<User>()
                .Property(item => item.PasswordHash)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<User>()
                .Property(item => item.CreatedDateTime)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();


        }
    }
}
