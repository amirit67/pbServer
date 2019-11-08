using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paye.Models
{
    public class returnPostdetails
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public bool isWoman { get; set; }
        public string mobile { get; set; }
        public string telegram { get; set; }
        public string instagram { get; set; }
        public string soroosh { get; set; }
        public string gmail { get; set; }
        public string profileimage { get; set; }
        public int city { get; set; }
        public int subject { get; set; }
        public string title { get; set; }
        public string images { get; set; }
        public string createDate { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public List<string> applicants { get; set; }
        public List<string> comments { get; set; }
        public string state { get; set; }
        public List<string> baseProperty { get; set; }
    }
}