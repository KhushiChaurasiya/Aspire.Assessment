using Assignment.Contracts.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Assignment.Contracts.DTO
{
    public class AppDownloadDTO
    {
        public int Id { get; set; }
        public virtual int AppId { get; set; }
        public DateOnly DownloadedDate { get; set; }
        //public DateTime AddedOn { get; set; }
    }
}
