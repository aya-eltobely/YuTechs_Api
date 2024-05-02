namespace YuTechsTask.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }



        public virtual List<News> News { get; set; }
    }
}
