using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.Models
{
    public class Souvenir
    {

        [Key]
        public int souvenirId { get; set; }
        public string souvenirName { get; set; }
        public string countrySouv { get; set; }
        public string selectedPicture { get; set; }

        [Required]
        public virtual SouvenirInfo souvenirInfo { get; set; }

        public virtual Request request { get; set; } 
    }
}