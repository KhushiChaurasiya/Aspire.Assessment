using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Contracts.DTO
{
    public class LoginResponseDTO
    {
        public string? token { get; set; }
        public string? Role { get; set; }
        public string? UserName { get; set; }
    }
}
