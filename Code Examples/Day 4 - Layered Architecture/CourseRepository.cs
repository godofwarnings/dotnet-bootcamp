using System.Collections.Generic;
using System.Linq;
using System;

namespace ConsoleApp7
{
    public class CourseRepository
    {
        // use readonly to prevent accidental reassignment
        private readonly List<Course> _courses =
        [
            new Course { CourseName = "C#", AvailableSeats = 2 },
            new Course { CourseName = "SQL", AvailableSeats = 0 },
            new Course { CourseName = "EF Core", AvailableSeats = 3 }
        ];

        // Expression-bodied member for cleaner syntax
        public Course? GetCourseByName(string courseName) =>
            _courses.FirstOrDefault(c => c.CourseName.Equals(courseName, StringComparison.OrdinalIgnoreCase));

        public void ReduceSeat(Course course)
        {
            // Add a simple guard clause to prevent negative seats
            if (course.AvailableSeats > 0)
            {
                course.AvailableSeats--;
            }
        }
    }
}
