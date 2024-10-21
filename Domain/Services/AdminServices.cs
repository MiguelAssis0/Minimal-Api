using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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