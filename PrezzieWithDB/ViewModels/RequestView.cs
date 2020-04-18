using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class RequestView
    {
        public int souvenirID { get; set; }
        public string userName { get; set; }
        public int amount { get; set; }
        public string reward { get; set; }
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
        public string selectedPicture { get; set; }
        public bool selectedInProgress { get; set; }
        public bool selectedNew { get; set; }
        public bool selectedDone { get; set; }
    }
}