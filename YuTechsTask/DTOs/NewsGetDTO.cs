namespace YuTechsTask.DTOs
{
    public class NewsGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Newss { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ImageData { get; set; }
    }
}
