using System.ComponentModel.DataAnnotations.Schema;

namespace YuTechsTask.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Newss { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; }



        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Author  { get; set; }

        public int ImageId { get; set; }
        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

    }
}
