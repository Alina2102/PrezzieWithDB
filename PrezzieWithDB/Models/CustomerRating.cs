using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.Models
{
    public class CustomerRating
    {
        [Key]
        public int profileRatingID { get; set; }
        public string userName { get; set; }
        public int ratingID { get; set; }


        public virtual Customer customer { get; set; }
        public virtual Rating rating { get; set; }
    }
}