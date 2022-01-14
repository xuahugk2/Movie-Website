using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACN.Models;
using System.IO;

namespace DACN.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        DACNEntities _dbContext;

        public HomeController()
        {
            _dbContext = new DACNEntities();
        }

        public ActionResult Index()
        {
            List<Movie> data = _dbContext.Movies.ToList();
            return View(data);
        }

        public ActionResult Watcher(int? id)
        {
            Movie mov = _dbContext.Movies.FirstOrDefault(m => m.Id == id);
            string user = (string)Session["userEmail"];
            //Follow f = _dbContext.Follows.FirstOrDefault(m => m.MovieId == id && m.UserEmail == user);
            ViewBag.Error = "exist";
            return (mov != null) ? View(mov) : View("Index");
        }

        [HttpPost]
        public ActionResult Watcher(string data)
        {
            Movie mov = _dbContext.Movies.FirstOrDefault(m => m.Name.ToLower().Contains(data.ToLower()));
            return (mov != null) ? View(mov) : View("Index");
        }

        public ActionResult AddToFavorite(int movId, string strUrl)
        {
            if(Session["userEmail"] == null)
            {
                return Redirect(strUrl);
            }
            string user = (string)Session["userEmail"];
            if (user != null || user != "")
            {
                Follow follow = new Follow(user, movId, DateTime.Now.ToString("yyyy-MM-dd"));
                _dbContext.Follows.Add(follow);
                _dbContext.SaveChanges();
            }
            return Redirect(strUrl);
        }

        public ActionResult RemoveFromFavorite(int movId, string strUrl)
        {
            string user = (string)Session["userEmail"];
            Follow follow = _dbContext.Follows.FirstOrDefault(f => f.UserEmail == user && f.MovieId == movId);
            _dbContext.Follows.Remove(follow);
            _dbContext.SaveChanges();
            return Redirect(strUrl);
        }

        public ActionResult Classify(string type)
        {
            List<Movie> list;
            switch (type)
            {
                case "anime":
                    ViewBag.Message = "Phim hoạt hình";
                    list = _dbContext.Movies.Where(m => m.isAnime == true).ToList();
                    break;
                case "theater":
                    ViewBag.Message = "Phim chiếu rạp";
                    list = _dbContext.Movies.Where(m => m.isTheater == true).ToList();
                    break;
                default:
                    list = _dbContext.Movies.ToList();
                    break;
            }

            return View(list);
        }

        public ActionResult Favorite()
        {
            string user = Session["userEmail"] as string;
            List<Follow> follows = _dbContext.Follows.Where(f => f.UserEmail == user).ToList();
            List<Movie> data = new List<Movie>();
            foreach (var f in follows)
            {
                Movie mov = _dbContext.Movies.FirstOrDefault(m => m.Id == f.MovieId);
                data.Add(mov);
            }
            return View(data);
            //return View();
        }

        public ActionResult Stream()
        {
            return View();
        }

        public FileContentResult ToFileUrl(string path, Boolean isImage)
        {
            byte[] result = System.IO.File.ReadAllBytes(path);
            return (isImage) ? new FileContentResult(result, "image/jpg") : new FileContentResult(result, "video/mp4");
        }
    }
}