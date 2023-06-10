using System.ComponentModel.DataAnnotations;

namespace Assignment.Contracts.Data.Entities
{
    public class AppDownload
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "App")]
        public virtual int AppId { get; set; }
        public virtual App? App { get; set; }
        public DateTime DownloadedDate { get; set; }
    }
}
