using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DACN.Models;


namespace DACN.ViewModels
{
    public class UploadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Anime { get; set; }
        public bool Theater { get; set; }
        public string CountryName { get; set; }
        public int YearRelease { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public string Season { get; set; }

    }
}