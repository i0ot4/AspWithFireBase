using Asp.FireStore.Models;
using Asp.FireStore.Repository.IRepository;
using Google.Cloud.Firestore;
using System.Linq.Expressions;

namespace Asp.FireStore.Repository.Implementation
{
    public class RequestPoliceRepository : GenericRepository<RequestPolice>, IRequestPoliceRepository
    {

    }
}
