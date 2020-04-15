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
        public int souvenirId { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }
        [DataType(DataType.MultilineText)]
        public string descriptionSouv { get; set; }

        public virtual Souvenir souvenir { get; set; }

    }
}