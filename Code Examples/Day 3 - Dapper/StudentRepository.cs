using ConsoleApp4;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

public class StudentRepository
{
    private readonly string connectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=EducationDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public List<Student> GetAllStudents()
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student";

        List<Student> students = connection.Query<Student>(query).ToList();

        return students;
    }

    public Student? GetStudentById(int id)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student WHERE Id = @Id";

        Student? student = connection.QueryFirstOrDefault<Student>(query, new { Id = id });

        return student;
    }

    public void InsertStudent(Student student)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = @"INSERT INTO Student (FirstName, LastName, Age, Email, Course)
                         VALUES (@FirstName, @LastName, @Age, @Email, @Course)";

        connection.Execute(query, student);   
    }

    public void UpdateStudentEmail(int id, string email)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "UPDATE Student SET Email = @Email WHERE Id = @Id";

        connection.Execute(query, new {Email = email, Id = id});
    }

    public void DeleteStudent(int id)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "DELETE FROM Student WHERE Id = @Id";

        connection.Execute(query, new { Id = id });
    }
}
