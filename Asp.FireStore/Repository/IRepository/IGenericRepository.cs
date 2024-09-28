using Asp.FireStore.Models;
using Firebase.Auth;
using System.Linq.Expressions;

namespace Asp.FireStore.Repository.IRepository
{
    public interface IGenericRepository<T>
    {
        Task AddAsync(string path, T item);
        Task AddAsyncWithSignUp(string path, T item, string id);
        Task<int> Count(string path);
        Task<int> Count(string path, Expression<Func<T, bool>> expression);
        Task<List<T>> GetAllAsync(string path);
        Task<List<T>> GetAllAsync(string path, Expression<Func<T, bool>> expression);
        Task<T> GetById(string path, string id);
        Task<T> GetById(string path, string id, Expression<Func<T, bool>> expression);
        Task<string> SignUp(string Email, string Password);
        Task Update(string path, string id, T item);
        Task<string> UploadImageAsync(string path, IFormFile imageFile);
    }
}