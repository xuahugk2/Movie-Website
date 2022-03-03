using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DACN.Models;
using DACN.Methods;

namespace DACN.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private DACNEntities _dbContext; 

        public AccountController()
        {
            _dbContext = new DACNEntities();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            var user = ModelMethod.GetUser(model.Email, model.Password);
            if (user != null)
            {
                Session.Clear();
                Session["userEmail"] = user.Email;
                Session["userName"] = user.FullName;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Thông tin đăng nhập không đúng!";
                return View(model);
            }

        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User(model.Email, model.Password, model.FullName, DateTime.Now.ToString("yyyy-MM-dd"));
                var result = ModelMethod.AddUser(user);

                if (result)
                {
                    Session.Clear();
                    Session["userEmail"] = user.Email;
                    Session["userName"] = user.FullName;
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            //return Redirect(strUrl);
            return RedirectToAction("Home", "Index");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}