using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class SouvenirView
    {
        [Required]
        [MaxLength(10)]
        public int souvenirID { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string souvenirName { get; set; }
        [Required]
        public string countrySouv { get; set; }

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
        public decimal price { get; set; }
        public string currency { get; set; }
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        public string descriptionSouv { get; set; }

    }
}