using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class RequestView
    {
        [Required]
        [MaxLength(10)]
        public int souvenirID { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string userName { get; set; }
        [Required]
        [Range(0, 100)]
        public int amount { get; set; }
        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "Reward must be between 0.01 and 10000.00")]
        public string reward { get; set; }
        [Required]
        public string status { get; set; }

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

        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(5)]
        [MaxLength(30)]
        public string eMail { get; set; }
        [Required]
        public string countryUser { get; set; }
        [MinLength(2)]
        [MaxLength(20)]
        public string firstName { get; set; }
        [MinLength(2)]
        [MaxLength(20)]
        public string surname { get; set; }
        [DataType(DataType.Date)]
        public string birthday { get; set; }
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        public string descriptionUser { get; set; }

    }
}