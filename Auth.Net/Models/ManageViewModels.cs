using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Auth.Net.Models
{

    public class AccountModel
    {
        public string Username { get; set; }
        public string UserId { get; set; }

        public int SessionCount { get; set; }
    }

    public class IndexViewModel
    {
        public IList<AccountModel> Logins { get; set; }

    }

}