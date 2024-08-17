using System.ComponentModel.DataAnnotations;

namespace RuntimeComposition.NET.Web.Models
{
    public class HomeViewModel
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required IEnumerable<CustomSelectList> Somethings { get; set; }

        [Required]
        public required string Chosen { get; set; }
    }
}
