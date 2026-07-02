using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ConsoleApp3
{
    // ======================================
    // 1. Model
    // ======================================
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
    }

    // ======================================
    // 2. Repository
    // ======================================
    public class StudentRepository
    {
        private readonly string connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=EducationDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Student> GetAllStudents()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student";

            // TODO:
            // Use Dapper to fetch all students and convert the result into a List.
            // Example method to use: Query<Student>(query)
            // List<Student> students = ____________________________________________;

            return new List<Student>();
        }

        public Student? GetStudentById(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student WHERE Id = @Id";

            // TODO:
            // Use Dapper to return one student or null if not found.
            // Example method to use: QueryFirstOrDefault<Student>(query, new { Id = id })
            // Student? student = _________________________________________________;

            return null;
        }

        public void InsertStudent(Student student)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string query = @"INSERT INTO Student (FirstName, LastName, Age, Email, Course)
                             VALUES (@FirstName, @LastName, @Age, @Email, @Course)";

            // TODO:
            // Use Dapper Execute(...) and pass the student object.
            // _________________________________________________________________
        }

        public void UpdateStudent(Student student)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string query = @"UPDATE Student
                             SET FirstName = @FirstName,
                                 LastName = @LastName,
                                 Age = @Age,
                                 Email = @Email,
                                 Course = @Course
                             WHERE Id = @Id";

            // TODO:
            // Use Dapper Execute(...) and pass the student object.
            // _________________________________________________________________
        }

        public void DeleteStudent(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            string query = "DELETE FROM Student WHERE Id = @Id";

            // TODO:
            // Use Dapper Execute(...) with an anonymous object for Id.
            // _________________________________________________________________
        }
    }

    // ======================================
    // 3. Program Entry Point
    // ======================================
    public class Program
    {
        public static void Main()
        {
            StudentRepository repository = new StudentRepository();

            Console.WriteLine("All Students");
            Console.WriteLine("------------");

            // TODO:
            // Call GetAllStudents() and store the result.
            // List<Student> students = ____________________________________________;
            List<Student> students = new List<Student>();

            foreach (Student student in students)
            {
                Console.WriteLine($"{student.Id} - {student.FirstName} {student.LastName} - Age: {student.Age} - {student.Email} - {student.Course}");
            }

            Console.WriteLine();

            Console.WriteLine("Student By Id");
            Console.WriteLine("-------------");

            // TODO:
            // Call GetStudentById(...) with any id you want to test.
            // Student? selectedStudent = __________________________________________;
            Student? selectedStudent = null;

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

            // TODO:
            // Insert the new student.
            // _________________________________________________________________
            Console.WriteLine("Student inserted successfully.");

            Student updatedStudent = new Student
            {
                Id = 1,
                FirstName = "Aarav",
                LastName = "Sharma",
                Age = 21,
                Email = "aarav.sharma@example.com",
                Course = "Data Science"
            };

            // TODO:
            // Update the student with repository.UpdateStudent(...).
            // _________________________________________________________________
            Console.WriteLine("Student updated successfully.");

            // TODO:
            // Delete a student by id.
            // _________________________________________________________________
            Console.WriteLine("Student deleted successfully.");
        }
    }
}
