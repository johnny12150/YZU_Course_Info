using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace yzu_course_info_API.Controllers
{
    [Authorize]
    public class ForumController : Controller
    {
        // GET: Forum
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "論壇";
            return View();
        }
        
        // GET:/Forum/CourseDetails?cId=
        public ActionResult CourseDetails(string cId)
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            ViewBag.nickname = ticket.UserData;
            ViewBag.mId = ticket.Name;
            ViewBag.course = cId;
            ViewBag.Title = "課程討論區";
            return View();
        }
    }
}