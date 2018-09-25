using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JazzCPO5.Models;

namespace JazzCPO5.Controllers
{
    public class OrderTablesController : Controller
    {
        private JazzCP05Entities db = new JazzCP05Entities();

        // GET: OrderTables
        public ActionResult Index()
        {
            var orderTables = db.OrderTables.Include(o => o.CustomerTable).Include(o => o.ProductTable);
            //ViewBag.cusName3 = (string)Session["customerName"];

            return View(orderTables.ToList());
        }

 //       public ActionResult Design()    // used to be in home controller
 //       {
            //var orderTables = db.OrderTables.Include(o => o.CustomerTable).Include(o => o.ProductTable);
            //ViewBag.cusName3 = (string)Session["customerName"];

  //          return View("Design");
    //    }



        public ActionResult PlaceOrder()                     // 
        {
            if (Session["customerName"] == null)
            {
                return RedirectToAction("Home", "Home" , new { area = "Admin" });
                //return View("Home/Design");
            }
            else
            {
                string nameCart = ViewBag.cusName = (string)Session["customerName"];
                CustomerTable cus1 = db.CustomerTables.Where(s => s.Name == nameCart).ToList().FirstOrDefault();    //added ToList()
                int x1 = 1;
                //ProductTable prod1 = new ProductTable();
                //OrderTable ord1 = new OrderTable();

                foreach (CartTable car1 in db.CartTables)
                {
                    ProductTable prod1 = new ProductTable();
                    prod1 = db.ProductTables.Where(s => s.Name == car1.Name).ToList().FirstOrDefault();       // FIXED!!!    added ToList()

                    OrderTable ord1 = new OrderTable();
                    ord1.OrderID = (db.OrderTables.Count() + x1);
                    ord1.CustomerID = cus1.CustomerID;
                    ord1.ProductID = prod1.ProductID;                                                // FIXED!!!
                    ord1.Date = DateTime.Today.ToString("d");
                    ord1.Quantity = car1.Quantity;
                    ord1.Total = (car1.Quantity * car1.Price);
                    x1++;

                    db.OrderTables.Add(ord1);
                }

                foreach (CartTable car2 in db.CartTables)                            // Delete items from cart
                {
                    db.CartTables.Remove(car2);
                }

                db.SaveChanges();                                                    // Save changes after loop!
                return RedirectToAction("Index");
            }
        }




        // GET: OrderTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTable orderTable = db.OrderTables.Find(id);
            if (orderTable == null)
            {
                return HttpNotFound();
            }
            return View(orderTable);
        }

        // GET: OrderTables/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.CustomerTables, "CustomerID", "Name");
            ViewBag.ProductID = new SelectList(db.ProductTables, "ProductID", "Name");
            return View();
        }

        // POST: OrderTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,ProductID,Date,Quantity,Total")] OrderTable orderTable)
        {
            if (ModelState.IsValid)
            {
                db.OrderTables.Add(orderTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.CustomerTables, "CustomerID", "Name", orderTable.CustomerID);
            ViewBag.ProductID = new SelectList(db.ProductTables, "ProductID", "Name", orderTable.ProductID);
            return View(orderTable);
        }

        // GET: OrderTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTable orderTable = db.OrderTables.Find(id);
            if (orderTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.CustomerTables, "CustomerID", "Name", orderTable.CustomerID);
            ViewBag.ProductID = new SelectList(db.ProductTables, "ProductID", "Name", orderTable.ProductID);
            return View(orderTable);
        }

        // POST: OrderTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,ProductID,Date,Quantity,Total")] OrderTable orderTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.CustomerTables, "CustomerID", "Name", orderTable.CustomerID);
            ViewBag.ProductID = new SelectList(db.ProductTables, "ProductID", "Name", orderTable.ProductID);
            return View(orderTable);
        }

        // GET: OrderTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTable orderTable = db.OrderTables.Find(id);
            if (orderTable == null)
            {
                return HttpNotFound();
            }
            return View(orderTable);
        }

        // POST: OrderTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderTable orderTable = db.OrderTables.Find(id);
            db.OrderTables.Remove(orderTable);
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
