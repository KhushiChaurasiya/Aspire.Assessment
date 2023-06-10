using System.ComponentModel.DataAnnotations;

namespace Assignment.Contracts.Data.Entities
{
    public class App : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Files { get; set; }
    }
}