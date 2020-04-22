using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class CustomerEditView
    {
        public string userName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string eMail { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string firstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string surname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string birthday { get; set; }
        [Required]
        public string countryUser { get; set; }
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        public string descriptionUser { get; set; }
        public string errorMessage { get; set; }
    }
}