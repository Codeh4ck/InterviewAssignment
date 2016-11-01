using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataStorage.Database;
using DataStorage.Models;

namespace DataStorage.Controllers
{
    public class NewsController : ApiController
    {  
        private readonly DatabaseContext db = DatabaseContext.Create();

        // GET api/News  
        public JsonResult<NewsDataModel[]> Get()
        {
            var Tweets = db.NewsData.Where(t => t.Provider == "Twitter").ToArray();
            return Json(Tweets.ToArray());
        }

        public JsonResult<NewsDataModel[]> Get(int id)
        {
            var News = db.NewsData.OrderBy(u => u.Date).Take(id).ToArray();
            return Json(News);
        }

        public JsonResult<NewsDataModel[]> Get(int id, string provider)
        {
            var News = db.NewsData.Where(n => n.Provider == provider).OrderBy(u => u.Date).Take(id).ToArray();
            return Json(News);
        }

        // POST api/News        
        public async Task<IHttpActionResult> Post(NewsDataModel value)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model state is invalid!");

            try
            {
                db.NewsData.AddOrUpdate(i => new { i.Poster, i.Message, i.Provider}, value);
                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
