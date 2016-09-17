using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using OneBlog.Models;
using OneBlog.Core;
using System.Web.Security;

namespace OneBlog.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {


        public AccountController()
        {
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = Security.AuthenticateUser(model.UserName, model.Password, model.RememberMe);
            if (result)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "无效的登录尝试。");
                return View(model);
            }

            //var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "无效的登录尝试。");
            //        return View(model);
            //}
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            //if (!await SignInManager.HasBeenVerifiedAsync())
            //{
            //    return View("Error");
            //}
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return null;
            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            //var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(model.ReturnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "代码无效。");
            //        return View(model);
            //}
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }





        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Membership.Provider.GetUser(model.UserName, false);
                if (user == null)
                {
                    // 请不要显示该用户不存在或者未经确认
                    return View("ForgotPasswordConfirmation");
                }
                // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                // 发送包含此链接的电子邮件
                var code = new Random().Next(1000, 9999).ToString();
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "重置密码", "请通过单击 <a href=\"" + callbackUrl + "\">此处</a>来重置你的密码");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //


        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }





        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Security.SignOut();
            if (this.Request.UrlReferrer != null && this.Request.UrlReferrer != this.Request.Url &&
                this.Request.UrlReferrer.LocalPath.IndexOf("/admin/", StringComparison.OrdinalIgnoreCase) == -1)
            {
                return Redirect(this.Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }


        //private void SendCode()
        //{
        //    _confirmationCode = new Random().Next(1000, 9999).ToString();

        //    var mail = new MailMessage
        //    {
        //        From = new MailAddress(BlogSettings.Instance.Email),
        //        Subject = "Your code for password reset is: " + _confirmationCode
        //    };

        //    mail.To.Add(_email);

        //    var sb = new StringBuilder();
        //    sb.Append("<div style=\"font: 11px verdana, arial\">");
        //    sb.AppendFormat("Dear {0}:", _userName);
        //    sb.AppendFormat("<br/><br/>Your password reset code at \"{0}\" is: {1}", BlogSettings.Instance.Name, _confirmationCode);
        //    sb.Append("<br/></br>Please enter this code in the form you used to send this email and we will reset password and send it to you.");
        //    sb.Append(
        //        "<br/><br/>If it wasn't you who initiated the reset, please let us know immediately (use contact form on our site)");
        //    sb.AppendFormat("<br/><br/>Sincerely,<br/><br/><a href=\"{0}\">{1}</a> team.", WebUtils.AbsoluteWebRoot, BlogSettings.Instance.Name);
        //    sb.Append("</div>");

        //    mail.Body = sb.ToString();

        //    var msg = WebUtils.SendMailMessage(mail);

        //    if (string.IsNullOrEmpty(msg))
        //    {
        //        this.Master.SetStatus("success", "Confirmation code was sent, please check your email.");
        //    }
        //    else
        //    {
        //        this.Master.SetStatus("warning", msg);
        //        ClearCode();
        //    }
        //}

        //private void SendMail()
        //{
        //    var mail = new MailMessage
        //    {
        //        From = new MailAddress(BlogSettings.Instance.Email),
        //        Subject = "Your password has been reset"
        //    };

        //    mail.To.Add(_email);

        //    var pwd = Membership.Provider.ResetPassword(_userName, string.Empty);
        //    var sb = new StringBuilder();
        //    sb.Append("<div style=\"font: 11px verdana, arial\">");
        //    sb.AppendFormat("Dear {0}:", _userName);
        //    sb.AppendFormat("<br/><br/>Your password at \"{0}\" has been reset to: {1}", BlogSettings.Instance.Name, pwd);
        //    sb.Append(
        //        "<br/><br/>If it wasn't you who initiated the reset, please let us know immediately (use contact form on our site)");
        //    sb.AppendFormat("<br/><br/>Sincerely,<br/><br/><a href=\"{0}\">{1}</a> team.", WebUtils.AbsoluteWebRoot, BlogSettings.Instance.Name);
        //    sb.Append("</div>");

        //    mail.Body = sb.ToString();

        //    WebUtils.SendMailMessageAsync(mail);

        //    //this.Master.SetStatus("success", Resources.labels.passwordSent);
        //    Response.Redirect(WebUtils.RelativeWebRoot + "Account/login.aspx");
        //}



        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
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