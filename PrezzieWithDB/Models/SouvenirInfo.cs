using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.Models
{
    public class SouvenirInfo
    {

        [Key]
        public int souvenirInfoID { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        [DataType(DataType.MultilineText)]
        public string descriptionSouv { get; set; }

        public virtual Souvenir souvenir { get; set; }

    }
}