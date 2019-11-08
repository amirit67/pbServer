using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Paye.Models
{
    public class ParamsWrapper
    {
        public ParamsWrapper()
        {
            if (contentSearch == null)
                contentSearch = "";
        }
        public Guid PostId { get; set; }
        public int Skip { get; set; }      
        public Guid UserId { get; set; }     
        public int city { get; set; }
        public int subject { get; set; }
        public string ids { get; set; }
        public string contentSearch { get; set; }
        public int Status { get; set; }
    }
}