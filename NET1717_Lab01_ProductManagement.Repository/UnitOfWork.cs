using NET1717_Lab01_ProductManagement.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NET1717_Lab01_ProductManagement.Repository
{
    public class UnitOfWork
    {
        private MyDbContext _context;
        private GenericRepository<CategoryEntity> _category;
        private GenericRepository<ProductEntity> _product;
        private GenericRepository<User> _user;



        public UnitOfWork(MyDbContext context)
        {
            _context = context;
        }

        public GenericRepository<CategoryEntity> CategoryRepository
        {
            get
            {
                if (_category == null)
                {
                    this._category = new GenericRepository<CategoryEntity>(_context);
                }
                return _category;
            }

        }
        public GenericRepository<ProductEntity> ProductRepository
        {
            get
            {
                if (_product == null)
                {
                    this._product = new GenericRepository<ProductEntity>(_context);
                }
                return _product;
            }

        }
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (_user == null)
                {
                    this._user = new GenericRepository<User>(_context);
                }
                return _user;
            }

        }


        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
