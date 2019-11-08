using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Paye.Models
{
    public class postsWrapper
    {

        public postsWrapper()
        {
            if (string.IsNullOrEmpty(images))
                images = "null";
        }
        public string postId { get; set; }      
        public string title { get; set; }     
        public int subject { get; set; }
        public int city { get; set; }
        public bool isWoman { get; set; }
        public bool isImmediate { get; set; }
        public string cost { get; set; }       
        public string images { get; set; }
        public string createDate { get; set; }       
        public string timeToJoin { get; set; }
        public string tag { get; set; }        
        public string state { get; set; }
    }
}