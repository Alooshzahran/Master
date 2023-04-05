using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using master1.Models;

namespace master1.Controllers
{
    public class ProductsinCartsController : Controller
    {
        private master1Entities db = new master1Entities();

        // GET: ProductsinCarts
        public ActionResult Index()
        {
            var productsinCarts = db.ProductsinCarts.Include(p => p.cart).Include(p => p.product);
            return View(productsinCarts.ToList());
        }

        // GET: ProductsinCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsinCart productsinCart = db.ProductsinCarts.Find(id);
            if (productsinCart == null)
            {
                return HttpNotFound();
            }
            return View(productsinCart);
        }

        // GET: ProductsinCarts/Create
        public ActionResult Create()
        {
            ViewBag.cartid = new SelectList(db.carts, "cart_id", "userid");
            ViewBag.productid = new SelectList(db.products, "product_id", "product_name");
            return View();
        }

        // POST: ProductsinCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cartid,productid,productquantity,Total")] ProductsinCart productsinCart)
        {
            if (ModelState.IsValid)
            {
                db.ProductsinCarts.Add(productsinCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cartid = new SelectList(db.carts, "cart_id", "userid", productsinCart.cartid);
            ViewBag.productid = new SelectList(db.products, "product_id", "product_name", productsinCart.productid);
            return View(productsinCart);
        }

        // GET: ProductsinCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsinCart productsinCart = db.ProductsinCarts.Find(id);
            if (productsinCart == null)
            {
                return HttpNotFound();
            }
            ViewBag.cartid = new SelectList(db.carts, "cart_id", "userid", productsinCart.cartid);
            ViewBag.productid = new SelectList(db.products, "product_id", "product_name", productsinCart.productid);
            return View(productsinCart);
        }

        // POST: ProductsinCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cartid,productid,productquantity,Total")] ProductsinCart productsinCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsinCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cartid = new SelectList(db.carts, "cart_id", "userid", productsinCart.cartid);
            ViewBag.productid = new SelectList(db.products, "product_id", "product_name", productsinCart.productid);
            return View(productsinCart);
        }

        // GET: ProductsinCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsinCart productsinCart = db.ProductsinCarts.Find(id);
            if (productsinCart == null)
            {
                return HttpNotFound();
            }
            return View(productsinCart);
        }

        // POST: ProductsinCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductsinCart productsinCart = db.ProductsinCarts.Find(id);
            db.ProductsinCarts.Remove(productsinCart);
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
