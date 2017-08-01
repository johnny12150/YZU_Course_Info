using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using yzu_course_info_API.Models;
using yzu_course_info_API.ViewModels;

namespace yzu_course_info_API.Controllers
{

    /// <summary>
    /// 後台首頁、登入頁...
    /// </summary>
    [Authorize]
    public class MemberController : Controller
    {
        private Entities db = new Entities();

        /// <summary>
        /// 呈現後台使用者登入頁
        /// </summary>
        /// <param name="ReturnUrl">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index");
            ViewBag.Title = "登入";
            //ReturnUrl字串是使用者在未登入情況下要求的的Url
            LoginViewModel lvm = new LoginViewModel() { ReturnUrl = ReturnUrl };
            return View(lvm);
        }

        /// <summary>
        /// 後台使用者進行登入
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="u">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel lvm)
        {

            //沒通過Model驗證(必填欄位沒填，DB無此帳密)
            if (!ModelState.IsValid)
            {
                return View(lvm);
            }


            //都成功時 進行表單登入
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                lvm.mId,//你想要存放在 User.Identy.Name 的值，通常是使用者帳號
                DateTime.Now,
                DateTime.Now.AddMinutes(10),
                false,//將管理者登入的 Cookie 設定成 Session Cookie
                lvm.mNickname,//userdata看你想存放啥
                FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            cookie.Expires = DateTime.Now.AddMinutes(10);
            Response.Cookies.Add(cookie);

            //導向預設Url

            //(1) 使用者原先Request的Url
            //FormsAuthentication.RedirectFromLoginPage(lvm.mId, false);
            try { Response.Redirect(this.Request.QueryString["ReturnUrl"]); }
            catch { }
            //(2) Web.config裡的defaultUrl定義 (若剛剛已導向，此行不會執行到)
            return Redirect(FormsAuthentication.GetRedirectUrl(lvm.mId, false));
        }

        /// <summary>
        /// 後台使用者登出動作
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //清除Session中的資料
            Session.Abandon();
            //登出表單驗證
            FormsAuthentication.SignOut();
            //導至登入頁
            return RedirectToAction("Login", "Member");
        }


        /// <summary>
        /// 註冊頁面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Title = "註冊";
            return View();
        }


        /// <summary>
        /// 註冊實做
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel rvm)
        {
            //沒通過Model驗證(必填欄位沒填，DB無此帳密)
            if (!ModelState.IsValid)
            {
                return View(rvm);
            }
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 後台首頁 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "會員中心";
            try
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                ViewBag.usrNickname = ticket.UserData;
            }
            catch { }
            return View();
        }

        /// <summary>
        /// 後台實做
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MemberViewModel mvm)
        {

            if (!ModelState.IsValid)
            {
                return View(mvm);
            }

            return RedirectToAction("Logout");
        }
    }
}