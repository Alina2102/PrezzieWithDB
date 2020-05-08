using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PrezzieWithDB.Models;

namespace PrezzieWithDB.ViewModels
{
    public class RequestContactModel
    {
        public int souvenirID { get; set; }
        public string userName { get; set; }
        public int amount { get; set; }
        public double reward { get; set; }
        public string status { get; set; }
        public string souvenirName { get; set; }
        public string countrySouv { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }
        public string descriptionSouv { get; set; }
        public string eMail { get; set; }
        public string countryUser { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        public string birthday { get; set; }
        public string descriptionUser { get; set; }
        [DisplayName("Upload Request Picture")]
        public string selectedPictureSouvenir { get; set; }
        public string selectedPictureCustomer { get; set; }
        public double rating { get; set; }
        public string ratingDescription { get; set; }
        public int ratingCount { get; set; }
        [DataType(DataType.MultilineText)]
        public string emailBody { get; set; }
        public string emailSubject { get; set; }
        public Customer customerSend { get; set; }
    }
}