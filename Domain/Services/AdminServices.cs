using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalsApi.Domain.DTO;
using MinimalsApi.Domain.Entities;
using MinimalsApi.Infra.Database;
using MinimalsApi.Infra.Interfaces;
using MinimalsApi.Models;

namespace MinimalsApi.Domain.Services
{
    public class AdminServices : IAdminService
    {
        private readonly MinimalsContext _context;

        public AdminServices(MinimalsContext context)
        {
            _context = context;
        }

        public List<Admin> GetAll(int? page)
        {
            var query = _context.Admins.AsQueryable();
            if (page.HasValue)
                query = query.Skip((page.Value - 1) * 10).Take(10);
            return query.ToList();
        }

        public List<Admin> GetAll()
        {
            return _context.Admins.ToList();
        }

        public Admin Include(Admin admin)
        {
            _context.Admins.Add(new Admin
            {
                Email = admin.Email,
                Password = admin.Password,
                Profile = admin.Profile
            });
            _context.SaveChanges();
            return _context.Admins.Where(x => x.Email == admin.Email && x.Password == admin.Password).FirstOrDefault();
        }

        public Admin Login(loginDTO loginDTO)
        {
            var qtt = _context.Admins.Where(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password).Count();
            if(qtt > 0)
                return _context.Admins.Where(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password).FirstOrDefault();
            else
                return null;
        }
        
    }
}