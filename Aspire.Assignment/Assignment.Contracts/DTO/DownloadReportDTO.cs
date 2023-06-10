using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Contracts.DTO
{
    public class DownloadReportDTO
    {
        public int NumberOfDownloads { get; set; }
        public string AppName { get; set; }
        public DateTime DownlodedDate { get; set; }
    }
}
