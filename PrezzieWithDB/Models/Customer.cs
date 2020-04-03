using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.Models
{
    public class Customer
    {
        [Key]
        public string userName { get; set; }
        public string countryUser { get; set; }

        [Required]
        public virtual Profile Profile { get; set; }

    }
}