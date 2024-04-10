using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoulFar.Models;

namespace SoulFar.Controllers
{
    public class AuthorController : Controller
    {
        DataContext db = new DataContext();
        // GET: Supplier
        public ActionResult Index()
        {
            return View(db.Authors.ToList());
        }

        // CREATE: Supplier

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Author suplr)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(suplr);
                db.SaveChanges();
                return PartialView("_Success");
            }
            return PartialView("_Error");
        }


        // EDIT: Supplier

        public ActionResult Edit(int id)
        {
            Author suplr = db.Authors.Single(x => x.AuthorID == id);
            if (suplr == null)
            {
                return HttpNotFound();
            }
            return View(suplr);
        }

        [HttpPost]
        public ActionResult Edit(Author suplr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suplr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suplr);
        }


        // DETAILS: Supplier

        public ActionResult Details(int id)
        {
            Author suplr = db.Authors.Find(id);
            if (suplr == null)
            {
                return HttpNotFound();
            }
            return View(suplr);
        }

        // DELETE: Supplier

        public ActionResult Delete(int id)
        {
            Author suplr = db.Authors.Find(id);
            if (suplr == null)
            {
                return HttpNotFound();
            }
            return View(suplr);
        }

        //Post Delete Confirmed

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author suplr = db.Authors.Find(id);
            db.Authors.Remove(suplr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}