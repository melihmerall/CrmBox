using System.Linq.Expressions;
using CrmBox.Core.Domain.Base;

namespace CrmBox.Application.Interfaces;


public interface IGenericService<TEntity>
        where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteAsync(int id);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);
        TEntity? Get(Expression<Func<TEntity, bool>> filter);
        

    }
