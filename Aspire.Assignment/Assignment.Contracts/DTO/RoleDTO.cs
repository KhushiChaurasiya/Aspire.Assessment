using Assignment.Contracts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Contracts.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Rolename { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
