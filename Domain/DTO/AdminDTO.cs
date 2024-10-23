using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalsApi.Domain.Enuns;

namespace MinimalsApi.Domain.DTO
{
    public class AdminDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Profile? Profile { get; set; }
    }
}