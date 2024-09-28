using Asp.FireStore.Models;
using Asp.FireStore.Repository.IRepository;

namespace Asp.FireStore.Repository.Implementation
{
    public class VehicleRepository : GenericRepository<Vehicle> , IVehicleRepository
    {
    }
}
