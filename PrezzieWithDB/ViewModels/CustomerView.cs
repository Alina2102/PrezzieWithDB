using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class CustomerView
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string userName { get; set; }
        [Required]
        public string countryUser { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(5)]
        [MaxLength(30)]
        public string eMail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(3)]
        [MaxLength(30)]
        public string password { get; set; }
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
        public string errorMessage { get; set; }

    }
}