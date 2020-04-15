using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class SouvenirView
    {

        public int souvenirID { get; set; }
        public string souvenirName { get; set; }
        public string countrySouv { get; set; }

        public decimal price { get; set; }
        public string currency { get; set; }
        [DataType(DataType.MultilineText)]
        public string descriptionSouv { get; set; }

    }
}