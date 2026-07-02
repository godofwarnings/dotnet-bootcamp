// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        List<string> courses = new List<string>
        {
            "C#",
            "SQL",
            "EF Core"
        };

        List<int> availableSeats =
        [
            2,
            0,
            3
        ];

        Console.WriteLine("Enter student name:");
        string studentName = Console.ReadLine()!;

        Console.WriteLine("Enter email:");
        string email = Console.ReadLine()!;

        Console.WriteLine("Enter course name:");
        string courseName = Console.ReadLine()!;

        if (string.IsNullOrWhiteSpace(studentName))
        {
            Console.WriteLine("Student name is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            Console.WriteLine("Valid email is required.");
            return;
        }

        int courseIndex = courses.IndexOf(courseName);

        if (courseIndex == -1)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        if (availableSeats[courseIndex] <= 0)
        {
            Console.WriteLine("No seats available.");
            return;
        }

        availableSeats[courseIndex]--;

        Console.WriteLine("Admission successful.");
        Console.WriteLine($"Student: {studentName}");
        Console.WriteLine($"Email: {email}");
        Console.WriteLine($"Course: {courseName}");
        Console.WriteLine($"Admission Date: {DateTime.Now}");
    }
}
