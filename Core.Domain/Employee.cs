using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeeNr { get; set; }
        public Location Location { get; set; }
    }
}
