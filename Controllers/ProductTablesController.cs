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
    public class ProductTablesController : Controller
    {
        private JazzCP05Entities db = new JazzCP05Entities();

        // GET: ProductTables
        public ActionResult Index()
        {
            return View(db.ProductTables.ToList());

        }

        public ActionResult IndexProducts()                  //DELETE LATER?
        {
            return View(db.ProductTables.ToList());
        }

        public ActionResult IndexCart()
        {
            decimal x1 = 0M;

            foreach (CartTable item in db.CartTables.ToArray())
            {
                x1 += (item.Quantity * item.Price);
            }
            ViewBag.TotPrice = x1;

            return View(db.CartTables.ToList());
        }

        public ActionResult Index2()                  // Displays the products and cart
        {
            return View(db.ProductTables.ToList());
        }



        public ActionResult SelectCustomer()                  // Select Customer
        {
            ViewBag.customers = db.CustomerTables.Select(i => new SelectListItem() { Text = i.Name });  // delete this soon
            ViewBag.cusName = Session["customerName"] = Request["customers"];

  //          if (ViewBag.cusName != null)
  //          {
  //              CartTable cart6 = db.CartTables.Where(s => s.Name == "Miles Ahead").FirstOrDefault();
  //              cart6.Image = ViewBag.cusName;
  //              Session["customerName"] = cart6.Image;
  //              db.SaveChanges(); 
  //          }

            return View(db.ProductTables.ToList());
        }



        public ActionResult ADD(int? id)                                // ADD to CartTable
        {
            ProductTable product1 = db.ProductTables.Find(id);
            string name1 = db.ProductTables.Find(id).Name;
            CartTable cart1 = db.CartTables.Where(s => s.Name == name1).FirstOrDefault();

            if(cart1 == null)
            {
                CartTable cart2 = new CartTable();
                cart2.CartID = db.ProductTables.Find(id).ProductID;
                cart2.Name = name1;
                cart2.Price = db.ProductTables.Find(id).Price;
                cart2.Image = db.ProductTables.Find(id).Image;
                cart2.Quantity = 1;

                db.CartTables.Add(cart2);
                db.SaveChanges();
            }
            else
            {
                cart1.Quantity++;
                db.SaveChanges();
            }

            return RedirectToAction("Index2");
        }

        public ActionResult RFT(int? id)                                // remove from CART
        {
            CartTable cart4 = db.CartTables.Find(id);
            db.CartTables.Remove(cart4);
            db.SaveChanges();

            return RedirectToAction("Index2");
        }






        // GET: ProductTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTable productTable = db.ProductTables.Find(id);
            if (productTable == null)
            {
                return HttpNotFound();
            }
            return View(productTable);
        }

        // GET: ProductTables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,Price,Image")] ProductTable productTable)
        {
            if (ModelState.IsValid)
            {
                db.ProductTables.Add(productTable);
                db.SaveChanges();
                //return RedirectToAction("Index");                   
                return View("~/Views/CustomerTables/index.cshtml", db.CustomerTables.ToList());
                //return View("~/Views/CustomerTables", db.CustomerTables.ToList());
            }

            return View(productTable);
        }

        // GET: ProductTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTable productTable = db.ProductTables.Find(id);
            if (productTable == null)
            {
                return HttpNotFound();
            }
            return View(productTable);
           
        }

        // POST: ProductTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Price,Image")] ProductTable productTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productTable).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                //return RedirectToAction("Index", "CustomerTablesController");
                //return RedirectToAction("IndexCus");
                //return View("~/Views/Home/Design.cshtml");
                return View("~/Views/CustomerTables/index.cshtml", db.CustomerTables.ToList());
            }
            return View(productTable);
            //return View("IndexCus");                     //\Views\CustomerTables\Index.cshtml
            //return RedirectToAction("customerTable", "CutomerTablesController");
        }

        // GET: ProductTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTable productTable = db.ProductTables.Find(id);
            if (productTable == null)
            {
                return HttpNotFound();
            }
            return View(productTable);
        }

        // POST: ProductTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductTable productTable = db.ProductTables.Find(id);
            db.ProductTables.Remove(productTable);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return View("~/Views/CustomerTables/index.cshtml", db.CustomerTables.ToList());
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
