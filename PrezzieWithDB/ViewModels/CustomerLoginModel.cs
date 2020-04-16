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
        [MinLength(3)]
        [MaxLength(20)]
        public string userName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(3)]
        [MaxLength(30)]
        public string password { get; set; }
        public string LoginErrorMessage { get; internal set; }
    }
}