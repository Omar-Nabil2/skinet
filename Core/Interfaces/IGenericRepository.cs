using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetItemByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllItemsAsync();
        Task<bool> SaveChangesAsync();
        bool Exists(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
        Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);
    }
}