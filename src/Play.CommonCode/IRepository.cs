using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.CommonCode
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateItemAsync(T item);
        Task DeleteItemAsync(Guid Id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid Id);
        Task UpdateItemAsync(T item);
    }
}
