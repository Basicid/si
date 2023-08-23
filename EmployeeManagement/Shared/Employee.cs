using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Shared
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public bool IsWorking { get; set; }
        public string? ImageUrl { get; set; }
        public List<Experience> Experiences { get; set; } = new List<Experience>();

    }

    public class Experience
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public string CompanyName { get; set; }
        public int EmployeeId { get; set; }
    }
}
