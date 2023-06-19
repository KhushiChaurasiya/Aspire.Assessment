using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Contracts.Data.Entities
{
    [ExcludeFromCodeCoverage]
    public class Logs
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Exception { get; set; }
        public string Logger { get; set; }
        public string Url { get; set; }
    }
}
