using NewsEngine2._0.Dto.MediaDto;
using NewsEngine2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsEngine2._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    ViewData["Category"] = GetAllCategories();

        //}

         
        public ActionResult Index()
        {
            var first = db.News.Where(p => p.IsProposed.Equals(false)).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            ViewBag.First = new MediaDto
            {
                News = first,
                Medias = db.Media.Where(x => x.NewsId == first.NewsId).ToList()
            };
            var news = db.News.Where(p => p.IsProposed.Equals(false)).OrderByDescending(x => x.CreateDate).Skip(1).Take(3);
            List<MediaDto> article = new List<MediaDto>();
            foreach(News item in news)
            {
                article.Add(new MediaDto
                {
                    News = item,
                    Medias = db.Media.Where(x => x.NewsId == item.NewsId).ToList()
                });
            }
            ViewBag.Article = article;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name.ToString()
                });
            }

            // returnam lista de categorii
            return selectList;
        }
    }
}