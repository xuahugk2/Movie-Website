using DACN.Methods;
using DACN.Models;
using DACN.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;

namespace DACN.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private DACNEntities _db;
        public int _page = 0;

        public AdminController()
        {
            _db = new DACNEntities();
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            if (Session["adminId"] != null)
            {
                return View(_db.Users.Where(m=>m.isAdmin == false).ToList().ToPagedList(pageNumber, 5));
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult Upgrade(string email)
        {
            User user = _db.Users.FirstOrDefault(u => u.Email == email);
            if(user != null)
            {
                user.isAdmin = true;
                _db.SaveChanges();
            }
            return View("Index");
        }

        public ActionResult DeleteUser(string email)
        {
            User user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult UploadMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadMovie(UploadViewModel model, HttpPostedFileBase image, HttpPostedFileBase video, HttpPostedFileBase thumbnail)
        {
            string Image = getFilePath(image, true);
            string Trailer = getFilePath(video, false);
            string Thumbnail = getFilePath(thumbnail, true);
            try
            {
                //Movie movie = new Movie(model.Name, model.Anime, model.Theater, model.CountryName, model.YearRelease, model.Status, model.Content, uploadFile(image, true), uploadFile(video, false), uploadFile(thumbnail, true), model.Season);
                Movie movie = new Movie(model.Name, model.Anime, model.Theater, model.CountryName, model.YearRelease, model.Status, model.Content, Image, Trailer, Thumbnail, model.Season);
                _db.Movies.Add(movie);
                _db.SaveChangesAsync();
                ViewBag.Message = "Thêm thành công";
            } catch(Exception e)
            {
                ViewBag.Message = "Thêm không thành công. Lỗi: " + e.ToString();
            }
            return View();
        }

        [HttpGet]
        public ActionResult Manage(int? page)
        {
            _page = page ?? 1;
            int pageSize = 5;
            return View(_db.Movies.ToList().OrderBy(m => m.Id).ToPagedList(_page, pageSize));
        }

        public string getFilePath(HttpPostedFileBase file, Boolean isImage)
        {
            string datetime = DateTime.Now.ToString("ddMMyyyyHHMMss");
            string finalPath = (isImage) ? "\\Upload\\Images\\" : "\\Upload\\Videos\\";
            finalPath += (datetime + file.FileName);
            file.SaveAs(Server.MapPath("~") + finalPath);
            return finalPath;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ViewModels.LoginViewModel model)
        {
            User admin = _db.Users.FirstOrDefault(a => a.Email == model.UserName && a.Password == model.Password);
            if(admin != null && admin.isAdmin == true)
            {
                Session["adminId"] = "Trang quản trị";
                return View("Index");
            }
            else
            {
                ViewBag.Message = "Thông tin đăng nhập sai";
                return View(model);
            }
        }
        
        public ActionResult LogOut()
        {
            Session.Clear();
            return View("Login");
        }

        public ActionResult DeleteAsync(int id, string strURL)
        {
            Movie mov = _db.Movies.FirstOrDefault(m => m.Id == id);
            _db.Movies.Remove(mov);
            _db.SaveChanges();
            return Redirect(strURL);
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            Movie mov = _db.Movies.Find(search);
            if(mov != null)
            {
                return View("Update", mov.Id);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            Movie mov = _db.Movies.FirstOrDefault(m => m.Id == id);
            UploadViewModel data = new UploadViewModel()
            {
                Id = mov.Id,
                Name = mov.Name,
                Anime = mov.isAnime,
                Theater = mov.isTheater,
                CountryName = mov.Country.CountryName,
                YearRelease = mov.YearRelease,
                Status = mov.Status,
                Content = mov.Content,
                Season = mov.Season

            };
            return View(data);
        }

        [HttpPost]
        public ActionResult Update(int id, UploadViewModel update)
        {
            try
            {
                Movie mov = _db.Movies.FirstOrDefault(m => m.Id == id);
                mov.Name = update.Name;
                mov.isAnime = update.Anime;
                mov.isTheater = update.Theater;
                mov.CountryId = ModelMethod.getCountry(update.CountryName);
                mov.YearRelease = update.YearRelease;
                mov.Status = update.Status;
                mov.Content = update.Content;
                mov.Season = update.Season;
                _db.SaveChanges();
                ViewBag.Message = "Chỉnh sửa thành công";
            }
            catch
            {
                ViewBag.Message = "Chỉnh sửa thất bại";
            }
            
            return View(update);
        }
    }
}