using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EmployeeEFRepo : IEmployeeRepo
    {
        public Employee GetEmployeeByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Employee GetEmployeeById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetEmployees()
        {
            throw new NotImplementedException();
        }
    }
}
