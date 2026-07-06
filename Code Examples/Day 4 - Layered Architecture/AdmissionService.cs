using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp7
{
    public class Course
    {
        public string CourseName { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
    }

    public class Student
    {
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class AdmissionRequest
    {
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CourseName {  get; set; } = string.Empty;
    }

    internal class Admission
    {
        public Student Student = new Student();
        public Course Course = new Course();
        public DateTime AdmissionDate { get; set; }
    }

    public class CourseRepository
    {
        private readonly List<Course> _courses =
        [
            new Course { CourseName = "C#", AvailableSeats = 2 },
            new Course { CourseName = "SQL", AvailableSeats = 0 },
            new Course { CourseName = "EF Core", AvailableSeats = 3 }
        ];

        public Course? GetCourseByName(string courseName) =>
            _courses.FirstOrDefault(c => c.CourseName.Equals(courseName, StringComparison.OrdinalIgnoreCase));

        public void ReduceSeat(Course course)
        {
            if (course.AvailableSeats > 0)
            {
                course.AvailableSeats--;
            }
        }
    }

    public class AdmissionRepository
    {
        public List<Admission> admissions = new List<Admission>();

        public void SaveAdmission(Admission admission)
        {
            admissions.Add(admission);
        }
    }

    // Mock DateFormatter for compilation
    public class DateFormatter {
        public string Format(DateTime date) => date.ToString();
    }

    // Mock Validator for compilation
    public class AdmissionValidator {
        public string? Validate(AdmissionRequest req) => null;
    }

    public class AdmissionService
    {
        private readonly AdmissionValidator validator;
        private readonly CourseRepository courseRepository;
        private readonly AdmissionRepository admissionRepository;
        private readonly DateFormatter dateFormatter;

        public AdmissionService()
        {
            validator = new AdmissionValidator();
            courseRepository = new CourseRepository();
            admissionRepository = new AdmissionRepository();
            dateFormatter = new DateFormatter();
        }

        public void AdmitStudent(AdmissionRequest request)
        {
            string? validationError = validator.Validate(request);

            if (validationError != null)
            {
                Console.WriteLine(validationError);
                return;
            }

            Course? course = courseRepository.GetCourseByName(request.CourseName);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            if (course.AvailableSeats <= 0)
            {
                Console.WriteLine("No seats available.");
                return;
            }

            Student student = new Student
            {
                StudentName = request.StudentName,
                Email = request.Email
            };

            Admission admission = new Admission
            {
                Student = student,
                Course = course,
                AdmissionDate = DateTime.Now
            };

            courseRepository.ReduceSeat(course);
            admissionRepository.SaveAdmission(admission);

            Console.WriteLine("Admission successful.");
            Console.WriteLine($"Student: {admission.Student.StudentName}");
            Console.WriteLine($"Email: {admission.Student.Email}");
            Console.WriteLine($"Course: {admission.Course.CourseName}");
            Console.WriteLine($"Admission Date: {dateFormatter.Format(admission.AdmissionDate)}");
        }
    }
}
