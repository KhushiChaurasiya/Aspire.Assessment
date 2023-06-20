using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Contracts.DTO
{
    [ExcludeFromCodeCoverage]
    public class ExternalAuthDTO
    {
        public string? provider { get; set; }
        public string? idToken { get; set; }
    }
}
