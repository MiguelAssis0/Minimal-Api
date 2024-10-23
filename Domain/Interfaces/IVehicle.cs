using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalsApi.Domain.Entities;

namespace MinimalsApi.Domain.Interfaces
{
    public interface IVehicle
    {
        List<Vehicle> GetALL(int page = 1, string? nome = null, string? marca = null);
        Vehicle? GetById(int id);
        void Include(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);
    }
}