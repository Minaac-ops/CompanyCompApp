using System;

namespace CompanyComp.Models
{
    public class Department
    {

        public string DName { get; set; }
        public int DNumber { get; set; }
        public Decimal MgrSSN { get; set; }
        public DateTime MgrStartDate { get; set; }
        public int EmpCount { get; set; }
    }
}