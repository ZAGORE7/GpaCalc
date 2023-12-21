using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozbul.Application.Portal.Repository.Entities
{
    public class Grade
    {
        public int GradeId { get; set; }
        public byte[] EncryptedCourseName { get; set; }
        public byte[] EncryptedCourseCode { get; set; }
        public byte[] EncryptedGradeValue { get; set; }
        public int StudentId { get; set; }
        public byte[] Credit { get; set; }

        // Properties for RSA keys
       
        public string PrivateKey { get; set; }

        // Navigation property for students
        public Student Student { get; set; }
    }
}
