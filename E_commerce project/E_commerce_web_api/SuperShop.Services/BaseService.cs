using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperShop.Common;
using SuperShop.Model.DBEntity;
using SuperShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public BaseService(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<T> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "")
        {
            return _repository.Get(filter, orderBy, includeProperties);
        }

        public async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            return await _repository.GetAsync(filter, orderBy, includeProperties);
        }

        public Task<(IEnumerable<T> Data, int TotalCount)> GetPaginateAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int pageNumber = 1,
            int pageSize = 10)
        {
            return _repository.GetPaginateAsync(filter, orderBy, includeProperties, pageNumber, pageSize);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }
        public IEnumerable<T> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            return _repository.GetAll(pageNumber, pageSize);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize);
        }
        public IEnumerable<T> GetAllIncluding(params string[] includeProperties)
        {
            return _repository.GetAllIncluding(includeProperties);
        }

        public IEnumerable<T> GetAllSorted<TType>(Expression<Func<T, TType>> sortCondition, bool sortDesc)
        {
            return _repository.GetAllSorted(sortCondition, sortDesc);
        }
        public T GetById(Int64 id)
        {
            return _repository.GetById(id);
        }
        public async Task<T> GetByIdAsync(Int64 id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public T GetByIdIncluding(object id, params string[] includeProperties)
        {
            return _repository.GetByIdIncluding(id, includeProperties);
        }
        public OperationResult Add(T entity, string idenityUser)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.CreatedDate = DateTime.Now;
                baseEntity.IsRemoved = false;
                baseEntity.IsActive = true;
                baseEntity.CreatedBy = Convert.ToInt64(idenityUser);
            }
            _repository.Add(entity);
            OperationResult result = _unitOfWork.Save();
            result.Result = entity;
            return result;
        }
        public async Task<OperationResult> AddAsync(T entity, string idenityUser)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.CreatedDate = DateTime.Now;
                baseEntity.IsRemoved = false;
                baseEntity.IsActive = true;
                baseEntity.CreatedBy = idenityUser != null ? Convert.ToInt64(idenityUser) : null;
            }
            await _repository.AddAsync(entity);
            OperationResult result = await _unitOfWork.SaveAsync();
            result.Result = entity;
            return result;
        }
		public async Task<OperationResult> AddRangeAsync(IEnumerable<T> entity, string idenityUser)
		{
			foreach(var entityItem in entity)
            {
				if (entityItem is BaseEntity baseEntity)
				{
					baseEntity.CreatedDate = DateTime.Now;
					baseEntity.IsRemoved = false;
					baseEntity.IsActive = true;
					baseEntity.CreatedBy = idenityUser != null ? Convert.ToInt64(idenityUser) : null;
				}
			}
			await _repository.AddRangeAsync(entity);
			OperationResult result = await _unitOfWork.SaveAsync();
			result.Result = entity;
			return result;
		}
		public async Task<OperationResult> Update(T entityToUpdate, string idenityUser)
        {
            if (entityToUpdate is BaseEntity baseEntity)
            {
                baseEntity.UpdatedDate = DateTime.Now;
                baseEntity.UpdatedBy = idenityUser != null ? Convert.ToInt64(idenityUser) : null;
            }
            return await _repository.Update(entityToUpdate);
            //OperationResult result = _unitOfWork.Save();
            //result.Result = entityToUpdate;
            //return _repository.Update(entityToUpdate);
        }
        public async Task<OperationResult> UpdateIsRemoveTrue(long entityId, string identityUser)
        {
            await _repository.UpdateIsRemoveTrue(entityId, identityUser);
            return await _unitOfWork.SaveAsync();
        }
        public async Task<OperationResult> UpdateIsRemoveTrueRange(List<T> entitiesToUpdate, string identityUser)
        {
            await _repository.UpdateIsRemoveTrueRange(entitiesToUpdate, identityUser);
            return await _unitOfWork.SaveAsync();
        }
        public OperationResult Delete(object id)
        {
            _repository.Delete(id);
            return _unitOfWork.Save();
        }
        public OperationResult Delete(T entityToDelete)
        {
            _repository.Delete(entityToDelete);
            return _unitOfWork.Save();
        }
        public IEnumerable<T> Where(Expression<Func<T, bool>> condition)
        {
            return _repository.Where(condition);
        }
        public IQueryable<T> WhereAsync(Expression<Func<T, bool>> condition)
        {
            return _repository.WhereAsync(condition);
        }
        public bool Any(Expression<Func<T, bool>> condition)
        {
            return _repository.Any(condition);
        }
        public Task<bool> AnyAsync(Expression<Func<T, bool>> condition)
        {
            return _repository.AnyAsync(condition);
        }
        public OperationResult Remove(T entity)
        {
            _repository.Remove(entity);
            return _unitOfWork.Save();  
        }
        public OperationResult RemoveRange(IEnumerable<T> entityList)
        {
            _repository.RemoveRange(entityList);
            return _unitOfWork.Save();
        }

    }

    public interface IBaseService<T> where T : class
    {
        IEnumerable<T> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");
        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        Task<(IEnumerable<T> Data, int TotalCount)> GetPaginateAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int pageNumber = 1,
            int pageSize = 10);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        IEnumerable<T> GetAllIncluding(params string[] includeProperties);
        IEnumerable<T> GetAllSorted<TType>(Expression<Func<T, TType>> sortCondition, bool sortDesc);
        //T GetById(Int64 id);
        T GetById(Int64 id);
        Task<T> GetByIdAsync(Int64 id);
        T GetByIdIncluding(object id, params string[] includeProperties);
        OperationResult Add(T entity, string identityUser);
        Task<OperationResult> AddAsync(T entity, string idenityUser);
        Task<OperationResult> AddRangeAsync(IEnumerable<T> entity, string idenityUser);
        Task<OperationResult> Update(T entityToUpdate, string identityUser);
        Task<OperationResult> UpdateIsRemoveTrue(long entityId, string identityUser);
        Task<OperationResult> UpdateIsRemoveTrueRange(List<T> entitiesToUpdate, string identityUser);
        OperationResult Delete(object id);
        OperationResult Delete(T entityToDelete);
        IEnumerable<T> Where(Expression<Func<T, bool>> condition);
        IQueryable<T> WhereAsync(Expression<Func<T, bool>> condition);
        bool Any(Expression<Func<T, bool>> condition);
        Task<bool> AnyAsync(Expression<Func<T, bool>> condition);
        OperationResult Remove(T entity);
        OperationResult RemoveRange(IEnumerable<T> entityList);
    }
}
