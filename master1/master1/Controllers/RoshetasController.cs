using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using master1.Models;
using Microsoft.AspNet.Identity;

namespace master1.Controllers
{
    public class RoshetasController : Controller
    {
        private master1Entities db = new master1Entities();

        // GET: Roshetas
        public ActionResult Index()
        {
            //accept doc ==1
            var x = from e in db.Roshetas
                    select e.userid;
             
            string[] users = x.ToArray();
            var No = users.Distinct().ToArray();
            ViewBag.us = No.Length;
            
                var roshetas = db.Roshetas.Include(r => r.AspNetUser);
                return View(roshetas.ToList());
           
       
        }

        public FileResult Download(string Rosheta_pic)
        {
            string name = "../photos/" + Rosheta_pic;
            string path = Server.MapPath(name);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application.pdf", Rosheta_pic);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Accept_doc")] Rosheta rosheta , int id, string search)
        {
           

                //var class2 = db.AspNetUsers.SingleOrDefault(m => m.Id == id);
                //class2. = true;
                //db.SaveChanges();
                var x = from c in db.Roshetas
                        where c.Rosheta_id == id
                        select c.Accept_doc;
                var myrosheta = x.First();
                myrosheta = true;
                //int pay = payment;
                //var newOne = new Rosheta { userid = id, Rosheta_pic = x., City = 250, age = "Semester fees" };

                //db.Roshetas.Add(myrosheta);
                db.SaveChanges();
                return RedirectToAction("Index");
            
     
        }

        // GET: Roshetas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rosheta rosheta = db.Roshetas.Find(id);
            if (rosheta == null)
            {
                return HttpNotFound();
            }
            return View(rosheta);
        }

        // GET: Roshetas/Create
        public ActionResult Create()
        {
            ViewBag.id = User.Identity.GetUserId();
            ViewBag.userid = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Roshetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Rosheta_id,userid,Rosheta_pic,City,age,note,Accept_doc,Accept_user")] Rosheta rosheta ,HttpPostedFileBase Rosheta_pic)
        {
          
            if (ModelState.IsValid)
            {
                string path = "~/photos/" + Path.GetFileName(Rosheta_pic.FileName);
                string path2 = Path.GetFileName(Rosheta_pic.FileName);
                Rosheta_pic.SaveAs(Server.MapPath(path));
                rosheta.Rosheta_pic = path2.ToString();
                db.Roshetas.Add(rosheta);
                db.SaveChanges();
                ViewBag.accept = "true";
                //return RedirectToAction("Home", "container");
            }
            else
            {
                ViewBag.accept = "false";
            }

            ViewBag.userid = new SelectList(db.AspNetUsers, "Id", "Email", rosheta.userid);
            return View(rosheta);
        }
        
        // GET: Roshetas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rosheta rosheta = db.Roshetas.Find(id);
            if (rosheta == null)
            {
                return HttpNotFound();
            }
            ViewBag.userid = new SelectList(db.AspNetUsers, "Id", "Email", rosheta.userid);
            return View(rosheta);
        }

        // POST: Roshetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Rosheta_id,userid,Rosheta_pic,City,age,note,Accept_doc,Accept_user")] Rosheta rosheta)
        {
            if (ModelState.IsValid)
            {
                if(rosheta.Rosheta_pic == null)
                {
                    var img = from x in db.Roshetas
                              where x.Rosheta_id==rosheta.Rosheta_id
                              select x.Rosheta_pic;
                    string pic = img.First();
                    rosheta.Rosheta_pic = pic;
                }
                rosheta.Accept_doc = true;
                db.Entry(rosheta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userid = new SelectList(db.AspNetUsers, "Id", "Email", rosheta.userid);
            return View(rosheta);
        }

        // GET: Roshetas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rosheta rosheta = db.Roshetas.Find(id);
            if (rosheta == null)
            {
                return HttpNotFound();
            }
            return View(rosheta);
        }

        // POST: Roshetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rosheta rosheta = db.Roshetas.Find(id);
            db.Roshetas.Remove(rosheta);
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
