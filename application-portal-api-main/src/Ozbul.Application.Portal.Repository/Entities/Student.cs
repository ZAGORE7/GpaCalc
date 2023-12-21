using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozbul.Application.Portal.Repository.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public byte[] Name { get; set; }
        public byte[] Surname { get; set; }
        public byte[] DateOfBirth { get; set; }
        public byte[] DateOfRegistration { get; set; }
        public byte[] Email { get; set; }

        public byte[] MothersName { get; set; }
        public byte[] FathersName { get; set; }
        public byte[] DESKey { get; set; }
        public byte[] IV { get; set; }

        // Navigation property for grades
        public List<Grade> Grades { get; set; }
    }

}
