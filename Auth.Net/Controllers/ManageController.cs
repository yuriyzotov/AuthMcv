using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Auth.Net.Models;
using Auth.Net.Hubs;


namespace Auth.Net.Controllers
{
    [Authorize(Users = StringConstants.MasterUserName)]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

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

        //
        // GET: /Manage/Index
        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                    message == ManageMessageId.UserLoggedOff ? "The login was logged off."
                    : message == ManageMessageId.Error ? "An error has occurred."
                    : "";
            var model = new IndexViewModel
            {
                Logins = UserManager.Users.ToList().Select( u=> new AccountModel() { Username = u.UserName, UserId = u.Id , SessionCount= AuthorisationHub.GetLiveConnectionsCount(u.UserName) }).ToList()
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string userId)
        {
            ManageMessageId? message;
            //refresh security token to end user session and invalidate his auth cookie
            var result = await UserManager.UpdateSecurityStampAsync(userId);
            if (result.Succeeded)
            {
                var user= await UserManager.FindByIdAsync(userId);
                message = ManageMessageId.UserLoggedOff;

                //update connected clients
                AuthorisationHub.Refresh(user.UserName);
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Index", new { Message = message });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers

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


        public enum ManageMessageId
        {
            UserLoggedOff,
            Error
        }

#endregion
    }
}