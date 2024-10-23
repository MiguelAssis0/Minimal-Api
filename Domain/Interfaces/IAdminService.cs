using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalsApi.Domain.DTO;
using MinimalsApi.Domain.Entities;
using MinimalsApi.Models;

namespace MinimalsApi.Infra.Interfaces
{
    public interface IAdminService
    {
        Admin Include(Admin admin);
        Admin? Login(loginDTO loginDTO);
        List<Admin> GetAll(int? page);
        List<Admin> GetAll();
    }
}