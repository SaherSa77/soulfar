﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoulFar.Models;
using System.Data;

namespace IMS_Project.Controllers
{
    public class DashboardController : Controller
    {
        DataContext db = new DataContext();
        public ActionResult Index()
        {

            ViewBag.latestOrders = db.Orders.Where(x=>x.PaymentID!=null).OrderByDescending(x => x.OrderID).Take(10).ToList();
            //ViewBag.NewOrders = db.Orders.Where(a => a.DIspatched == false && a.Shipped == false && a.Deliver == false).Count();
            //ViewBag.DispatchedOrders = db.Orders.Where(a => a.DIspatched == true && a.Shipped == false && a.Deliver == false).Count();
            //ViewBag.ShippedOrders = db.Orders.Where(a => a.DIspatched == true && a.Shipped == true && a.Deliver == false).Count();
            //ViewBag.DeliveredOrders = db.Orders.Where(a => a.DIspatched == true && a.Shipped == true && a.Deliver == true).Count();
            return View();
        }

    }
}