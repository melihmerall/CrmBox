using System.Linq.Expressions;
using CrmBox.Application.Interfaces;
using CrmBox.Core.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrmBox.Application.Services;

public class GenericService<TEntity, TContext> : IGenericService<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        readonly TContext _context;

        public GenericService(TContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = await _context.Set<TEntity>().AddAsync(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            TEntity? model = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            EntityEntry<TEntity> entityEntry = _context.Set<TEntity>().Remove(model);
            _context.SaveChanges();
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().FirstOrDefault(filter);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>()
                : _context.Set<TEntity>().Where(filter);
        }

        public void Update(TEntity entity)
        {
        //EntityEntry<TEntity> entityEntry = _context.Set<TEntity>().Update(entity);
        //_context.SaveChanges();
      

        var updateEntity = _context.Entry(entity);
        updateEntity.State = EntityState.Modified;
        _context.SaveChanges();
    }
    }
