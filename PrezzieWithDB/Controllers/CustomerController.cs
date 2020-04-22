﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
        public ActionResult SignUp(CustomerView model, HttpPostedFileBase file)
        {
            if (db.customers.Find(model.userName) != null)
            {
                model.errorMessage = "Username already exists";
                return View("SignUp", model);
            }
            var customers = db.customers.Include(c => c.profile);
            foreach (Customer c in customers)
            {
                if (c.profile.eMail == model.eMail)
                {
                    model.errorMessage = "E-mail already exists";
                    return View("SignUp", model);
                }
            }

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

                Customer customer = new Customer();
                try
                {
                    string fileName = profile.userName;
                    string extension = Path.GetExtension(file.FileName);
                    fileName += extension;
                    customer.selectedPictureCustomer = "~/Content/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/"), fileName);
                    file.SaveAs(fileName);
                }
                catch (Exception)
                {
                    customer.selectedPictureCustomer = "~/Content/defaultUser.png";
                }

                customer.userName = profile.userName;
                customer.countryUser = model.countryUser;

                db.customers.Add(customer);
                db.SaveChanges();

                Session["userName"] = customer.userName;

                return RedirectToAction("Index");
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
                if (db.customers.Find(model.userName) == null || db.customers.Find(model.userName).userName != model.userName)
                {
                    model.LoginErrorMessage = "Wrong username!";
                    return View("Login", model);
                }

                if (db.customers.Find(model.userName).profile.password == model.password)
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
                    var customers = db.customers.Include(c => c.profile);
                    return View(customers.ToList());
                }
                else 
                {
                    List<Customer> customers = db.customers.Include(c => c.profile).ToList();
                    List<Customer> myCustomer = new List<Customer>();
                    foreach (Customer c in customers)
                    {
                        if (c.userName == userName)
                        {
                            myCustomer.Add(c);
                            return RedirectToAction("Details", new { id = c.userName });
                        }
                    }
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Customer");
            }
        }

        // GET: Customer/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                string userName = Session["userName"].ToString();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Customer customer = db.customers.Find(id);
                if (customer == null || customer.userName != userName)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Customer");
            }
        }
        


        // GET: Customer/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            userName_tmp = id;
            CustomerEditView customer = GetCustomer(id);
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
        public ActionResult Edit(CustomerEditView model, string id)
        {
            var customers = db.customers.Include(c => c.profile);
            foreach (Customer c in customers)
            {
                if (c.profile.eMail == model.eMail && c.userName != model.userName)
                {
                    model.errorMessage = "E-mail already exists";
                    return View("Edit", model);
                }
            }

            if (ModelState.IsValid)
            {
                Profile profile = db.profiles.Find(userName_tmp);
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

        public CustomerEditView GetCustomer(string userName)
        {
            CustomerEditView cv = new CustomerEditView();
            var customer = db.customers.Find(userName);

            cv.userName = customer.userName;
            cv.eMail = customer.profile.eMail;
            cv.firstName = customer.profile.firstName;
            cv.surname = customer.profile.surname;
            cv.birthday = customer.profile.birthday;
            cv.countryUser = customer.countryUser;
            cv.descriptionUser = customer.profile.descriptionUser;

            return cv;
        }

        public ActionResult ChangePicture(Customer customer)
        {
            userName_tmp = customer.userName;
            return View(customer);
        }
        [HttpPost]
        public ActionResult ChangePicture(HttpPostedFileBase file)
        {
            Customer customer = db.customers.Find(userName_tmp);
            try
            {
                string fileName = customer.userName;
                string extension = Path.GetExtension(file.FileName);
                fileName += extension;
                customer.selectedPictureCustomer = "~/Content/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/"), fileName);
                file.SaveAs(fileName);
            }
            catch (Exception)
            {
                customer.selectedPictureCustomer = "~/Content/defaultUser.png";
            }
            db.Entry(customer).CurrentValues.SetValues(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeletePicture()
        {
            Customer customer = db.customers.Find(userName_tmp);
            customer.selectedPictureCustomer = "~/Content/defaultUser.png";
            db.Entry(customer).CurrentValues.SetValues(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
