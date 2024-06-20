using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NET1717_Lab01_ProductManagement.Repository.DBContextConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NET1717_Lab01_ProductManagement.Repository.Entities
{
    public class MyDbContext : DbContext
    {
        private static bool _databaseInitialized = false;
        private static readonly object _lock = new object();
        public MyDbContext()
        {

        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            // Ensure that the database initialization runs only once
            if (!_databaseInitialized)
            {
                lock (_lock)
                {
                    if (!_databaseInitialized)
                    {
                        try
                        {
                            var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                            if (databaseCreator != null)
                            {
                                if (databaseCreator.CanConnect())
                                {
                                    databaseCreator.Delete();
                                }
                                databaseCreator.Create();
                                databaseCreator.CreateTables();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        _databaseInitialized = true;
                    }
                }
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        }

        public virtual DbSet<CategoryEntity> CategoryEntities { get; set; }
        public virtual DbSet<ProductEntity> ProductEntities { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Hao",
                    userName = "kakaka",
                    userPassword = "123",
                    role = "admin"
                },
                new User
                {
                    Id = 2,
                    Name = "string",
                    userName = "string",
                    userPassword = "string",
                    role = "user"
                }
                );
            modelBuilder.Entity<CategoryEntity>().HasData(
                new CategoryEntity { CategoryId = 1, CategoryName = "Beverages" },
                new CategoryEntity { CategoryId = 2, CategoryName = "Condiments" },
                new CategoryEntity { CategoryId = 3, CategoryName = "Confections" },
                new CategoryEntity { CategoryId = 4, CategoryName = "Dairy Products" },
                new CategoryEntity { CategoryId = 5, CategoryName = "Grains/Cereals" },
                new CategoryEntity { CategoryId = 6, CategoryName = "Meat/Poultry" },
                new CategoryEntity { CategoryId = 7, CategoryName = "Produce" },
                new CategoryEntity { CategoryId = 8, CategoryName = "Seafood" }
                );
            modelBuilder.Entity<ProductEntity>().HasData(
                new ProductEntity
                {
                    ProductId = 1,
                    ProductName = "Vu Nhat Hao",
                    CategoryId = 1,
                    UnitsInStock = 1,
                    UnitPrice = 5,
                },
                new ProductEntity
                {
                    ProductId = 2,
                    ProductName = "Nguyen Hoang Bao",
                    CategoryId = 2,
                    UnitsInStock = 2,
                    UnitPrice = 6,
                },
                new ProductEntity
                {
                    ProductId = 3,
                    ProductName = "Nguyen Hung Hao",
                    CategoryId = 3,
                    UnitsInStock = 3,
                    UnitPrice = 7,
                },
                new ProductEntity
                {
                    ProductId = 5,
                    ProductName = "Truong Sy Quang",
                    CategoryId = 5,
                    UnitsInStock = 5,
                    UnitPrice = 8,
                },
                new ProductEntity
                {
                    ProductId = 6,
                    ProductName = "TongTruongThanh",
                    CategoryId = 6,
                    UnitsInStock = 6,
                    UnitPrice = 8,
                },
                new ProductEntity
                {
                    ProductId = 7,
                    ProductName = "Nguyen Trong Nghia",
                    CategoryId = 7,
                    UnitsInStock = 7,
                    UnitPrice = 9,
                }
                );
        }


    }
}
