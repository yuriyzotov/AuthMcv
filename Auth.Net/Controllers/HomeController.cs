using Auth.Net.Helpers;
using Auth.Net.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
                Session["key"] as byte[],
                Session["iv"] as byte[]
                );
            return Json(new { value });
        }

        [HttpPost]
        public JsonResult GetAES256(string data)
        {
            var publicKeyBytes = Convert.FromBase64String(data);

            var keyBytes = CryptoHelper.GetRandomKey(48);
            //48 bytes means: 256 bits for key and 128 bits for iv value 
            Session["key"] = keyBytes.Take(32).ToArray();
            Session["iv"] = keyBytes.Skip(32).Take(16).ToArray();

            //convert values to string by hex, due js encryptor work with strings
            var keyStr = (Session["key"] as byte[]).ToHex();
            var ivStr = (Session["iv"] as byte[]).ToHex();

            //encrypt them with RSA
            var keyEnc = CryptoHelper.EncryptRSA(publicKeyBytes, Encoding.ASCII.GetBytes(keyStr));
            var ivEnc = CryptoHelper.EncryptRSA(publicKeyBytes, Encoding.ASCII.GetBytes(ivStr));

            return Json(new {
                key = Convert.ToBase64String(keyEnc, Base64FormattingOptions.None),
                iv = Convert.ToBase64String(ivEnc, Base64FormattingOptions.None)
                } , JsonRequestBehavior.AllowGet);
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