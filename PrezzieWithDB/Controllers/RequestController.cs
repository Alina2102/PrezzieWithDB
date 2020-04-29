using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using PrezzieWithDB.DAL;
using PrezzieWithDB.Models;
using PrezzieWithDB.ViewModels;

namespace PrezzieWithDB.Controllers
{
    public class RequestController : Controller
    {
        private PrezzieContext db = new PrezzieContext();
        private static int? souvenierID_tmp = null;

        // GET: Request
        public ActionResult Index()
        {
            var requests = db.requests.Where(x => x.status == "new");
            requests = requests.OrderByDescending(r => r.souvenirID);
            ViewBag.sort = "Latest";
            ViewBag.New = true;
            ViewBag.inProgress = false;
            return View(requests.ToList());
        }

        [HttpPost]
        public ActionResult Index(string sorting, bool New, bool InProgress, string search)
        {
            var requests = db.requests.Include(r => r.customer).Include(r => r.souvenir);

            if (New == true && InProgress == false)
            {
                requests = db.requests.Where(x => x.status == "new");
                ViewBag.New = true;
                ViewBag.inProgress = false;
            }
            else if (New == true && InProgress == true)
            {
                requests = db.requests.Where(x => x.status == "in progress" || x.status == "new");
                ViewBag.New = true;
                ViewBag.inProgress = true;
            }
            else if (New == false && InProgress == true)
            {
                requests = db.requests.Where(x => x.status == "in progress");
                ViewBag.New = false;
                ViewBag.inProgress = true;
            }
            else
            {
                requests = db.requests.Where(x => x.status == "new");
                ViewBag.New = true;
                ViewBag.inProgress = false;
            }

            switch (sorting)
            {
                case "Souvenir":
                    requests = requests.OrderBy(r => r.souvenir.souvenirName);
                    break;
                case "Country":
                    requests = requests.OrderBy(r => r.souvenir.countrySouv);
                    break;
                case "Username":
                    requests = requests.OrderBy(r => r.userName);
                    break;
                default:
                    requests = requests.OrderByDescending(r => r.souvenirID);
                    break;
            }
            ViewBag.sort = sorting;

            if (search == "")
            {
                return View(requests.ToList());
            }
            else
            {
                List<Request> requests1 = requests.ToList();
                List<Request> searchRequests = new List<Request>();
                foreach (Request r in requests1)
                {
                    if (r.souvenir.souvenirName.ToLower().Contains(search.ToLower()))
                    {
                        searchRequests.Add(r);
                    }
                }
                return View(searchRequests.ToList());
            }
        }
        public ActionResult MyOwnRequests()
        {
            try
            {
                string userName = Session["userName"].ToString();
                var requests = db.requests.Include(r => r.customer).Include(r => r.souvenir);
                requests = requests.OrderByDescending(r => r.souvenirID);
                List<Request> myRequests = new List<Request>();

                foreach (Request r in requests)
                {
                    if (r.userName == userName)
                    {
                        myRequests.Add(r);
                    }
                }

                return View(myRequests.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Customer");
            }
        }

        [HttpPost]
        public ActionResult MyOwnRequests(string status)
        {
            string userName = Session["userName"].ToString();
            var requests = db.requests.Include(r => r.customer).Include(r => r.souvenir);
            requests = requests.OrderByDescending(r => r.souvenirID);
            switch (status)
            {
                case "InProgress":
                    requests = db.requests.Where(x => x.status == "in progress");
                    break;
                case "Done":
                    requests = db.requests.Where(x => x.status == "done");
                    break;
                default:
                    requests = db.requests.Where(x => x.status == "new");
                    break;
            }
            ViewBag.status = status;

            List<Request> myRequests = new List<Request>();

            foreach (Request r in requests)
            {
                if (r.userName == userName)
                {
                    myRequests.Add(r);
                }
            }
            return View(myRequests.ToList());
        }


        // GET: Request/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                string userName = Session["userName"].ToString();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                RequestView request = GetRequest(id);

                if (request == null)
                {
                    return HttpNotFound();
                }
                return View(request);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Customer");
            }
        }


        // GET: Request/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestCreate model, HttpPostedFileBase file)
        {
            Boolean isValid = true;

            ViewBag.ErrorMessageSouvenirName = null;
            ViewBag.ErrorMessageCountry = null;
            ViewBag.ErrorMessageAmount = null;
            ViewBag.ErrorMessagePrice = null;
            ViewBag.ErrorMessageCurrency = null;
            ViewBag.ErrorMessageReward = null;
            ViewBag.ErrorMessageDescription = null;

            if (model.souvenirName == null || model.souvenirName.Length < 3 || model.souvenirName.Length > 30)
            {
                ViewBag.ErrorMessageSouvenirName = "The name of your souvenir should have between 3 and 30 characters.";
                isValid = false;
            }
            if (model.countrySouv == null)
            {
                ViewBag.ErrorMessageCountry = "Please select a country.";
                isValid = false;
            }
            //Fehlermeldung von amount wird noch nicht angezeigt
            if (model.amount < 1 || model.amount > 1000)
            {
                ViewBag.ErrorMessageAmount = "Please enter an amount between 1 and 1000.";
                isValid = false;
            }
            if (model.price == null || model.price.Length > 7)
            {
                ViewBag.ErrorMessagePrice = "Please enter a price under 7 digits.";
                isValid = false;
            }
            if (model.currency == null)
            {
                ViewBag.ErrorMessageCurrency = "Please select a currency.";
                isValid = false;
            }
            if (model.reward == null)
            {
                ViewBag.ErrorMessageReward = "Please select the percentage of your award (the reward is calculated by price * amount * reward in %).";
                isValid = false;
            }
            if (model.descriptionSouv == null || model.descriptionSouv.Length > 300 || model.descriptionSouv.Length < 5)
            {
                ViewBag.ErrorMessageDescription = "Please enter a description of your souvenir, it should be more than 5 and less than 300 characters.";
                isValid = false;
            }

            if (isValid == true)
            {
                SouvenirInfo souvenirInfo = new SouvenirInfo();
                if (model.price == null)
                {
                    model.price = "0,0";
                }
                else
                if (model.price.Contains("."))
                {
                    model.price = model.price.Replace(".", ",");
                }
                souvenirInfo.price = Decimal.Parse(model.price);
                souvenirInfo.currency = model.currency;
                souvenirInfo.descriptionSouv = model.descriptionSouv;

                db.souvenirInfos.Add(souvenirInfo);
                db.SaveChanges();

                Souvenir souvenir = new Souvenir();

                try
                {
                    string fileName = souvenirInfo.souvenirId.ToString();
                    string extension = Path.GetExtension(file.FileName);
                    fileName += extension;
                    souvenir.selectedPictureSouvenir = "~/Content/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/"), fileName);
                    file.SaveAs(fileName);
                }
                catch (Exception)
                {
                    souvenir.selectedPictureSouvenir = "~/Content/defaultSouvenir.png";
                }
                souvenir.souvenirId = souvenirInfo.souvenirId;
                souvenir.souvenirName = model.souvenirName;
                souvenir.countrySouv = model.countrySouv;
                souvenir.souvenirInfo = souvenirInfo;

                db.souvenirs.Add(souvenir);
                db.SaveChanges();

                Customer customer = db.customers.Find(Session["userName"]);

                Request request = new Request();
                request.souvenir = souvenir;
                request.customer = customer;
                request.amount = model.amount;
                request.reward = model.reward;
                request.status = "new";
                request.souvenirID = souvenirInfo.souvenirId;

                db.requests.Add(request);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            else
            {
                return View("Create", model);
            }
        }

        // GET: Request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            souvenierID_tmp = id;
            Request request = db.requests.Find(id);

            if (request == null)
            {
                return HttpNotFound();
            }

            RequestEditView requestEdit = new RequestEditView();
            requestEdit.amount = request.amount;
            requestEdit.reward = request.reward;
            requestEdit.status = request.status;
            requestEdit.souvenirName = request.souvenir.souvenirName;
            requestEdit.countrySouv = request.souvenir.countrySouv;
            requestEdit.price = request.souvenir.souvenirInfo.price.ToString();
            requestEdit.currency = request.souvenir.souvenirInfo.currency;
            requestEdit.descriptionSouv = request.souvenir.souvenirInfo.descriptionSouv;

            return View(requestEdit);
        }

        // POST: Request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequestEditView model, int id)
        {
            Boolean isValid = true;

            ViewBag.ErrorMessageSouvenirName = null;
            ViewBag.ErrorMessageCountry = null;
            ViewBag.ErrorMessageAmount = null;
            ViewBag.ErrorMessagePrice = null;
            ViewBag.ErrorMessageCurrency = null;
            ViewBag.ErrorMessageReward = null;
            ViewBag.ErrorMessageDescription = null;
            ViewBag.ErrorMessageStatus = null;


            if (model.souvenirName == null || model.souvenirName.Length < 3 || model.souvenirName.Length > 30)
            {
                ViewBag.ErrorMessageSouvenirName = "The name of your souvenir should have between 3 and 30 characters.";
                isValid = false;
            }
            if (model.countrySouv == null)
            {
                ViewBag.ErrorMessageCountry = "Please select a country.";
                isValid = false;
            }
            if (model.amount < 1 || model.amount > 1000)
            {
                ViewBag.ErrorMessageAmount = "Please enter an amount between 1 and 1000.";
                isValid = false;
            }
            if (model.price == null || model.price.Length > 7)
            {
                ViewBag.ErrorMessagePrice = "Please enter a price under 7 digits.";
                isValid = false;
            }
            if (model.currency == null)
            {
                ViewBag.ErrorMessageCurrency = "Please select a currency.";
                isValid = false;
            }
            if (model.reward == null)
            {
                ViewBag.ErrorMessageReward = "Please select the percentage of your award (the reward is calculated by price * amount * reward in %).";
                isValid = false;
            }
            if (model.descriptionSouv == null || model.descriptionSouv.Length > 300 || model.descriptionSouv.Length < 5)
            {
                ViewBag.ErrorMessageDescription = "Please enter a description of your souvenir, it should be more than 5 and less than 300 characters.";
                isValid = false;
            }
            if (model.status == null)
            {
                ViewBag.ErrorMessageStatus = "Please select a status.";
                isValid = false;
            }

            if (isValid == true)
            {
                SouvenirInfo souvenirInfo = db.souvenirInfos.Find(souvenierID_tmp);
                if (model.price == null)
                {
                    model.price = "0,0";
                }
                else if (model.price.Contains("."))
                {
                    model.price = model.price.Replace(".", ",");
                }
                souvenirInfo.price = Decimal.Parse(model.price);
                souvenirInfo.currency = model.currency;
                souvenirInfo.descriptionSouv = model.descriptionSouv;
                db.Entry(souvenirInfo).CurrentValues.SetValues(souvenirInfo);
                db.SaveChanges();

                Souvenir souvenir = db.souvenirs.Find(souvenierID_tmp);
                souvenir.souvenirName = model.souvenirName;
                souvenir.countrySouv = model.countrySouv;
                db.Entry(souvenir).CurrentValues.SetValues(souvenir);
                db.SaveChanges();

                Request request = db.requests.Find(souvenierID_tmp);
                request.amount = model.amount;
                request.reward = model.reward;
                request.status = model.status;
                db.Entry(request).CurrentValues.SetValues(request);
                db.SaveChanges();

                souvenierID_tmp = null;
                return RedirectToAction("MyOwnRequests");
            }
            else
            {
                return View("Edit", model);
            }
        }

        // GET: Request/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Request request = db.requests.Find(id);
            Customer customer = request.customer;
            /////////////////////////////////////////////////
            var body = "<p>Dear " + customer.profile.firstName + ",</p> " + "<p></p><p>we had to delete your request " + request.souvenir.souvenirName + " with the request ID: " + request.souvenirID + " because it was against our business security rules</p><p></p><p>With kind Regards</p><p>Your Prezzie Team</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(customer.profile.eMail));  // replace with valid value 
            message.From = new MailAddress("prezzie.info@gmail.com");  // replace with valid value
            message.Subject = "Prezzie - your Request " + request.souvenir.souvenirName + " was deleted";
            message.Body = body;
            message.IsBodyHtml = true;

            db.requests.Remove(request);
            db.SaveChanges();

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "prezzie.info@gmail.com",
                    Password = "A1b2C3D$"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return RedirectToAction("Sent");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public RequestView GetRequest(int? souvenirID)
        {
            RequestView rv = new RequestView();
            var request = db.requests.Find(souvenirID);

            rv.souvenirID = request.souvenirID;
            rv.userName = request.userName;
            rv.amount = request.amount;
            rv.reward = request.reward;
            rv.status = request.status;

            rv.souvenirName = request.souvenir.souvenirName;
            rv.countrySouv = request.souvenir.countrySouv;

            rv.price = request.souvenir.souvenirInfo.price;
            rv.currency = request.souvenir.souvenirInfo.currency;
            rv.descriptionSouv = request.souvenir.souvenirInfo.descriptionSouv;

            var customer = db.customers.Find(rv.userName);

            rv.eMail = customer.profile.eMail;
            rv.countryUser = customer.countryUser;
            rv.firstName = customer.profile.firstName;
            rv.surname = customer.profile.surname;
            rv.birthday = customer.profile.birthday;
            rv.descriptionUser = customer.profile.descriptionUser;

            try
            {
                List<CustomerRating> customerRatings = new List<CustomerRating>();
                customerRatings = customer.customerRatings.ToList();

                foreach (CustomerRating cr in customerRatings)
                {
                    rv.rating += cr.ratingID;
                }
                rv.ratingCount = customerRatings.Count;
                rv.rating /= rv.ratingCount;
                rv.rating = Math.Round(rv.rating, 1);
                int ratingRounded = (int)rv.rating;
                rv.ratingDescription = db.ratings.Find(ratingRounded).ratingDescription;
            }
            catch (Exception)
            {
                rv.ratingCount = 0;
                rv.rating = 0;
                rv.ratingDescription = "no Ratings";
            }


            return rv;
        }

        public ActionResult ChangePicture(int? souvenirID)
        {
            souvenierID_tmp = souvenirID;
            Request request = db.requests.Find(souvenirID);
            return View(request);
        }
        [HttpPost]
        public ActionResult ChangePicture(HttpPostedFileBase file)
        {
            Request request = db.requests.Find(souvenierID_tmp);
            try
            {
                string fileName = request.souvenirID.ToString();
                string extension = Path.GetExtension(file.FileName);
                fileName += extension;
                request.souvenir.selectedPictureSouvenir = "~/Content/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/"), fileName);
                file.SaveAs(fileName);
            }
            catch (Exception)
            {
                request.souvenir.selectedPictureSouvenir = "~/Content/defaultSouvenir.png";
            }
            db.Entry(request).CurrentValues.SetValues(request);
            db.SaveChanges();
            souvenierID_tmp = null;
            return RedirectToAction("MyOwnRequests");
        }
        public ActionResult DeletePicture()
        {
            Request request = db.requests.Find(souvenierID_tmp);
            request.souvenir.selectedPictureSouvenir = "~/Content/defaultSouvenir.png";
            db.Entry(request).CurrentValues.SetValues(request);
            db.SaveChanges();
            souvenierID_tmp = null;
            return RedirectToAction("MyOwnRequests");
        }

        public ActionResult Sent()
        {
            return View();
        }

        public ActionResult FullFilled(int id, string userNameDelivery)
        {
            souvenierID_tmp = id;
            Customer customer = new Customer();
            customer.userName = userNameDelivery;
            return View(customer);
        }
        [HttpPost]
        public ActionResult FullFilled(int stars)
        {
            Request request = db.requests.Find(souvenierID_tmp);
            Customer customer = db.customers.Find(request.userNameDelivery);
            request.status = "done";
            CustomerRating customerRating = new CustomerRating();
            customerRating.customer = customer;

            Rating rating = db.ratings.Find(stars);

            customerRating.rating = rating;
            db.customerRatings.Add(customerRating);
            db.Entry(request).CurrentValues.SetValues(request);
            db.Entry(customer).CurrentValues.SetValues(customer);

            db.SaveChanges();

            return RedirectToAction("MyOwnRequests");
        }

        public ActionResult SetNew(int id)
        {
            Request request = db.requests.Find(id);
            request.status = "new";
            request.userNameDelivery = null;
            db.Entry(request).CurrentValues.SetValues(request);
            db.SaveChanges();

            return RedirectToAction("MyOwnRequests");
        }
        [HttpGet]
        public ActionResult ContactMe(int souvenirID, string souvenirName,string countrySouv, string eMail, string firstName, string surname)
        {
            RequestContactModel rcm = new RequestContactModel();
            rcm.souvenirID = souvenirID;
            rcm.souvenirName = souvenirName;
            rcm.countrySouv = countrySouv;
            rcm.eMail = eMail;
            rcm.firstName = firstName;
            rcm.surname = surname;
            rcm.customerSend = db.customers.Find(Session["userName"].ToString());

            rcm.emailSubject = "Prezzie - an user wants to bring you your request: " + rcm.souvenirName;
            rcm.emailBody = "Hello " + rcm.firstName + "," + Environment.NewLine + Environment.NewLine + "I am " + rcm.customerSend.profile.firstName + " and I saw your request: " + rcm.souvenirName + "." + Environment.NewLine + "I am going to " + rcm.countrySouv + " and can bring it to you." + Environment.NewLine + "So let me know if it's okay for you :-) " + Environment.NewLine + "You can contact me under: " + rcm.customerSend.profile.eMail + "." + Environment.NewLine + Environment.NewLine + "Kind regards " + Environment.NewLine + rcm.customerSend.profile.firstName + " " + rcm.customerSend.profile.surname;
            
            return View(rcm);
        }

        [HttpPost]
        public async Task<ActionResult> ContactMe(RequestContactModel rcm)
        {

            var message = new MailMessage();
            message.To.Add(new MailAddress(rcm.eMail));
            message.From = new MailAddress("prezzie.info@gmail.com");
            message.Subject = rcm.emailSubject;
            message.Body = rcm.emailBody;
            message.IsBodyHtml = true;

            Request request = db.requests.Find(rcm.souvenirID);
            request.status = "in progress";
            request.userNameDelivery = rcm.userName;
            db.Entry(request).CurrentValues.SetValues(request);
            db.SaveChanges();

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "prezzie.info@gmail.com",
                    Password = "A1b2C3D$"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return RedirectToAction("Sent");
            }
        }
    }
}
