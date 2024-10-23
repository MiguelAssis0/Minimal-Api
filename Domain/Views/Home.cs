using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalsApi.Domain.Views
{
    public struct Home
    {
        public string Documantation { get => "/swagger"; }
        public string Message { get => "Welcome to the vehicles API - Minimals"; }
    }
}