using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalsApi.Domain.Entities;
using MinimalsApi.Models;

namespace MinimalsApi.Infra.Interfaces
{
    public interface IAdminService
    {
        Admin Login(loginDTO loginDTO);
    }
}