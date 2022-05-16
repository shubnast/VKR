using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataBaseProvider
{
    /// <summary>
    /// CRUD Репозиторий для удобства работы с отдельными таблицами
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        void Create(T value);
        void Create(IEnumerable<T> values);

        IEnumerable<T> Read();
        T Read(Func<IQueryable<T>, T> action);
        IEnumerable<T> Read(Func<IQueryable<T>, IEnumerable<T>> action);
        Task<T> ReadAsync(Func<IQueryable<T>, Task<T>> action);
        Task<IEnumerable<T>> ReadAsync(Func<IQueryable<T>, Task<IEnumerable<T>>> action);

        void Update(T value);

        void Delete(T value);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        IDbContextTransaction BeginTransaction();

        void CommitTransaction();
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        void RollbackTransaction();
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        bool SaveChanges();
        Task<bool> SaveChangesAsync();
    }
}
