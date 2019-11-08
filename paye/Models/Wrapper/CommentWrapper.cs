using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Paye.Models
{
    [Table("Comments", Schema = "dbo")]
    public class CommentModel
    {     
        public string userName { get; set; }
        public string comment { get; set; }             
    }
}