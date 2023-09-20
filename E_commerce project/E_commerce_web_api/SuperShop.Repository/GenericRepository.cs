using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using SuperShop.Common;
using SuperShop.Common.Constant;
using SuperShop.Model.Data;
using SuperShop.Model.DBEntity;
using SuperShop.Model.DBEntity.Customers;
using System.Linq;
using System.Linq.Expressions;

namespace SuperShop.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<TEntity> DbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }
        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // Add AsNoTracking() here to avoid change tracking
            query = query.AsNoTracking();

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet.Where(IgnoreIsRemoved());

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Add AsNoTracking() here to avoid change tracking
            query = query.AsNoTracking();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            int totalCount = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var data = await query.ToListAsync();

            return data;
        }
        public virtual async Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPaginateAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int pageNumber = 1,
            int pageSize = 10
            )
        {
            IQueryable<TEntity> query = DbSet.Where(IgnoreIsRemoved());

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Add AsNoTracking() here to avoid change tracking
            query = query.AsNoTracking();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            int totalCount = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var data = await query.ToListAsync();

            return (data, totalCount);
        }
        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.AsNoTracking();
        }
        public virtual IEnumerable<TEntity> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            // Calculate the number of items to skip based on the page number and page size
            int itemsToSkip = (pageNumber - 1) * pageSize;

            // Query the database to retrieve the desired page of data
            return DbSet
                .AsNoTracking()
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToList();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            // Calculate the number of items to skip based on the page number and page size
            int itemsToSkip = (pageNumber - 1) * pageSize;

            // Query the database to retrieve the desired page of data
            return await DbSet
                .AsNoTracking()
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToListAsync();
        }
        public virtual IEnumerable<TEntity> GetAllIncluding(params string[] includeProperties)
        {
            //IQueryable<TEntity> query = DbSet.Where(IgnoreIsRemoved());
            IQueryable<TEntity> query = DbSet;
            if (query != null)
            {
                if (query.ToList().Count() > 0)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        query.Include(includeProperty);
                    }
                }
            }

            // Apply AsNoTracking() to avoid change tracking
            query = query.AsNoTracking();

            return query;
        }
        public virtual IEnumerable<TEntity> GetAllSorted<TType>(Expression<Func<TEntity, TType>> sortCondition, bool sortDesc)
        {
            if (sortDesc)
            {
                //return DbSet.Where(IgnoreIsRemoved()).OrderBy(sortCondition);
                return DbSet.AsNoTracking().OrderByDescending(sortCondition);
            }
            else
            {
                //return DbSet.Where(IgnoreIsRemoved()).OrderBy(sortCondition);
                return DbSet.AsNoTracking().OrderBy(sortCondition);
            }
        }
        public virtual TEntity GetById(Int64 id)
        {
            return DbSet.Find(id);
        }
        public virtual async Task<TEntity> GetByIdAsync(Int64 id)
        {
            return await DbSet.FindAsync(id);
        }
        public virtual TEntity GetByIdIncluding(object id, params string[] includeProperties)
        {

            var model = DbSet.Find(id);

            if (model != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    _context.Entry(model).Collection(includeProperty.ToString()).Load();
                }
            }

            return model;
        }
        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }
        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return true;
        }
		public virtual async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
            return true;
        }
		public virtual async Task<OperationResult> Update(TEntity entityToUpdate)
        {
            
            try
            {
                //DbSet.Attach(entityToUpdate);
                //DbSet.Entry(entityToUpdate).State = EntityState.Modified;

                DbSet.Update(entityToUpdate);
                await _context.SaveChangesAsync();
                return new OperationResult(true, entityToUpdate, ApiResponseMessage.Update_success);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, entityToUpdate, ex.Message);
            }
        }
        public virtual async Task UpdateIsRemoveTrue(long entityId, string identityUser)
        {
            try
            {
                var entityToUpdate = await _context.Set<TEntity>().FindAsync(entityId);

                if (entityToUpdate != null)
                {
                    _context.Entry(entityToUpdate).Property("IsRemoved").CurrentValue = true;
                    _context.Entry(entityToUpdate).Property("UpdatedBy").CurrentValue = Convert.ToInt64(identityUser);
                    _context.Entry(entityToUpdate).Property("UpdatedDate").CurrentValue = DateTime.Now;

                }
                else
                {
                    // Handle the case where the entity with the specified ID was not found.
                    // You may throw an exception, log a warning, or take appropriate action.
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public virtual async Task UpdateIsRemoveTrueRange(List<TEntity> entitiesToUpdate, string identityUser)
        {
            try
            {
                var idProperty = typeof(TEntity).GetProperty("Id");

                foreach (var entityToUpdate in entitiesToUpdate)
                {
                    var entityId = idProperty.GetValue(entityToUpdate);

                    var entityInDb = await _context.Set<TEntity>().FindAsync(entityId);

                    if (entityInDb != null)
                    {
                        _context.Entry(entityInDb).Property("IsRemoved").CurrentValue = true;
                        _context.Entry(entityInDb).Property("UpdatedBy").CurrentValue = Convert.ToInt64(identityUser);
                        _context.Entry(entityInDb).Property("UpdatedDate").CurrentValue = DateTime.Now;
                    }
                    else
                    {
                        // Handle the case where the entity with the specified ID was not found.
                        // You may throw an exception, log a warning, or take appropriate action.
                    }
                }
            }
            catch(Exception er)
            {
                throw er;
            }
        }
        public virtual void Delete(object id)
        {
            Delete(DbSet.Find(id));
        }
        public virtual bool Delete(TEntity entityToDelete)
        {
            bool isSuccess = false;
            try
            {
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    DbSet.Attach(entityToDelete);
                }
                DbSet.Remove(entityToDelete);
                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = true;
            }
            return isSuccess;
        }
        public virtual IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> condition)
        {
            return DbSet.Where(condition);
        }
        public virtual IQueryable<TEntity> WhereAsync(Expression<Func<TEntity, bool>> condition)
        {
            return DbSet.Where(condition);
        }
        public virtual bool Any(Expression<Func<TEntity, bool>> condition)
        {
            return DbSet.Any(condition);
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await DbSet.AnyAsync(condition);
        }
        public virtual void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        public virtual void RemoveRange(IEnumerable<TEntity> entityList)
        {
            DbSet.RemoveRange(entityList);
        }
        public Expression<Func<TEntity, bool>> IgnoreIsRemoved()
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var property = Expression.Property(parameter, "IsRemoved");
            var equal = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
            return lambda;
        }
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "");
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPaginateAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int pageNumber = 1,
            int pageSize = 10);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        IEnumerable<TEntity> GetAllIncluding(params string[] includeProperties);
        IEnumerable<TEntity> GetAllSorted<TType>(Expression<Func<TEntity, TType>> sortCondition, bool sortDesc);
        //TEntity GetById(Int64 id);
        TEntity GetById(Int64 id);
        Task<TEntity> GetByIdAsync(Int64 id);
        TEntity GetByIdIncluding(object id, params string[] includeProperties);
        void Add(TEntity entity);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync (IEnumerable<TEntity> entities);
        Task<OperationResult> Update(TEntity entity);
        Task UpdateIsRemoveTrue(long entityId, string identityUser);
        Task UpdateIsRemoveTrueRange(List<TEntity> entitiesToUpdate, string identityUser);
        void Delete(object id);
        bool Delete(TEntity entityToDelete);
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> condition);
        IQueryable<TEntity> WhereAsync(Expression<Func<TEntity, bool>> condition);
        bool Any(Expression<Func<TEntity, bool>> condition);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entityList);


        //OperationResult<TEntity> ActiveInactive(TEntity entity);
        //OperationResult<TEntity> SoftDelete(TEntity entity);
    }
}