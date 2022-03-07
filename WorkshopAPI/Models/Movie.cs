using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshopAPI.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        //Movie-Category relationship
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public Category Category { get; set; }

    }
}
