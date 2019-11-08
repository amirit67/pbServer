using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Paye.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public virtual User UserAccount { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}