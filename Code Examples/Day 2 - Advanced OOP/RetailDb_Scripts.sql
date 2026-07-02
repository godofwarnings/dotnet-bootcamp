-- RetailDb Setup
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
GO


-- EducationDB Setup (Student Practice)
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
GO
