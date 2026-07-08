using WebApplication11.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication11.Services
{
    public class StudentService : IStudentService
    {
        private readonly List<Student> students =
        [
            new Student
            {
                StudentId = 1,
                StudentName = "Amit Sharma",
                Email = "amit@example.com",
                Course = "C#"
            },
            new Student
            {
                StudentId = 2,
                StudentName = "Sara Khan",
                Email = "sara@example.com",
                Course = "ASPDotNETCore"
            },
            new Student
            {
                StudentId = 3,
                StudentName = "John Roy",
                Email = "john@example.com",
                Course = "SQL"
            },
            new Student
            {
                StudentId = 4,
                StudentName = "Fatima Ali",
                Email = "fatima@example.com",
                Course = "ASPDotNETCore"
            }
        ];

        public List<Student> GetAllStudents()
        {
            return students;
        }

        public Student? GetStudentById(int studentId)
        {
            return students.FirstOrDefault(s => s.StudentId == studentId);
        }

        public List<Student> GetStudentsByCourse(string course)
        {
            return [.. students.Where(s => s.Course.Equals(course, StringComparison.OrdinalIgnoreCase))];
        }
    }
}
