using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoBlog.Models;
using DemoBlog.Extensions;

namespace DemoBlog.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var post = db.Posts
                .Include (p => p.Author)
                .ToList ();

            return View(post);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post postDetails = db.Posts
                .Include(p => p.Author)
                .First(p => p.Id == id);

            if (postDetails == null)
            {
                return HttpNotFound();
            }

            return View(postDetails);
        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Author = db.Users.FirstOrDefault (p => p.UserName == User.Identity.Name);

                db.Posts.Add(post);
                db.SaveChanges();

                this.AddNotification ("Post created", NotificationType.INFO);
                return RedirectToAction("Index");
            }

            this.AddNotification ("Post ERROR :P", NotificationType.ERROR);
            return View (post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateInput (false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Date")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();

                this.AddNotification ("Post edited", NotificationType.INFO);
                return RedirectToAction ("Index");
            }

            this.AddNotification ("Post ERROR :P", NotificationType.ERROR);
            return View (post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DelComments (id);

            Post post = db.Posts.Find(id);

            db.Posts.Remove(post);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public void DelComments (int id)
        {
            var comments = db.Comments.Where (c => c.PostComm.Id == id).ToList ();
            foreach (var comm in comments) {
                db.Comments.Remove (comm);
            }
            db.SaveChanges ();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
