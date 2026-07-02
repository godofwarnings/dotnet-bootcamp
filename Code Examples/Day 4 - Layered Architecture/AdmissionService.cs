using System;

namespace ConsoleApp7
{
    // Note: AdmissionValidator and DateFormatter classes were not provided in the snippet,
    // so this class expects them to exist in the same namespace.
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

    // Dummy classes added so code compiles conceptually.
    public class AdmissionValidator
    {
        public string? Validate(AdmissionRequest request) => null;
    }

    public class DateFormatter
    {
        public string Format(DateTime date) => date.ToString("yyyy-MM-dd");
    }
}
