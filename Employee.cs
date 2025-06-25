using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Final_Project
{
    public class Employee
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GenderIdentity { get; set; }
        public decimal GrossSalary { get; set; }
        public int BranchId { get; set; }
        public int SupervisorId { get; set; }
    }

}
