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
        public int ratingValue { get; set; }
        public string ratingValueDescription { get; set; }

        public virtual ICollection<CustomerRating> customerRatings { get; set; }
    }
}