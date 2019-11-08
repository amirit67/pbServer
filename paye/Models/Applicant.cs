using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Paye.Models
{
    public class Applicant
    {     
        public string UserId { get; set; }
        public string ProfileImage { get; set; }             
    }
}