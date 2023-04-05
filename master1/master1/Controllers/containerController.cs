using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using master1.Models;
using Microsoft.AspNet.Identity;

namespace master1.Controllers
{
    public class containerController : Controller
    {
        master1Entities db = new master1Entities();
        // GET: container
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Home([Bind(Include = "cart_id,user_id")]cart cart1 )
        {
            string idd = User.Identity.GetUserId();
            HttpCookie id = new HttpCookie("id");
            
            id.Values.Add("userid", idd);
            Response.Cookies.Add(id);
            ViewBag.dbcat = db.Categories;
            ViewBag.dbprod = db.products;
            var userexist = db.carts.Any(x => x.userid == idd);
            if (idd != null)
            {
                if (userexist)
                {
                    string a = "asdasd";
                }
                else
                {
                    cart1 = new cart();
                    cart1.userid = idd;    
                    db.carts.Add(cart1); 
                    db.SaveChanges();
                }

            }
            return View();
        }
        public ActionResult singlecard()
        {
           int idd = Convert.ToInt32(Request.QueryString["id"]);
            var id2 = from x in db.products
                      where x.product_id == idd
                      select x;
                     ViewBag.prod=id2;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult singlecard([Bind(Include = "id,cartid,productid,productquantity,Total")] ProductsinCart productsinCart,FormCollection form)
        {
            if (ModelState.IsValid)
            {
                int idd = Convert.ToInt32(Request.QueryString["id"]);

                string idd2 = User.Identity.GetUserId();
                
                if (idd2 != null)
                {
                    var cartid1 = from r in db.carts
                                  where r.userid == idd2

                                  select r.cart_id;
                    int mycart = cartid1.First();
                    //var productexist = db.products.Any(x => x.product_id == idd);

                    productsinCart = new ProductsinCart();
                    productsinCart.cartid = mycart;
                    productsinCart.productid = idd;
                    productsinCart.productquantity = Convert.ToInt16(form["productquantity"]);
                    productsinCart.Total = 0;

                    db.ProductsinCarts.Add(productsinCart);
                    db.SaveChanges();
                    return RedirectToAction("Cart");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
               
            }

            ViewBag.cartid = new SelectList(db.carts, "cart_id", "userid", productsinCart.cartid);
            ViewBag.productid = new SelectList(db.products, "product_id", "product_name", productsinCart.productid);
            return View(productsinCart);
            
        }
        public ActionResult Cart()
        {
            
            string id = User.Identity.GetUserId();
            if ( id != null)
            {
                var carts = from ca in db.carts
                            where ca.userid == id
                            select ca.cart_id;

                int formID = carts.First();
                var productincart = from x in db.ProductsinCarts
                                   join  a in db.products 
                                    on x.productid equals a.product_id
                                    where x.cartid == formID
                                    select a;
                ViewBag.dbcart = productincart;
                //db.ProductsinCarts.Add(productincart);
                //db.SaveChanges();
              
            }
            else
            {
                ViewBag.dbcart = "";
            }
            return View();
        }
        public ActionResult shop(string search)
        {
            var products = from pr in db.products select pr;
            if (search != null)
            {
            
                var product = db.products.Where(r => r.product_name.Contains(search)).ToList();
                ViewBag.dbproduct = product;
               
                return View();
            }
            else
            {
                ViewBag.Message = "Your application description page.";
                ViewBag.dbproduct = db.products.ToList();
                return View();
            }
        }
        public ActionResult Rosheta()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
      
        public ActionResult Profilee(userinfo userinfo1)
        {
            //get
            string id = User.Identity.GetUserId();
           
            ViewBag.Message = "Your application description page.";
            var s = db.userinfoes.Any(x => x.Userid == id);

            if (id != null)
            {
                var m = from d in db.userinfoes
                        where d.Userid == id
                        select d;
                ViewBag.dbinfo = m;
                var userexist = db.userinfoes.Any(x => x.Userid == id);
                if (userexist)
                {
                    string a = "asdasd";
                }
                else
                {
                    userinfo1 = new userinfo();
                    userinfo1.Userid = id;
                    db.userinfoes.Add(userinfo1);
                    db.SaveChanges();
                }
              
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
            
           
            return View();
        }
        [HttpPost]
     
        public ActionResult Profilee([Bind(Include = "Id,Userid,Username,Phone,Address,Bio,Email")] userinfo userinfo1, FormCollection form)
        {
            string id = User.Identity.GetUserId();
            Convert.ToInt16(form["productquantity"]);
            if (ModelState.IsValid)
            {


                var oldinfo = from s in db.userinfoes
                              where s.Userid == id
                              select s;

                userinfo1.Userid = id;
                userinfo1.Username = form["Username"].ToString();
                userinfo1.Email = form["Email"].ToString();


                db.Entry(oldinfo).CurrentValues.SetValues(userinfo1);

                db.SaveChanges();

            }
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout([Bind(Include = "Order_id,userid,total_price,location,cartid")] Order order)
        {
            string id = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {

                var tot = from x in db.carts
                          join a in db.ProductsinCarts
                          on x.cart_id equals a.cartid
                          where x.userid == id
                          select a.Total;
                var car = from x in db.carts
                          where x.userid == id
                          select x.cart_id;
                int tot1 = tot.First();
                int car1 = car.First();
                order.total_price = tot1;
                  order.location = null;
                order.userid = id;
                order.cartid =car1 ;
              
                db.Orders.Add(order);
                db.SaveChanges();
                ViewBag.accept = "true";
                //return RedirectToAction("Home");
            }
            else
            {
                ViewBag.accept = "false";
            }

            ViewBag.userid = new SelectList(db.AspNetUsers, "Id", "Email", order.userid);
            return View(order);

           
        }
        public ActionResult Nofication()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ProfileInfo( userinfo userinfo3)
        {
            string idd = User.Identity.GetUserId();
            string ror = Request["Username"].ToString();
            if (ModelState.IsValid)
            {
                userinfo3.Username = ror;
                db.Entry(userinfo3).State = EntityState.Modified;
                db.SaveChanges();
        
            }
            ViewBag.Userid = new SelectList(db.AspNetUsers, "Id", "Email", userinfo3.Userid);
            return View(userinfo3);
            
        }
    }
}