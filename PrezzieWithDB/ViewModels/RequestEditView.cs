using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class RequestEditView
    {
        public string souvenirName { get; set; }
        public string countrySouv { get; set; }
        public int amount { get; set; }
        [RegularExpression(@"^(((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?$", ErrorMessage = "The Value has to be a floating Number with ',' as delimiter")]
        [Range(0, 10000)]
        public string price { get; set; }
        public string currency { get; set; }
        public string reward { get; set; }
        [DataType(DataType.MultilineText)]
        public string descriptionSouv { get; set; }
        public string status { get; set; }

    }
}