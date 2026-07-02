using ConsoleApp4;
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        StudentRepository repository = new StudentRepository();

        Console.WriteLine("All Students");
        Console.WriteLine("------------");

        List<Student> students = repository.GetAllStudents();

        foreach (Student student in students)
        {
            Console.WriteLine($"{student.Id} - {student.FirstName} {student.LastName} - Age: {student.Age} - {student.Email} - {student.Course}");
        }

        Console.WriteLine();

        Console.WriteLine("Student By Id");
        Console.WriteLine("-------------");

        Student? selectedStudent = repository.GetStudentById(1);

        if (selectedStudent != null)
        {
            Console.WriteLine($"{selectedStudent.Id} - {selectedStudent.FirstName} {selectedStudent.LastName} - Age: {selectedStudent.Age} - {selectedStudent.Email} - {selectedStudent.Course}");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }

        Console.WriteLine();

        Student newStudent = new Student
        {
            FirstName = "Ishaan",
            LastName = "Kapoor",
            Age = 24,
            Email = "ishaan.kapoor@example.com",
            Course = "Biology"
        };

        //repository.InsertStudent(newStudent);
        //Console.WriteLine("Student inserted successfully.");

        repository.UpdateStudentEmail(1, "arav.sharma@example.com");

        if (selectedStudent != null)
        {
            Console.WriteLine($"{selectedStudent.Id} - {selectedStudent.FirstName} {selectedStudent.LastName} - Age: {selectedStudent.Age} - {selectedStudent.Email} - {selectedStudent.Course}");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }

        Console.WriteLine("Student updated successfully.");

        repository.DeleteStudent(1);
        Console.WriteLine("Student deleted successfully.");
    }
}
