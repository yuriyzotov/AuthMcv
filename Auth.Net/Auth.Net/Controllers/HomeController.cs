using Auth.Net.Helpers;
using Auth.Net.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Auth.Net.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            
        }

        [HttpPost]
        public async Task<ActionResult> DecryptAES(string data)
        {
            //wait for 5 seconds
            await Task.Delay(5000);
            string value = CryptoHelper.DecryptAES256(
                Convert.FromBase64String(data),
                Convert.FromBase64String(Session["key"] as string),
                Convert.FromBase64String(Session["iv"] as string)
                );
            return Json(new { value });
        }

        public JsonResult GetAES256()
        {
            var keyBytes = CryptoHelper.GetRandomKey(48);
            //48 bytes means -256 bits for key and 128 bits for iv value 

            Session["key"] = Convert.ToBase64String(keyBytes.Take(32).ToArray(), Base64FormattingOptions.None) ;
            Session["iv"] = Convert.ToBase64String(keyBytes.Skip(32).Take(16).ToArray(), Base64FormattingOptions.None);

            return Json(new { key=Session["key"], iv= Session["iv"] } , JsonRequestBehavior.AllowGet);
        }

        public  ActionResult Index()
        {
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
    }
}