using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reviewer_Site.Models
{
    public class Company
    {
        public int CompanyID { get; set; }

        public string Name { get; set; }

        public string Website { get; set; }

        public List<Review> Reviews { get; set; }
    }
}