using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Contracts.Data.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string? Rolename { get; set; }

        public ICollection<User>? User { get; set; }
    }
}
