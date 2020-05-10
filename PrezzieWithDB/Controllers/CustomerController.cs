using System;
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
        public ActionResult SignUp(CustomerSignUpModel model, HttpPostedFileBase file)
        {
            Boolean isValid = true;

            ViewBag.ErrorMessageUsername= null;
            ViewBag.ErrorMessagePassword = null;
            ViewBag.ErrorMessageEmail = null;
            ViewBag.ErrorMessageFirstName = null;
            ViewBag.ErrorMessageSurName = null;
            ViewBag.ErrorMessageBirthday = null;
            ViewBag.ErrorMessageCountry = null;
            ViewBag.ErrorMessageDescription = null;

            if (db.customers.Find(model.userName) != null)
            {
                model.errorMessage = "Username already exists.";
                return View("SignUp", model);
            }
            var customers = db.customers.Include(c => c.profile);
            foreach (Customer c in customers)
            {
                if (c.profile.eMail == model.eMail)
                {
                    model.errorMessage = "E-mail already exists.";
                    return View("SignUp", model);
                }
            }
            if (model.userName == null || model.userName.Length < 3 || model.userName.Length > 30)
            {
                ViewBag.ErrorMessageUsername = "Please enter an username between 3 and 30 characters.";
                isValid = false;
            }
            if (model.password == null || model.password.Length < 3 || model.password.Length > 30)
            {
                ViewBag.ErrorMessagePassword = "Please a password between 3 and 30 characters.";
                isValid = false;
            }
            if (model.eMail == null || model.eMail.Length < 3 || model.eMail.Length > 30)
            {
                ViewBag.ErrorMessageEmail = "Please enter an email address between 3 and 30 characters.";
                isValid = false;
            }
            if (model.firstName == null || model.firstName.Length < 4 || model.firstName.Length > 30)
            {
                ViewBag.ErrorMessageFirstName = "Please enter a valid first name between 3 and 30 characters.";
                isValid = false;
            }
            if (model.surname == null || model.surname.Length < 4 || model.surname.Length > 30)
            {
                ViewBag.ErrorMessageSurName = "Please enter a valid surname between 3 and 30 characters.";
                isValid = false;
            }
            if (model.birthday == null || model.birthday.ToString() == "01.01.0001 00:00:00")
            {
                ViewBag.ErrorMessageBirthday = "Please enter a birthday.";
                isValid = false;
            }
            else
            {
                DateTime today = DateTime.Now;
                var diff = (today - model.birthday).TotalDays;
                if (diff < 6570)
                {
                    ViewBag.ErrorMessageBirthday = "You have to be 18 Years old to register on this homepage.";
                    isValid = false;
                }
                if (diff > 43800)
                {
                    ViewBag.ErrorMessageBirthday = "Your birth date would mean you are over 120 years old. Sorry that's not possible. :-)";
                    isValid = false;
                }
            }
            if (model.countryUser == null)
            {
                ViewBag.ErrorMessageCountry = "Please select a country.";
                isValid = false;
            }
            if (model.descriptionUser != null && model.descriptionUser.Length > 300)
            {
                ViewBag.ErrorMessageDescription = "The maximum capacity are 300 characters.";
                isValid = false;
            }

            if (isValid == true)
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

            else
            {
                return View("SignUp", model);
            }
            
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
                CustomerView cv = new CustomerView();
                cv.birthday = customer.profile.birthday;
                cv.countryUser = customer.countryUser;
                cv.descriptionUser = customer.profile.descriptionUser;
                cv.eMail = customer.profile.eMail;
                cv.firstName = customer.profile.firstName;
                cv.surname = customer.profile.surname;
                cv.selectedPictureCustomer = customer.selectedPictureCustomer;
                cv.userName = customer.userName;
                cv.password = customer.profile.password;

                try
                {
                    List<CustomerRating> customerRatings = new List<CustomerRating>();
                    customerRatings = customer.customerRatings.ToList();

                    foreach (CustomerRating cr in customerRatings)
                    {
                        cv.rating += cr.rating.ratingValue;
                    }
                    cv.ratingCount = customerRatings.Count;
                    cv.rating /= cv.ratingCount;
                    cv.rating = Math.Round(cv.rating, 1);
                    int ratingRounded = (int)cv.rating;
                    cv.ratingDescription = db.ratings.Find(ratingRounded).ratingValueDescription;
                }
                catch (Exception)
                {
                    cv.ratingCount = 0;
                    cv.rating = 0;
                    cv.ratingDescription = "no Ratings";
                }
                if (customer == null || customer.userName != userName)
                {
                    return HttpNotFound();
                }
                return View(cv);
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
            Boolean isValid = true;

            ViewBag.ErrorMessageEmail = null;
            ViewBag.ErrorMessageFirstName = null;
            ViewBag.ErrorMessageSurName = null;
            ViewBag.ErrorMessageBirthday = null;
            ViewBag.ErrorMessageCountry = null;
            ViewBag.ErrorMessageDescription = null;
           
            if(model.eMail == null)
            {
                ViewBag.ErrorMessageEmail = "Please enter an email address.";
                isValid = false;
            }
            else
            {
                var customers = db.customers.Include(c => c.profile);
                foreach (Customer c in customers)
                {
                    if (c.profile.eMail == model.eMail && c.userName != model.userName)
                    {
                        ViewBag.ErrorMessageEmail = "E-mail already exists.";
                        isValid = false;
                    }
                }
            }

            if (model.firstName == null || model.firstName.Length < 4 || model.firstName.Length > 30)
            {
                ViewBag.ErrorMessageFirstName = "Please enter a valid first name between 3 and 30 characters.";
                isValid = false;
            }
            if (model.surname == null || model.surname.Length < 4 || model.surname.Length > 30)
            {
                ViewBag.ErrorMessageSurName = "Please enter a valid surname between 3 and 30 characters.";
                isValid = false;
            }
            if (model.birthday == null || model.birthday.ToString() == "01.01.0001 00:00:00")
            {
                ViewBag.ErrorMessageBirthday = "Please enter a birthday.";
                isValid = false;
            }
            else {
                DateTime today = DateTime.Now;
                var diff = (today - model.birthday).TotalDays;
            if (diff < 6570)
            {
                ViewBag.ErrorMessageBirthday = "You have to be 18 Years old to register on this homepage";
                isValid = false;
            }
            if (diff > 43800)
            {
                ViewBag.ErrorMessageBirthday = "Your birth date would mean you are over 120 years old. Sorry that's not possible :-)";
                isValid = false;
            }
            }
            if (model.countryUser == null)
            {
                ViewBag.ErrorMessageCountry = "Please select a country.";
                isValid = false;
            }
            if (model.descriptionUser != null && model.descriptionUser.Length > 300)
            {
                ViewBag.ErrorMessageDescription = "The maximum capacity are 300 characters.";
                isValid = false;
            }

            if(isValid == false)
            {
            return View("Edit", model);
            }

            else
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
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                string userName = Session["userName"].ToString();
                if (userName != "Admin")
                {
                    return RedirectToAction("Index", "Home");
                }
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
            catch (Exception)
            {
                return RedirectToAction("Login", "Customer");
            }
    }
                                    
    // POST: Customer/Delete/5
    [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {


            Customer customer = db.customers.Find(id);
            foreach(CustomerRating cr in customer.customerRatings.ToList())
            {
                db.customerRatings.Remove(cr);
            }
            db.SaveChanges();
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

        public ActionResult ChangePicture(string userName)
        {
            if (userName == null)
            {
                return RedirectToAction("Index");
            }
            userName_tmp = userName;
            Customer customer = db.customers.Find(userName);
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
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(CustomerChangePasswordView model)
        {
            
            Boolean isValid = true;

            ViewBag.ErrorMessagePassword = null;
            ViewBag.ErrorMessageConfirmPassword = null;


            if (model.password == null || model.password.Length < 3 || model.password.Length > 30)
            {
                ViewBag.ErrorMessagePassword = "Please enter a password between 3 and 30 characters.";
                isValid = false;
            }

            if (model.confirmPassword == null)
            {
                ViewBag.ErrorMessageConfirmPassword = "Please confirm the password.";
                isValid = false;
            }

            if (model.password != model.confirmPassword)
            {
                ViewBag.ErrorMessageConfirmPassword = "Please make sure your passwords match.";
                isValid = false;
            }


            if (isValid == true)
            {

                model.userName = Session["userName"].ToString();
                
                Profile profile = db.profiles.Find(model.userName);

                profile.password = model.password;
                db.Entry(profile).CurrentValues.SetValues(profile);
                db.SaveChanges();

                return RedirectToAction("Index");
                }
            else
            {
                return View("ChangePassword", model);
            }
        }
    }
}
