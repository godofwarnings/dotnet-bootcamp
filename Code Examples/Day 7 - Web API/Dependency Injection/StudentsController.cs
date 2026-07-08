using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication11.Models;
using WebApplication11.Services;
using System.Collections.Generic;

namespace WebApplication11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            this._studentService = studentService;
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAllStudents()
        {
            List<Student> students = _studentService.GetAllStudents();

            return Ok(students);
        }

        [HttpGet("{studentId}")]
        public ActionResult<Student> GetStudentById(int studentId)
        {
            Student? student = _studentService.GetStudentById(studentId);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return Ok(student);
        }

        [HttpGet("course/{course}")]
        public ActionResult<List<Student>> GetStudentsByCourse(string course)
        {
            List<Student> students = _studentService.GetStudentsByCourse(course);

            if (students.Count == 0)
            {
                return NotFound("No students found for this course.");
            }

            return Ok(students);
        }
    }
}
