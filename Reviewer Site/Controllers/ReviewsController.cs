using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Reviewer_Site.Models
{
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reviews
        public ActionResult Index(string searchString)
        {
            var reviews = db.Reviews.Where(s => s.Company.Name.Equals(searchString));

            ViewBag.Company = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (reviews.Any())
                {        
                    double average = 0;
                    foreach (Review r in reviews)
                    {
                        average += r.Rating;
                    }
                    average = average / reviews.Count();

                    ViewBag.Average = average;
                }
                else
                {
                    ViewBag.Average = "N/A";
                }
            }

            return View(reviews);
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize()]
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize()]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewID,CompanyID,UserID,Rating,Comment")] Review review)
        {
            string UserName = System.Web.HttpContext.Current.User.Identity.Name;
            review.UserID = UserName;

            if (db.Reviews.Where(r => r.UserID == UserName && r.CompanyID == review.CompanyID).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "You have already rated this company");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(review);
                    db.SaveChanges();

                    //string CompanyName = db.Companies.Where(c => c.CompanyID == review.CompanyID).FirstOrDefault().Name;
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", review.CompanyID);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", review.CompanyID);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewID,CompanyID,UserID,Rating,Comment")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "Name", review.CompanyID);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
