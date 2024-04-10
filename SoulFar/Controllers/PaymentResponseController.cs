using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoulFar.Models;

namespace SoulFar.Controllers
{
    public class PaymentResponseController : Controller
    {
        DataContext db = new DataContext();
        // GET: ThankYou
        public ActionResult ThankYou()
        {
            ViewBag.cartBox = null;
            ViewBag.Total = null;
            ViewBag.NoOfItem = null;
            TempShpData.items = null;
            var payobj = Session["payObj"] as PaymentVM;
            if (payobj != null)
            {
                var items=db.OrderDetails.Where(x=>x.OrderID== payobj.OrderID).ToList();
                foreach (var item in items)
                {
                    var prod = db.Products.FirstOrDefault(x => x.ProductID == item.ProductID);
                    if (prod != null)
                    {
                        prod.UnitInStock = prod.UnitInStock - item.Quantity;
                        db.Products.AddOrUpdate(prod);
                        db.SaveChanges();
                    }
                }
            }
            Session["payObj"] = null;
            return View("Thankyou");
        }

        public ActionResult Cancel()
        {
            return View();
        }

    }
}