using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paye.Models
{
    public class UpdateApp
    {
        public string VersionName { get; set; }
        public string DownloadUrl { get; set; }
        public string Description { get; set; }
        public string Feepayable { get; set; }
    }
}