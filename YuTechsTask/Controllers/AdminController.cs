using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YuTechsTask.DTOs;
using YuTechsTask.Models;
using static System.Net.Mime.MediaTypeNames;

namespace YuTechsTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public AdminController(ApplicationDBContext _context)
        {
            context = _context;
        }

        #region Author

        [HttpGet("author")]
        public IActionResult GetAuthors()
        {
            List<Author> authors = context.Authors.ToList();

            List<AuthorGetDTO> authorsDto = new List<AuthorGetDTO>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorGetDTO() { Id = author.Id, Name = author.Name });
            }
            return Ok(authorsDto);
        }

        [HttpPost("author")]
        public IActionResult AddAuthor( [FromBody] AuthorDTO authorAddDTO)
        {
            if(ModelState.IsValid)
            {
                Author author = new Author() { Name = authorAddDTO.Name };
                context.Authors.Add(author);
                int res = context.SaveChanges();
                if(res>0)
                {
                    return Ok(new { message = "Author Created" });
                }
                else
                {
                    return BadRequest("SomeThing Wrong");
                }


            }
            return BadRequest(ModelState);
        }

        [HttpPut("author/{id}")]
        public IActionResult EditAuthor(int id, [FromBody] AuthorDTO authorDTO)
        {
            if (ModelState.IsValid)
            {
                var author = context.Authors.Find(id);

                if (author != null)
                {
                    author.Name = authorDTO.Name;
                    context.Authors.Update(author);
                    int res = context.SaveChanges();
                    if (res > 0)
                    {
                        return Ok(new { message = "Author Updated" });
                    }
                    else
                    {
                        return BadRequest("SomeThing Wrong");
                    }
                }
                else
                {
                    return BadRequest("SomeThing Wrong");
                }

            }
            return BadRequest(ModelState);
        }

        [HttpDelete("author/{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = context.Authors.Find(id);

            if (author != null)
            {
                context.Authors.Remove(author);
                int res = context.SaveChanges();
                if (res > 0)
                {
                    return Ok(new { message = "Author Deleted" });
                }
                else
                {
                    return BadRequest("SomeThing Wrong");
                }
            }
            else
            {
                return BadRequest("SomeThing Wrong");
            }
        }

        #endregion

        #region News

        [HttpPost("news")]
        public IActionResult AddNews([FromBody] NewsDto newsAddDTO)
        {
            if (ModelState.IsValid)
            {
                using var transaction = context.Database.BeginTransaction();

                try
                {
                    Models.Image image = new Models.Image()
                    {
                        FileName = newsAddDTO.FileName,
                        ContentType = newsAddDTO.ContentType,
                        ImageData = newsAddDTO.ImageData
                    };

                    context.Images.Add(image);
                    context.SaveChanges();

                    News news = new News()
                    {
                        Title = newsAddDTO.Title,
                        Newss = newsAddDTO.Newss,
                        CreationDate = DateTime.Now,
                        PublicationDate = newsAddDTO.PublicationDate,
                        AuthorId = newsAddDTO.AuthorId,
                        ImageId = image.Id,
                    };

                    context.News.Add(news);


                    context.SaveChanges();
                    transaction.Commit();

                    return Ok(new { message = "News Created" });

                }
                catch (Exception)
                {

                    transaction.Rollback();
                    return BadRequest("Something Wrong");
                }

            }
            return BadRequest(ModelState);
        }


        [HttpPut("news/{id}")]
        public IActionResult EditNews(int id, [FromBody] NewsDto newsDto)
        {
            if (ModelState.IsValid)
            {
                using var transaction = context.Database.BeginTransaction();

                try
                {
                    var news = context.News.Find(id);
                    
                    var image = context.Images.SingleOrDefault(i=>i.Id==news.ImageId);
                    image.FileName = newsDto.FileName;
                    image.ContentType = newsDto.ContentType;
                    image.ImageData = newsDto.ImageData;
                    context.Images.Update(image);


                    news.Title = newsDto.Title;
                    news.Newss = newsDto.Newss;
                    news.PublicationDate = newsDto.PublicationDate;
                    news.AuthorId = newsDto.AuthorId;
                    news.ImageId = image.Id;
                    context.News.Update(news);


                    context.SaveChanges();
                    transaction.Commit();

                    return Ok(new { message = "News updated" });

                }
                catch (Exception)
                {

                    transaction.Rollback();
                    return BadRequest("Something Wrong");
                }

            }
            return BadRequest(ModelState);

        }

        
        [HttpDelete("news/{id}")]
        public IActionResult DeleteNews(int id)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var news = context.News.Find(id);
                context.News.Remove(news);
                var image = context.Images.SingleOrDefault(i=>i.Id==news.ImageId);
                context.Images.Remove(image);



                context.SaveChanges();
                transaction.Commit();

                return Ok(new { message = "News Deleted" });

            }
            catch (Exception)
            {

                transaction.Rollback();
                return BadRequest("Something Wrong");
            }


        }

        //------------------------------------------------ Get All
        [HttpGet("news")]
        public IActionResult GetAllNews()
        {
            var news = context.News.ToList();

            List<NewsGetDTO> newsGetDTOs = new List<NewsGetDTO>();

            foreach (var neww in news)
            {
                var img = context.Images.SingleOrDefault(i=>i.Id == neww.ImageId);

                newsGetDTOs.Add(
                new NewsGetDTO()
                {
                    Id = neww.Id,
                    Title = neww.Title,
                    Newss = neww.Newss,
                    CreationDate = neww.CreationDate,
                    PublicationDate = neww.PublicationDate,
                    AuthorId = neww.AuthorId,
                    AuthorName = neww.Author.Name,
                    FileName = img.FileName,
                    ContentType = img.ContentType,
                    ImageData = img.ImageData
                });
            }
            return Ok(newsGetDTOs);

        }
        #endregion

    }
}
