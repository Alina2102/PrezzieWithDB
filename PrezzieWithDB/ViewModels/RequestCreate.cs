﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class RequestCreate
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string souvenirName { get; set; }
        [Required]
        public string countrySouv { get; set; }
        public int amount { get; set; }
        [RegularExpression(@"^(((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?$", ErrorMessage = "The Value has to be a floating Number with ',' as delimiter")]
        [Range(0, 10000)]
        public string price { get; set; }
        public string currency { get; set; }
        public string reward { get; set; }
        [DataType(DataType.MultilineText)]
        [MinLength(5)]
        [MaxLength(200)] 
        public string descriptionSouv { get; set; }
        [DisplayName("Upload Request Picture")]
        public string selectedPictureSouvenir { get; set; }
    }
}