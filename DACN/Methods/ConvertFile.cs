using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DACN.Methods
{
    public class ConvertFile
    {
        public static string ToStringArray(HttpPostedFileBase file)
        {
            //File size: 98.2MB
            var length = file.InputStream.Length; //Length: 103050706
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target); // generates problem in this line
            byte[] data = target.ToArray();
            string result = Convert.ToBase64String(data);
            return result;
        }

        public static FileContentResult ToFileUrl(string data)
        {
            byte[] result = Convert.FromBase64String(data);
            return new FileContentResult(result, "image/jpg");
        }
    }
}