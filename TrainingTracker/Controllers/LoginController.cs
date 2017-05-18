using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI.WebControls;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Controllers
{
    [CustomAuthorizeAttribute]
    public class LoginController : Controller
    {
        /// <summary>
        /// Default Action result to Login Action
        /// </summary>
        /// <returns>redirect to Login</returns>
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Http get method for Default login controller method
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpGet]
        public ActionResult TTLogin(string returnUrl)
        {
            var formCookies = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            ViewBag.ReturnUrl = returnUrl;

            if (formCookies == null) return View("Login");

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(formCookies.Value);

            if (authTicket != null && !authTicket.Expired)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View("Login");
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult TTLogin(LoginModel objLoginModel, string returnUrl)
        {
            if (!ModelState.IsValid) return View("TTLogin");

            var userData = new LoginBl().AuthenticateUser(objLoginModel.UserName, Common.Encryption.Cryptography.Encrypt(objLoginModel.Password));

            if (userData.IsValid)
            {
                User currentUser = new UserBl().GetUserByUserName(userData.UserName);
                Session["currentUser"] = currentUser;
                string serializedUser = new JavaScriptSerializer().Serialize(currentUser);

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                  1,
                  userData.UserName.ToString(),
                  DateTime.Now,
                  DateTime.Now.AddDays(7),
                  false,
                  serializedUser,
                  "/");
                Response.Cookies.Clear();
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                Response.SetCookie(cookie);

                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Login Failed,Invalid Credentials");
            return View("Login");
        }

        /// <summary>
        /// Redirects to Manage license view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Valid()
        {
            return RedirectToAction("Index", "Dashboard");
        }


        public ActionResult GetCurrentUser()
        {
            return Json(new UserBl().GetUserByUserName(User.Identity.Name), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sign outs the user.
        /// </summary>
        /// <returns>To login page.</returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var formCookies = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            ViewBag.ReturnUrl = returnUrl;

            if (formCookies == null) return View("GPSLogin");

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(formCookies.Value);

            if (authTicket != null && !authTicket.Expired)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View("GPSLogin");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel objLoginModel, string returnUrl)
        {
            if (!ModelState.IsValid) return View("GPSLogin");

            var userData = await new LoginBl().GPSAuthentication(objLoginModel.UserName, objLoginModel.Password);
            if (userData != null)
            {
                if (userData.IsValid)
                {
                    User currentUser = new UserBl().GetUserByUserName(userData.UserName);
                    Session["currentUser"] = currentUser;
                    string serializedUser = new JavaScriptSerializer().Serialize(currentUser);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                      1,
                      userData.UserName.ToString(),
                      DateTime.Now,
                      DateTime.Now.AddDays(7),
                      false,
                      serializedUser,
                      "/");
                    Response.Cookies.Clear();
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.SetCookie(cookie);

                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "Login Failed,Invalid Credentials");
            }
            else
            {
                ModelState.AddModelError("", "Your details are not found in TT. Contact Manager for further queries!");
            }
            return View("GPSLogin");
        }
    }
}