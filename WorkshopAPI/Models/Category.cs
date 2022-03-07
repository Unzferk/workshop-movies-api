using System.ComponentModel.DataAnnotations;

namespace WorkshopAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }
    }
}
