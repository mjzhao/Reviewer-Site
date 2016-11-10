using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Reviewer_Site.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        public Company Company { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name="Username")]
        public string UserID { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}