using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Contracts.Data.Entities
{
    public class ErrorLog
    {
        [Key]
        public int Id { get; set; }
        public string? LogLevel { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
