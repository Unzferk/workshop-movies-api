using System.ComponentModel.DataAnnotations;

namespace WorkshopAPI.Models.DTOs
{
    public class MovieDto
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }


        //Movie-Category relationship
        public int categoryId { get; set; }
        public Category Category { get; set; }
    }

}
