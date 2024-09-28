using Asp.FireStore.Models;
using Asp.FireStore.Repository.IRepository;
using Google.Cloud.Firestore;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

namespace Asp.FireStore.Repository.Implementation
{
    public class UsersRepository : GenericRepository<Users>, IUsersRepository
    {
    }
}
