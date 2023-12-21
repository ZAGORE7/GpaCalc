using global::Ozbul.Application.Portal.Repository.Entities;
using System;
using System.Collections.Generic;
namespace Ozbul.Application.Portal.Api.Models
{
    
   
    
        public class StudentInput
        {
            public int StudentId { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public DateTime DateOfBirth { get; set; }
            public DateTime DateOfRegistration { get; set; }
            public string Email { get; set; }
            public string MothersName { get; set; }
            public string FathersName { get; set; }
            public byte[] DESKey { get; set; }

            // Navigation property for grades
            public List<Grade> Grades { get; set; }
        }
    

}
