using System;
using System.ComponentModel.DataAnnotations;

namespace challenge.Models
{
    public class Compensation
    {
        [Key]
        public string employee { get; set; }

        public decimal salary { get; set; }

        public DateTime effectiveDate { get; set; }
    }
}