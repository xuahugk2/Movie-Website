using DACN.Methods;
using DACN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACN.Models
{
    public class ModelMethod
    {
        public static Boolean AddUser(User user)
        {
            try
            {
                DACNEntities _db = new DACNEntities();
                _db.Users.Add(user);
                _db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
            
        }
        
        public static User GetUser(string email, string password)
        {
            try
            {
                DACNEntities _db = new DACNEntities();
                User user = _db.Users.FirstOrDefault(m => m.Email == email && m.Password == password);
                return user;
            }
            catch
            {
                return null;
            }
        }

        public static int getCountry(string name)
        {
            DACNEntities _db = new DACNEntities();
            Country cou = _db.Countries.FirstOrDefault(c => c.CountryName == name);
            return cou.Id;
        }

        public static SelectList getCountries()
        {
            DACNEntities _db = new DACNEntities();
            var list = _db.Countries.ToList();
            List<string> data = new List<string>();
            foreach(var item in list)
            {
                data.Add(item.CountryName);
            }
            return new SelectList(data);
        }

        public static SelectList getStatus()
        {
            var data = new List<string>();
            data.Add("Chưa phát sóng");
            data.Add("Đã phát sóng");
            return new SelectList(data);
        }

        public static SelectList getSeason()
        {
            var data = new List<string>();
            data.Add("Mùa xuân");
            data.Add("Mùa hạ");
            data.Add("Mùa thu");
            data.Add("Mùa đông");
            return new SelectList(data);
        }

        public static SelectList getYears()
        {
            int year = DateTime.Now.Year;
            int begin = year - 10, end = year + 5;
            var data = new List<int>();
            for(int i = begin; i <= end; i++)
            {
                data.Add(i);
            }
            return new SelectList(data);
        }

        public static List<Movie> getSameMovie(Boolean isAnime, Boolean isTheater, int id)
        {
            DACNEntities _db = new DACNEntities();
            var data = new List<Movie>();
            if (isAnime)
            {
                var listAnime = _db.Movies.Where(m => m.isAnime == isAnime && m.Id != id).ToList().Take(3);
                data.AddRange(listAnime);
            }
            if (isTheater)
            {
                var listTheater = _db.Movies.Where(m => m.isAnime == isTheater && m.Id != id).ToList().Take(3);
                data.AddRange(listTheater);
            }
            return data;
        }

        public static Boolean checkFollow(string user, int id)
        {
            DACNEntities _db = new DACNEntities();
            Follow f = _db.Follows.FirstOrDefault(m => m.UserEmail == user && m.MovieId == id);
            return (f != null);
        }
    }
}