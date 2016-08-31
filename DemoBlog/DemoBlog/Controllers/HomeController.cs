using DemoBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using DemoBlog.Extensions;

namespace DemoBlog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext ();

        public ActionResult Index()
        {
            var lastPosts = db.Posts
                .Include (p => p.Author)
                .OrderByDescending(p => p.Date)
                .Take(3)
                .ToList();
            
            return View (lastPosts);
        }
    }
}