using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalsApi.Domain.Entities;
using MinimalsApi.Domain.Interfaces;
using MinimalsApi.Infra.Database;

namespace MinimalsApi.Domain.Services
{
    public class VehicleServices : IVehicle
    {
        private readonly MinimalsContext _context;
        public VehicleServices(MinimalsContext context)
        {
            _context = context;
        }

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }

        public List<Vehicle> GetALL(int page = 1, string nome = null, string marca = null)
        {
            var query = _context.Vehicles.AsQueryable();
            if(!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome.ToLower()}%"));
            }

            

            if(!string.IsNullOrEmpty(marca))   
            {
                query = query.Where(v => EF.Functions.Like(v.Marca.ToLower(), $"%{marca.ToLower()}%"));
            }

            query = query
                .Skip((page - 1) * 10)
                .Take(10);

            return query.ToList();
        }

        public Vehicle? GetById(int id)
        {
            return _context.Vehicles.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Include(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }

        public void Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }

        internal object GetAll()
        {
            throw new NotImplementedException();
        }
    }
}