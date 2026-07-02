Do the following for me.

I've attached some code i've used before. write something along those lines. also leave empty space in code where in i'll write code by myself and give to to you for final review. this is for my learning purposes. Also give an sql query to make a table and insert data into it. i've attached example of that as well

```csharp
using ConsoleApp3;
using Dapper;
using Microsoft.Data.SqlClient;

public class ProductRepository
{
    private readonly string connectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=RetailDb;Trusted_Connection=True;TrustServerCertificate=True;";

    // transformed into Dapper

    public List<Product> GetAllProducts()
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "SELECT Id, Name, Category, Price, Stock FROM Products";

        List<Product> products = connection.Query<Product>(query).ToList();

        return products;
    }

    public Product? GetProductById(int id)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "SELECT Id, Name, Category, Price, Stock FROM Products WHERE Id = @Id";

        Product? product = connection.QueryFirstOrDefault<Product>(query, new { Id = id });

        return product;
    }

    public void InsertProduct(Product product)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "INSERT INTO Products (Name, Category, Price, Stock) VALUES (@Name, @Category, @Price, @Stock)";

        connection.Execute(query, product);
    }

    public void UpdateProductStock(int id, int stock)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "UPDATE Products SET Stock = @Stock WHERE Id = @Id";

        connection.Execute(query, new {Id = id, Stock = stock});
    }

    public void DeleteProduct(int id)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "DELETE FROM Products WHERE Id = @Id";

        connection.Execute(query, new { Id = id});
    }

    // without Dapper
    //public List<Product> GetAllProducts()
    //{
    //    List<Product> products = new List<Product>();

    //    using SqlConnection connection = new SqlConnection(connectionString);

    //    connection.Open();

    //    string query = "SELECT Id, Name, Category, Price, Stock FROM Products";

    //    using SqlCommand command = new SqlCommand(query, connection);

    //    using SqlDataReader reader = command.ExecuteReader();

    //    while (reader.Read())
    //    {
    //        Product product = new Product
    //        {
    //            Id = Convert.ToInt32(reader["Id"]),
    //            Name = reader["Name"].ToString()!,
    //            Category = reader["Category"].ToString()!,
    //            Price = Convert.ToDecimal(reader["Price"]),
    //            Stock = Convert.ToInt32(reader["Stock"])
    //        };

    //        products.Add(product);
    //    }

    //    return products;
    //}

    //public Product? GetProductById(int id)
    //{
    //    using SqlConnection connection = new SqlConnection(connectionString);

    //    connection.Open();

    //    string query = "SELECT Id, Name, Category, Price, Stock FROM Products WHERE Id = @Id";

    //    using SqlCommand command = new SqlCommand(query, connection);

    //    command.Parameters.AddWithValue("@Id", id);

    //    using SqlDataReader reader = command.ExecuteReader();

    //    if (reader.Read())
    //    {
    //        return new Product
    //        {
    //            Id = Convert.ToInt32(reader["Id"]),
    //            Name = reader["Name"].ToString()!,
    //            Category = reader["Category"].ToString()!,
    //            Price = Convert.ToDecimal(reader["Price"]),
    //            Stock = Convert.ToInt32(reader["Stock"])
    //        };
    //    }

    //    return null;
    //}

    //public void InsertProduct(Product product)
    //{
    //    using SqlConnection connection = new SqlConnection(connectionString);

    //    connection.Open();

    //    string query = "INSERT INTO Products (Name, Category, Price, Stock) VALUES (@Name, @Category, @Price, @Stock)";

    //    using SqlCommand command = new SqlCommand(query, connection);

    //    command.Parameters.AddWithValue("@Name", product.Name);
    //    command.Parameters.AddWithValue("@Category", product.Category);
    //    command.Parameters.AddWithValue("@Price", product.Price);
    //    command.Parameters.AddWithValue("@Stock", product.Stock);

    //    command.ExecuteNonQuery();
    //}

    //public void UpdateProductStock(int id, int newStock)
    //{
    //    using SqlConnection connection = new SqlConnection(connectionString);

    //    connection.Open();

    //    string query = "UPDATE Products SET Stock = @Stock WHERE Id = @Id";

    //    using SqlCommand command = new SqlCommand(query, connection);

    //    command.Parameters.AddWithValue("@Stock", newStock);
    //    command.Parameters.AddWithValue("@Id", id);

    //    command.ExecuteNonQuery();
    //}

    //public void DeleteProduct(int id)
    //{
    //    using SqlConnection connection = new SqlConnection(connectionString);

    //    connection.Open();

    //    string query = "DELETE FROM Products WHERE Id = @Id";

    //    using SqlCommand command = new SqlCommand(query, connection);

    //    command.Parameters.AddWithValue("@Id", id);

    //    command.ExecuteNonQuery();
    //}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}

using ConsoleApp3;
using System;

public class Program
{
    public static void Main()
    {
        ProductRepository repository = new ProductRepository();

        Console.WriteLine("All Products");
        Console.WriteLine("------------");

        List<Product> products = repository.GetAllProducts();

        foreach (Product product in products)
        {
            Console.WriteLine($"{product.Id} - {product.Name} - {product.Category} - {product.Price} - Stock: {product.Stock}");
        }

        Console.WriteLine();

        Console.WriteLine("Product By Id");
        Console.WriteLine("-------------");

        Product? selectedProduct = repository.GetProductById(4);

        if (selectedProduct != null)
        {
            Console.WriteLine($"{selectedProduct.Id} - {selectedProduct.Name} - {selectedProduct.Category} - {selectedProduct.Price} - Stock: {selectedProduct.Stock}");
        }
        else
        {
            Console.WriteLine("Product not found.");
        }

        Console.WriteLine();

        Product newProduct = new Product
        {
            Name = "Scanner",
            Category = "Electronics",
            Price = 6500,
            Stock = 6
        };

        repository.InsertProduct(newProduct);
        Console.WriteLine("Product inserted successfully.");

        repository.UpdateProductStock(1, 30);
        Console.WriteLine("Product stock updated successfully.");

        repository.DeleteProduct(6);
        Console.WriteLine("Product deleted successfully.");
    }
}
```

```sql
CREATE DATABASE RetailDb;
GO

USE RetailDb;
GO

CREATE TABLE Products
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100),
    Category VARCHAR(100),
    Price DECIMAL(10,2),
    Stock INT
);
GO

INSERT INTO Products (Name, Category, Price, Stock)
VALUES
('Laptop', 'Electronics', 55000, 10),
('Mouse', 'Electronics', 700, 50),
('Keyboard', 'Electronics', 1500, 25),
('Chair', 'Furniture', 3500, 8),
('Table', 'Furniture', 7500, 5);
GO

SELECT * FROM Products;
```


Based on your screenshot, the task is to create `EducationDB`, create a `Student` table with inserted data in MSSQL, and then connect to it from C# using Dapper with the Repository Pattern and full CRUD operations . Since the screenshot says you can decide the columns, I chose `FirstName`, `LastName`, `Age`, `Email`, and `Course` for the `Student` table .

Below is a practice version with `TODO` sections left for you to complete yourself. Fill those in and send it back to me for final review.

### SQL Script

I used `Student` as the table name to match the screenshot requirement .

```sql
CREATE DATABASE EducationDB;
GO

USE EducationDB;
GO

CREATE TABLE Student
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Age INT NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Course VARCHAR(100) NOT NULL
);
GO

INSERT INTO Student (FirstName, LastName, Age, Email, Course)
VALUES
('Aarav', 'Sharma', 20, 'aarav.sharma@example.com', 'Computer Science'),
('Diya', 'Patel', 21, 'diya.patel@example.com', 'Mathematics'),
('Rohan', 'Verma', 22, 'rohan.verma@example.com', 'Physics'),
('Anaya', 'Singh', 19, 'anaya.singh@example.com', 'Chemistry'),
('Kabir', 'Mehta', 23, 'kabir.mehta@example.com', 'English');
GO

SELECT * FROM Student;
```

### C# - Student.cs

```csharp
namespace ConsoleApp3
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Course { get; set; } = string.Empty;
    }
}
```

### C# - StudentRepository.cs

```csharp
using ConsoleApp3;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

public class StudentRepository
{
    private readonly string connectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=EducationDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public List GetAllStudents()
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student";

        // TODO:
        // Use Dapper to fetch all students and convert the result into a List.
        // Example method to use: Query(query)
        // List students = ____________________________________________;

        return new List();
    }

    public Student? GetStudentById(int id)
    {
        using SqlConnection connection = new SqlConnection(connectionString);

        string query = "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student WHERE Id = @Id";

        // TODO:
        // Use Dapper to return one student or null if not found.
        // Example method to use: QueryFirstOrDefault(query, new { Id = id })
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
```

### C# - Program.cs

```csharp
using ConsoleApp3;
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        StudentRepository repository = new StudentRepository();

        Console.WriteLine("All Students");
        Console.WriteLine("------------");

        // TODO:
        // Call GetAllStudents() and store the result.
        // List students = ____________________________________________;
        List students = new List();

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
```

### What you should complete

- In `StudentRepository`:
  - `GetAllStudents()`
  - `GetStudentById(int id)`
  - `InsertStudent(Student student)`
  - `UpdateStudent(Student student)`
  - `DeleteStudent(int id)`
- In `Program.cs`:
  - Fetch all students
  - Fetch one student by id
  - Insert a student
  - Update a student
  - Delete a student

This structure follows the `EducationDB` + `Student` + Dapper CRUD setup shown in your screenshot .

When you fill in the `TODO` parts, send your version back and I’ll review it line by line.


why are you using @ in queries before the query begins? what does it signify?
In your C# Dapper CRUD exercise for `EducationDB`/`Student`, there are actually **two different uses of `@`**, and they do **different jobs** .

### 1) `@` before the string
Example:
```csharp
string query = @"INSERT INTO Student (FirstName, LastName)
                 VALUES (@FirstName, @LastName)";
```
This `@` is a **C# verbatim string literal**.

It means:
- You can write the SQL on **multiple lines** more easily
- Backslashes don’t need escaping as much
- The string is taken more literally

Without `@`, you would usually write the SQL in one line or use `\n` for line breaks.

Example without it:
```csharp
string query = "INSERT INTO Student (FirstName, LastName) VALUES (@FirstName, @LastName)";
```

So:
- `@"..."` = C# string syntax
- It is **not SQL syntax**

### 2) `@` inside the SQL query
Example:
```csharp
WHERE Id = @Id
```
Here, `@Id` is a **SQL parameter placeholder**.

It means:
- A value will be supplied from C# code
- Dapper matches it with something like:
```csharp
new { Id = id }
```
- This helps avoid string concatenation
- It is the safer and standard way to pass values into SQL queries

Example:
```csharp
string query = "SELECT * FROM Student WHERE Id = @Id";
Student? student = connection.QueryFirstOrDefault(query, new { Id = id });
```

Here:
- `@Id` in SQL matches
- `Id = id` in the anonymous object

### Short answer
- `@` **before the string** = C# says: “treat this as a verbatim string”
- `@` **inside the SQL** = SQL/Dapper parameter name

### Easy way to remember
Think of it like this:
- `@"..."` → for **writing the query text nicely**
- `@Id`, `@Name` → for **sending values into the query safely**

If you want, I can show the **same query written 3 ways**:
1. without `@` before the string
2. with `@` before the string
3. with a bad string-concatenation example vs the correct parameterized version 


got it. is there way to make the code more structuredlike is there something getting repeated again and again
Yes — for the `EducationDB` / `Student` CRUD exercise using Repository Pattern and Dapper, you can make the code cleaner and more structured .

## Question
Is there a way to make the code more structured? Is something getting repeated again and again?

## Answer
Yes.

The repetition in this kind of code usually comes from:
- creating a `SqlConnection` in every method
- writing SQL strings inside each method
- printing student data in multiple places
- keeping too much logic inside `Main`

## Important note
Some repetition is **okay**.

For learning, it is actually good to keep separate CRUD methods like:
- `GetAllStudents()`
- `GetStudentById()`
- `InsertStudent()`
- `UpdateStudent()`
- `DeleteStudent()`

So don’t try to make it *too* clever yet.

## Best simple improvements

### 1) Create one helper method for the connection
Instead of repeating:
```csharp
using SqlConnection connection = new SqlConnection(connectionString);
```
write:
```csharp
private SqlConnection CreateConnection()
{
    return new SqlConnection(connectionString);
}
```

Then inside methods:
```csharp
using SqlConnection connection = CreateConnection();
```

---

### 2) Move SQL queries into constants or readonly fields
Instead of writing SQL text from scratch inside every method, keep them at the top.

```csharp
private const string SelectAllQuery =
    "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student";

private const string SelectByIdQuery =
    "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student WHERE Id = @Id";

private const string InsertQuery =
    @"INSERT INTO Student (FirstName, LastName, Age, Email, Course)
      VALUES (@FirstName, @LastName, @Age, @Email, @Course)";

private const string UpdateQuery =
    @"UPDATE Student
      SET FirstName = @FirstName,
          LastName = @LastName,
          Age = @Age,
          Email = @Email,
          Course = @Course
      WHERE Id = @Id";

private const string DeleteQuery =
    "DELETE FROM Student WHERE Id = @Id";
```

This makes each method much shorter.

---

### 3) Keep `Main` small
If `Main` is doing everything, it becomes hard to read.

You can move display logic into helper methods.

```csharp
private static void PrintStudent(Student student)
{
    Console.WriteLine($"{student.Id} - {student.FirstName} {student.LastName} - Age: {student.Age} - {student.Email} - {student.Course}");
}

private static void PrintStudents(List students)
{
    foreach (Student student in students)
    {
        PrintStudent(student);
    }
}
```
