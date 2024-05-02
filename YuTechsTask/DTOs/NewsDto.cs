using System.ComponentModel.DataAnnotations;
using YuTechsTask.Helpers;

namespace YuTechsTask.DTOs
{
    public class NewsDto
    {
        [Required]

        public string Title { get; set; }
        [Required]

        public string Newss { get; set; }
        [Required]
        [DateWithinRange(ErrorMessage = "Publication date must be between today and a week from today.")]
        public DateTime PublicationDate { get; set; }
        

        [Required]

        public int AuthorId { get; set; }
        [Required]

        public string FileName { get; set; }
        [Required]

        public string ContentType { get; set; }
        [Required]

        public string ImageData { get; set; }
    }
}
