using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.Models
{
    public class Request
    {
        [Key]
        public int souvenirID { get; set; }
        public string userName { get; set; }
        public int amount { get; set; }
        public string reward { get; set; }
        public string status { get; set; }
        public string userNameDelivery { get; set; }

        [Required]
        public virtual Souvenir souvenir { get; set; }
        [Required]
        public virtual Customer customer { get; set; }
    }
}