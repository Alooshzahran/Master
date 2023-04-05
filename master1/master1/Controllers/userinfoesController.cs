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
    public class userinfoesController : Controller
    {
        private master1Entities db = new master1Entities();

        // GET: userinfoes
        public ActionResult Index()
        {
            var userinfoes = db.userinfoes.Include(u => u.AspNetUser);
            return View(userinfoes.ToList());
        }

        // GET: userinfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userinfo userinfo = db.userinfoes.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        // GET: userinfoes/Create
        public ActionResult Create()
        {
            ViewBag.Userid = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: userinfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Userid,Username,Phone,Address,Bio,Email")] userinfo userinfo)
        {
            if (ModelState.IsValid)
            {
                db.userinfoes.Add(userinfo);
                db.SaveChanges();
                
                return RedirectToAction("Index"); 
            }
          
            ViewBag.Userid = new SelectList(db.AspNetUsers, "Id", "Email", userinfo.Userid);
            return View(userinfo);
        }

        // GET: userinfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userinfo userinfo = db.userinfoes.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Userid = new SelectList(db.AspNetUsers, "Id", "Email", userinfo.Userid);
            return View(userinfo);
        }

        // POST: userinfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Userid,Username,Phone,Address,Bio,Email")] userinfo userinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Userid = new SelectList(db.AspNetUsers, "Id", "Email", userinfo.Userid);
            return View(userinfo);
        }

        // GET: userinfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userinfo userinfo = db.userinfoes.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        // POST: userinfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userinfo userinfo = db.userinfoes.Find(id);
            db.userinfoes.Remove(userinfo);
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
