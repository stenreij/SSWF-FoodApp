using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int StudentNr { get; set; }   
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public int PhoneNr { get; set; }
        public City StudyCity { get; set; }

    }
}
