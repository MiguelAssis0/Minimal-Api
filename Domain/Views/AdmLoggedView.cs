using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalsApi.Domain.Views
{
    public class AdmLogged
    {

        public string Token { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Profile { get; set; } = default!;
    }
}