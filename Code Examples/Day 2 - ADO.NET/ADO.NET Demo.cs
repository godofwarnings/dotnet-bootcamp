// Code Examples/ADO.NET Demo.cs
using System;
using Microsoft.Data.SqlClient;

public class ProductDatabaseDemo
{
    public static void Main()
    {
        // Connection string used to connect to the SQL Server LocalDB database.
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=RetailDb;Trusted_Connection=True;TrustServerCertificate=True;";

        // Create one connection object and reuse it in both examples.
        // using keyword is used to dispose of resources once it has been used.
        using SqlConnection connection = new SqlConnection(connectionString);

        DisplayAllProducts(connection);
        DisplayProductCount(connection);
        InsertQuery(connection);
        UpdateQuery(connection);
        DisplayAllProducts(connection);
        DisplayProductCount(connection);
        DeleteQuery(connection);    
        DisplayAllProducts(connection);
        DisplayProductCount(connection);
    }

    private static void DisplayAllProducts(SqlConnection connection)
    {
        // Open the connection before running the query.
        connection.Open();
        Console.WriteLine("Connection opened successfully");

        // This query returns all product records from the Products table.
        string selectProductsQuery = "SELECT Id, Name, Category, Price, Stock FROM Products";

        // Create a command object using the query and connection.
        using SqlCommand selectProductsCommand = new SqlCommand(selectProductsQuery, connection);

        // ExecuteReader is used when the query returns multiple rows.
        using SqlDataReader reader = selectProductsCommand.ExecuteReader();

        // Read each row one by one from the result set.
        while (reader.Read())
        {
            // Extract values from the current row and convert them
            // to the correct C# data types.
            int id = Convert.ToInt32(reader["Id"]);
            string name = reader["Name"].ToString()!;
            string category = reader["Category"].ToString()!;
            decimal price = Convert.ToDecimal(reader["Price"]);
            int stock = Convert.ToInt32(reader["Stock"]);

            // Display product details in a simple readable format.
            Console.WriteLine($"{id} - {name} - {category} - {price} - Stock: {stock}");
        }

        // Close the connection after finishing the read operation.
        connection.Close();
        Console.WriteLine("Connection closed after reading products");
    }

    private static void DisplayProductCount(SqlConnection connection)
    {
        // Reopen the same connection for the next query.
        connection.Open();
        Console.WriteLine("Connection opened successfully");

        // This query returns only one value: the total number of products.
        string countProductsQuery = "SELECT COUNT(*) FROM Products";

        // Create a command object for the count query.
        using SqlCommand countProductsCommand = new SqlCommand(countProductsQuery, connection);

        // ExecuteScalar is used when the query returns a single value.
        int productCount = Convert.ToInt32(countProductsCommand.ExecuteScalar());

        Console.WriteLine($"Product count: {productCount}");

        // Close the connection after the query is complete.
        connection.Close();
        Console.WriteLine("Connection closed successfully");
    }

    private static void InsertQuery(SqlConnection connection)
    {
        connection.Open();
        Console.WriteLine("Connection opened successfully");

        string insertProductQuery = "INSERT INTO Products (Name, Category, Price, Stock) VALUES ('Printer', 'Electronics', 1800, 5)";

        using SqlCommand insertProductCommand = new SqlCommand(insertProductQuery, connection);

        // ExecuteNonQuery for any DML statements (Insert, Update, Delete)
        int rowsAffected = insertProductCommand.ExecuteNonQuery();

        Console.WriteLine($"{rowsAffected} row(s) inserted successfully.");

        connection.Close();
        Console.WriteLine("Connection closed successfully");
    }

    private static void UpdateQuery(SqlConnection connection)
    {
        connection.Open();
        Console.WriteLine("Connection opened successfully");

        string updateProductQuery = "UPDATE Products SET Stock = 20 WHERE Id = 6";

        using SqlCommand updateProductCommand = new SqlCommand(updateProductQuery, connection);

        int rowsAffected = updateProductCommand.ExecuteNonQuery();

        Console.WriteLine($"{rowsAffected} row(s) updated successfully.");

        connection.Close();
    }

    private static void DeleteQuery(SqlConnection connection)
    {
        connection.Open();
        Console.WriteLine("Connection opened successfully");

        string deleteProductQuery = "DELETE FROM Products WHERE Id = 6";

        using SqlCommand deleteProductCommand = new SqlCommand(deleteProductQuery, connection);

        int rowsAffected = deleteProductCommand.ExecuteNonQuery();

        Console.WriteLine($"{rowsAffected} row(s) deleted successfully.");

        connection.Close();
    }
}
