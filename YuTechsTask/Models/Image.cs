﻿namespace YuTechsTask.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ImageData { get; set; }


        public virtual News News { get; set; }
    }
}
