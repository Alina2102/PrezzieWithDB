using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrezzieWithDB.DAL;
using PrezzieWithDB.Models;
using PrezzieWithDB.ViewModels;

namespace PrezzieWithDB.Controllers
{
    public class CustomerController : Controller
    {
        private PrezzieContext db = new PrezzieContext();

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(CustomerView model)
        {
            if (ModelState.IsValid)
            {

                Profile profile = new Profile
                {
                    userName = model.userName,
                    eMail = model.eMail,
                    password = model.password,
                    firstName = model.firstName,
                    surname = model.surname,
                    birthday = model.birthday,
                    descriptionUser = model.descriptionUser
                };


                db.profiles.Add(profile);
                db.SaveChanges();

                Customer customer = new Customer
                {
                    userName = profile.userName,
                    countryUser = model.countryUser
                };

                db.customers.Add(customer);
                db.SaveChanges();

                Session["userName"] = customer.userName;
                return RedirectToAction("Index");

            }

            return View();
        }



        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(CustomerLoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = db.customers.Find(model.userName).userName;
                }
                catch (Exception)
                {
                    model.LoginErrorMessage = "Wrong username!";
                    return View("Login", model);
                }
                {
                    if (db.customers.Find(model.userName).Profile.password == model.password)
                        {
                            Session["userName"] = model.userName;
                            return RedirectToAction("Index", "Home");
                        }
                    else
                        {
                            model.LoginErrorMessage = "Wrong password!";
                            return View("Login", model);
                        }
                }
            }
            return View();
        }

        // GET: Customer
        public ActionResult Index()
        {
            if (Session["userName"] == null)
            {
                return HttpNotFound();
            }

            if (Session["userName"] == "Admin")
            {
                var customers = db.customers.Include(c => c.Profile);
                return View(customers.ToList());
            }
            else
            {
                var customers = db.customers.Include(c => c.Profile);
                //customers.Where(x => x.userName == Session["userName"]).SingleOrDefault().ToList();
                return View(customers.ToList());
            }
        }

        // GET: Customer/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

       
        // GET: Customer/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.userName = new SelectList(db.profiles, "userName", "eMail", customer.userName);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userName,countryUser")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userName = new SelectList(db.profiles, "userName", "eMail", customer.userName);
            return View(customer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
            db.SaveChanges();
            Profile profile = db.profiles.Find(id);
            db.profiles.Remove(profile);
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
