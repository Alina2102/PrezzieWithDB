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
        private static string userName_tmp = null;

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(CustomerView model)
        {
            if (db.customers.Find(model.userName) != null)
            {
                model.errorMessage = "Username already exists";
                return View("SignUp", model);
            }
            var customers = db.customers.Include(c => c.Profile);
            foreach (Customer c in customers)
            {
                if (c.Profile.eMail == model.eMail)
                {
                    model.errorMessage = "E-mail already exists";
                    return View("SignUp", model);
                }
            }
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
                if (db.customers.Find(model.userName) == null)
                {
                    model.LoginErrorMessage = "Wrong username!";
                    return View("Login", model);
                }

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
            return View();
        }

        // GET: Customer
        public ActionResult Index()
        {
            try
            {
                string userName = Session["userName"].ToString();
                if (userName == "Admin")
                {
                    var customers = db.customers.Include(c => c.Profile);
                    return View(customers.ToList());
                }
                else
                {
                    List<Customer> customers = db.customers.Include(c => c.Profile).ToList();
                    List<Customer> myCustomer = new List<Customer>();
                    foreach (Customer c in customers)
                    {
                        if (c.userName == userName)
                        {
                            myCustomer.Add(c);
                        }
                    }

                    return View(myCustomer.ToList());
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Customer");
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
                return RedirectToAction("Login", "Customer");
            }
            userName_tmp = id;
            CustomerView customer = GetCustomer(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerView model, string id)
        {
            var customers = db.customers.Include(c => c.Profile);
            foreach (Customer c in customers)
            {
                if (c.Profile.eMail == model.eMail)
                {
                    model.errorMessage = "E-mail already exists";
                    return View("Edit", model);
                }
            }

            if (ModelState.IsValid)
            {
                Profile profile = db.profiles.Find(userName_tmp);
                profile.userName = model.userName;
                profile.password = model.password;
                profile.eMail = model.eMail;
                profile.firstName = model.firstName;
                profile.surname = model.surname;
                profile.birthday = model.birthday;
                profile.descriptionUser = model.descriptionUser;
                db.Entry(profile).CurrentValues.SetValues(profile);
                db.SaveChanges();


                Customer customer = db.customers.Find(userName_tmp);
                customer.countryUser = model.countryUser;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           return View(model);
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

        public CustomerView GetCustomer(string userName)
        {
            CustomerView cv = new CustomerView();
            var customer = db.customers.Find(userName);

            cv.userName = customer.userName;
            cv.password = customer.Profile.password;
            cv.eMail = customer.Profile.eMail;
            cv.firstName = customer.Profile.firstName;
            cv.surname = customer.Profile.surname;
            cv.birthday = customer.Profile.birthday;
            cv.countryUser = customer.countryUser;
            cv.descriptionUser = customer.Profile.descriptionUser;

            return cv;
        }
    }
}
