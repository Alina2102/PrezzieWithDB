using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrezzieWithDB.ViewModels
{
    public class SendMailModel
    {
        public string receiver { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }
}