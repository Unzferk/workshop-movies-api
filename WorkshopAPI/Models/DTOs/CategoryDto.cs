using System.ComponentModel.DataAnnotations;

namespace WorkshopAPI.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Available { get; set; }
    }
}
