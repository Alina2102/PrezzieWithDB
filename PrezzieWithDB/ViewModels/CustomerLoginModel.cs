using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class CustomerLoginModel
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        public string LoginErrorMessage { get; internal set; }
    }
}