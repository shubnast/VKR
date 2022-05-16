using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataBaseProvider
{
    public class AbstractRepository<T> : IEnumerable<T>, IRepository<T> where T : class
    {
        private ApplicatonDBContext _applicatonDBContext;

        private DbSet<T> _currentSet;

        public AbstractRepository(ApplicatonDBContext context, DbSet<T> dbSet)
        {
            _applicatonDBContext = context;
            _currentSet = dbSet;
        }

        public virtual void Create(T value)
        {
            _currentSet.Add(value);
        }

        public virtual void Create(IEnumerable<T> values)
        {
            _currentSet.AddRange(values);
        }

        public virtual IEnumerable<T> Read()
        {
            return _currentSet;
        }

        public virtual T Read(Func<IQueryable<T>, T> action)
        {
            return action(_currentSet);
        }

        public virtual IEnumerable<T> Read(Func<IQueryable<T>, IEnumerable<T>> action)
        {
            return action(_currentSet);
        }

        public virtual async Task<T> ReadAsync(Func<IQueryable<T>, Task<T>> action)
        {
            return await action(_currentSet);
        }

        public virtual async Task<IEnumerable<T>> ReadAsync(Func<IQueryable<T>, Task<IEnumerable<T>>> action)
        {
            return await action(_currentSet);
        }

        public virtual void Update(T value)
        {
            _currentSet.Attach(value).State = EntityState.Modified;
        }

        public virtual void Delete(T value)
        {
            _currentSet.Remove(value);
        }

        public virtual bool SaveChanges()
        {
            return _applicatonDBContext.SaveChanges() > 0;
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return await _applicatonDBContext.SaveChangesAsync() > 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T val in _currentSet)
            {
                yield return val;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _applicatonDBContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _applicatonDBContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _applicatonDBContext.Database.CommitTransaction();
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _applicatonDBContext.Database.CommitTransactionAsync(cancellationToken);
        }

        public void RollbackTransaction()
        {
            _applicatonDBContext.Database.RollbackTransaction();
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _applicatonDBContext.Database.RollbackTransactionAsync(cancellationToken);
        }
    }
}
