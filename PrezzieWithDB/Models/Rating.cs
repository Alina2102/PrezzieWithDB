using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.Models
{
    public class Rating
    {
        [Key]
        public int ratingID { get; set; }
        public string ratingDescription { get; set; }

        public virtual ICollection<CustomerRating> customerRatings { get; set; }
    }
}