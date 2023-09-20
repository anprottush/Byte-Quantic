using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Model.Data;
using SuperShop.Model.DBEntity;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Repository.TypeRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository<RefreshToken> RefreshToken { get; private set; }
        public GenericRepository<ApplicationUser> ApplicationUser { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ApplicationUser = new GenericRepository<ApplicationUser>(_context);
            RefreshToken = new GenericRepository<RefreshToken>(_context);
        }
        public OperationResult Save()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                     _context.SaveChanges();
                     dbContextTransaction.Commit();

                    return new OperationResult(true, null, "Data Save Success.");
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return new OperationResult(false, null, ex.Message);
                }
            }
        }
        public async Task<OperationResult> SaveAsync()
        {
            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();

                    return new OperationResult(true, null, "Data Save Success.");
                }
                catch (Exception ex)
                {
                    await dbContextTransaction.RollbackAsync();
                    return new OperationResult(false, null, ex.Message);
                }
            }
        }

        // Garbage Collector
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

    public interface IUnitOfWork : IDisposable
    {
        OperationResult Save();
        Task<OperationResult> SaveAsync();
    }
}
