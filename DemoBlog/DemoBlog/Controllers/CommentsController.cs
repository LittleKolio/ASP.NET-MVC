using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DemoBlog.Models;
using DemoBlog.Extensions;

namespace DemoBlog.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult (HttpStatusCode.BadRequest);
            }
            var listComments = db.Comments
                .Include(c => c.Author)
                .Where (c => c.PostComm.Id == id)
                .OrderByDescending(c => c.Date)
                .ToList();

            return View(listComments);
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments
                .Include(c => c.Author)
                .Include(c => c.PostComm)
                .First(c => c.Id == id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult (HttpStatusCode.BadRequest);
            }

            TempData ["data1"] = id;
            ViewBag.toPost = id;

            return View ();
        }

        // POST: Comments/Create
        [HttpPost]
        [Authorize]
        [ValidateInput (false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            var currentPost = int.Parse (TempData ["data1"].ToString ());
            if (ModelState.IsValid)
            {
                comment.PostComm = db.Posts.FirstOrDefault (c => c.Id == currentPost);
                comment.Author = db.Users.FirstOrDefault (a => a.UserName == User.Identity.Name);

                db.Comments.Add(comment);
                db.SaveChanges();

                this.AddNotification ("Comment created", NotificationType.INFO);
                return RedirectToAction("Details", "Posts", new { id = currentPost });
            }

            this.AddNotification ("Comment ERROR :P", NotificationType.ERROR);
            return View (comment);
        }

        // GET: Comments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult (HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments
                .Include(c => c.PostComm)
                .FirstOrDefault(c => c.Id == id);

            TempData ["data2"] = db.Comments.First(c => c.Id == id).PostComm.Id;

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateInput (false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body,Date")] Comment comment)
        {
            var currentPost = int.Parse (TempData ["data2"].ToString ());
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();

                this.AddNotification ("Comment edited", NotificationType.INFO);
                return RedirectToAction("Details", "Posts", new { id = currentPost });
            }

            this.AddNotification ("Comment ERROR :P", NotificationType.ERROR);
            return View (comment);
        }

        // GET: Comments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments
                .Include(c => c.PostComm)
                .First(c => c.Id == id);

            TempData ["data3"] = db.Comments.First (c => c.Id == id).PostComm.Id;

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var currentPost = int.Parse (TempData ["data3"].ToString ());

            Comment comment = db.Comments.Find(id);

            db.Comments.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Details", "Posts", new { id = currentPost });
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
