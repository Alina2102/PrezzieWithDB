using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class CustomerView
    {
        public string userName { get; set; }
        public string countryUser { get; set; }
        public string eMail { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime birthday { get; set; }
        public string descriptionUser { get; set; }
        [DisplayName("Upload Profile Picture")]
        public string selectedPictureCustomer { get; set; }
        public double rating { get; set; }
        public string ratingDescription { get; set; }
        public int ratingCount { get; set; }
        public string errorMessage { get; set; }

    }
}