using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class CustomerChangePasswordView
    {
        public string userName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(3)]
        [MaxLength(30)]
        public string password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(3)]
        [MaxLength(30)]
        public string confirmPassword { get; set; }
    }
}