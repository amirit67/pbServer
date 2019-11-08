using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paye.Models
{
    public class ReportWrapper
    {
        public string ComplainantId { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
        public int Type { get; set; }
    }
}