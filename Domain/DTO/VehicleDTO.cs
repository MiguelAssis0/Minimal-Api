using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalsApi.Domain.DTO
{
    public record VehicleDTO
    {
        public string Nome { get; set; } = default;
        public string Marca { get; set; } = default;
        public int Ano { get; set; } = default;
    }
}