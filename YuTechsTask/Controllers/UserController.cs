using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YuTechsTask.DTOs;
using YuTechsTask.Models;

namespace YuTechsTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public UserController(ApplicationDBContext _context)
        {
            context = _context;
        }

        [HttpGet("news")]
        public IActionResult GetAllNews()
        {
            var news = context.News.ToList();

            List<NewsGetTitleImageDTO> newsGetDTOs = new List<NewsGetTitleImageDTO>();

            foreach (var neww in news)
            {
                if(neww.PublicationDate <= DateTime.Now )
                {
                    var img = context.Images.SingleOrDefault(i => i.Id == neww.ImageId);

                    newsGetDTOs.Add(
                    new NewsGetTitleImageDTO()
                    {
                        Id = neww.Id,
                        Title = neww.Title,
                        FileName = img.FileName,
                        ContentType = img.ContentType,
                        ImageData = img.ImageData
                    });
                }
                
            }
            return Ok(newsGetDTOs);
        }

        [HttpGet("news/{id}")]
        public IActionResult GetNewById(int id)
        {
            var neww = context.News.Find(id);

            var img = context.Images.SingleOrDefault(i => i.Id == neww.ImageId);

            NewsGetDTO newsGetDTO = new NewsGetDTO()
            {
                Id = neww.Id,
                Title = neww.Title,
                Newss = neww.Newss,
                PublicationDate = neww.PublicationDate,
                CreationDate = neww.CreationDate,
                AuthorId = neww.AuthorId,
                AuthorName = neww.Author.Name,
                FileName = img.FileName,
                ContentType = img.ContentType,
                ImageData = img.ImageData
            };

            return Ok(newsGetDTO);
        }
    }
}
