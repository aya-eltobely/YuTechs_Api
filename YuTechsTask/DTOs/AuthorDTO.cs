using System.ComponentModel.DataAnnotations;

namespace YuTechsTask.DTOs
{
    public class AuthorDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
