﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Omadiko.Database;
using Omadiko.Entities;

namespace Omadiko.WebApp.Controllers
{
    public class CommentsAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CommentsAdmin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Article).Include(c => c.Customer);
            return View(comments.ToList());
        }

        // GET: CommentsAdmin/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: CommentsAdmin/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ArticleId = new SelectList(db.Articles, "ArticleId", "Blog");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerFirstName");
            return View();
        }

        // POST: CommentsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CommentId,Comments,CustomerName,CustomerEmail,ArticleId,CustomerId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleId = new SelectList(db.Articles, "ArticleId", "Blog", comment.ArticleId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerFirstName", comment.CustomerId);
            return View(comment);
        }

        // GET: CommentsAdmin/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "ArticleId", "Blog", comment.ArticleId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerFirstName", comment.CustomerId);
            return View(comment);
        }

        // POST: CommentsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CommentId,Comments,CustomerName,CustomerEmail,ArticleId,CustomerId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "ArticleId", "Blog", comment.ArticleId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerFirstName", comment.CustomerId);
            return View(comment);
        }

        // GET: CommentsAdmin/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: CommentsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
