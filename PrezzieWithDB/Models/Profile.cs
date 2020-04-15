using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.Models
{
    public class Profile
    {
        [Key]
        public string userName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string eMail { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        [DataType(DataType.Date)]
        public string birthday { get; set; }
        [DataType(DataType.MultilineText)]
        public string descriptionUser { get; set; }

        public virtual Customer customer { get; set; }
    }
}