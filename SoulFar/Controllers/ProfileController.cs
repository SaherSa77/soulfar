using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoulFar.Models;

namespace SoulFar.Controllers
{
    public class ProfileController : Controller
    {
        DataContext db = new DataContext();
        // GET: Profile
        public ActionResult Index()
        {
            var userid = Session["empId"];
            return View(db.admin_Employee.Find(userid));
        }
    }
}