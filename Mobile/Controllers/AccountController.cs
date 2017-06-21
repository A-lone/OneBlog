using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;

namespace Mobile.Controllers
{
    //http://www.cnblogs.com/indexlang/p/indexlang.html
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        //public String Authenticate(string user, string password)
        //{

        //    //if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
        //    //    return "failed";
        //    //var userIdentity = UserManager.FindAsync(user, password).Result;
        //    //if (userIdentity != null)
        //    //{
        //    //    var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
        //    //    identity.AddClaim(new Claim(ClaimTypes.Name, user));
        //    //    identity.AddClaim(new Claim(type: ClaimTypes.NameIdentifier, value: userIdentity.Id));
        //    //    AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
        //    //    var currentUtc = new SystemClock().UtcNow;
        //    //    ticket.Properties.IssuedUtc = currentUtc;
        //    //    ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
        //    //    string AccessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);
        //    //    return AccessToken;
        //    //}
        //    //return "failed";
        //}
    }
}
