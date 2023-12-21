using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Ozbul.Application.Portal.Repository;
using Ozbul.Application.Portal.Api.Models;
using Ozbul.Application.Portal.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Ozbul.Application.Portal.Services.Abstractions;
using System.Text;

namespace Ozbul.Application.Portal.Api.Controllers
{
    // Controllers/StudentController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly DesKeyManager _desKeyManager;
        private readonly RSAHelper _rsaHelper;

        public StudentController(AppDbContext context, DesKeyManager desKeyManager, RSAHelper rsaHelper)
        {
            _context = context;
            _desKeyManager = desKeyManager;
            _rsaHelper = rsaHelper;
        }

        [HttpPost("addStaff")]
        public IActionResult AddStaff([FromForm] UserRequest model)
        {
         

            // Check if a user with the same email already exists (you may want to handle this based on your requirements)
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return Conflict("User with the provided email already exists");
            }

            // Create a new User entity
            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(newUser);
        }
        [HttpPost("add")]
        public IActionResult AddStudent([FromForm] StudentRequest input)
        {
            byte[] DESKey = _desKeyManager.GenerateDesKey();
            byte[] IV = _desKeyManager.GenerateRandomIV();
            if (input == null)
            {
                return BadRequest("Invalid student data");
            }

            // Create a new Student entity
            _context.Database.OpenConnection();
            try
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Students ON");
                
                var newStudent = new Student
                {
                    
                 
                    StudentId =input.StudentId,
                    Name = _desKeyManager.EncryptData(input.Name,DESKey,IV),
                    Surname = _desKeyManager.EncryptData(input.Surname, DESKey, IV),
                    DateOfBirth = _desKeyManager.EncryptDateTime(input.DateOfBirth, DESKey, IV),
                    DateOfRegistration = _desKeyManager.EncryptDateTime(input.DateOfRegistration, DESKey, IV),
                    Email = _desKeyManager.EncryptData(input.Email, DESKey, IV),
                    MothersName = _desKeyManager.EncryptData(input.MothersName, DESKey, IV ),
                    FathersName = _desKeyManager.EncryptData(input.FathersName, DESKey, IV),
                    DESKey= DESKey,
                    IV=IV,
                    // Add other properties as needed
                };


                _context.Students.Add(newStudent);
                _context.SaveChanges();
                return Ok(newStudent);
            }
            finally
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Students OFF");
                _context.Database.CloseConnection();
            }

       

        
        }
        [HttpGet("getAll")]
        public IActionResult GetAllStudents()
        {
            // Retrieve all students from the database
            var students = _context.Students.ToList();
            var decryptedStudents = new List<StudentInput>(); // Assuming StudentInput is a model for displaying data

            // Decrypt properties
            foreach (var student in students)
            {
                var newStudent = new StudentInput
                {
                    StudentId = student.StudentId,
                    Name = _desKeyManager.DecryptData(student.Name, student.DESKey,student.IV),
                    Surname = _desKeyManager.DecryptData(student.Surname, student.DESKey, student.IV),
                    DateOfBirth = _desKeyManager.DecryptDateTime(student.DateOfBirth, student.DESKey, student.IV),
                    DateOfRegistration = _desKeyManager.DecryptDateTime(student.DateOfRegistration, student.DESKey, student.IV),
                    Email = _desKeyManager.DecryptData(student.Email, student.DESKey, student.IV),
                    MothersName = _desKeyManager.DecryptData(student.MothersName, student.DESKey, student.IV),
                    FathersName = _desKeyManager.DecryptData(student.FathersName, student.DESKey, student.IV),
                    // Decrypt other properties as needed
                };

                decryptedStudents.Add(newStudent);
            }

            return Ok(decryptedStudents);
        }
        [HttpPost("addGrade")]
        public IActionResult AddGrade([FromForm] GradeRequest input)
        {
            string publicKey, privateKey;
            RSAHelper.GenerateKeyPair(out publicKey, out privateKey);
            if (input == null)
            {
                return BadRequest("Invalid grade data");
            }

            // Validate input data as needed

            // Create a new Grade entity
            var newGrade = new Grade
            {
                
                EncryptedCourseName = _rsaHelper.EncryptData( input.CourseName, publicKey),
                EncryptedCourseCode = _rsaHelper.EncryptData(input.CourseCode, publicKey),
                EncryptedGradeValue  =_rsaHelper.EncryptDouble(input.GradeValue, publicKey), 
                StudentId = input.StudentId,
                Credit = _rsaHelper.EncryptInt(input.Credit, publicKey), 
                PrivateKey= privateKey
                // Add other properties as needed
            };

            // Add the new grade to the database
            _context.Grades.Add(newGrade);
            _context.SaveChanges();

            return Ok(newGrade);
        }
        [HttpGet("{studentId}/grades")]
        public IActionResult GetStudentGrades(int studentId)
        {
            
            var encryptedData = _context.Grades.FirstOrDefault(e => e.StudentId == studentId);
            // Implement logic to retrieve grades for a specific student
            var decryptedDataResponse = new GradeRequest
            {
                CourseName = _rsaHelper.DecryptData(encryptedData.EncryptedCourseName, encryptedData.PrivateKey),
                CourseCode = _rsaHelper.DecryptData(encryptedData.EncryptedCourseCode, encryptedData.PrivateKey), 
                GradeValue = _rsaHelper.DecryptDouble(encryptedData.EncryptedGradeValue, encryptedData.PrivateKey),
                Credit = _rsaHelper.DecryptInt(encryptedData.Credit, encryptedData.PrivateKey),
                StudentId = studentId
            };

            return Ok(decryptedDataResponse);
        }
    }

}
