using System;
using Microsoft.Data.SqlClient;

public class ProductDatabaseDemo
{
    public static void Main()
    {
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=RetailDb;Trusted_Connection=True;TrustServerCertificate=True;";

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
        connection.Open();
        Console.WriteLine("Connection opened successfully");

        string selectProductsQuery = "SELECT Id, Name, Category, Price, Stock FROM Products";

        using SqlCommand selectProductsCommand = new SqlCommand(selectProductsQuery, connection);

        using SqlDataReader reader = selectProductsCommand.ExecuteReader();

        while (reader.Read())
        {
            int id = Convert.ToInt32(reader["Id"]);
            string name = reader["Name"].ToString()!;
            string category = reader["Category"].ToString()!;
            decimal price = Convert.ToDecimal(reader["Price"]);
            int stock = Convert.ToInt32(reader["Stock"]);

            Console.WriteLine($"{id} - {name} - {category} - {price} - Stock: {stock}");
        }

        connection.Close();
        Console.WriteLine("Connection closed after reading products");
    }

    private static void DisplayProductCount(SqlConnection connection)
    {
        connection.Open();
        Console.WriteLine("Connection opened successfully");

        string countProductsQuery = "SELECT COUNT(*) FROM Products";

        using SqlCommand countProductsCommand = new SqlCommand(countProductsQuery, connection);

        int productCount = Convert.ToInt32(countProductsCommand.ExecuteScalar());

        Console.WriteLine($"Product count: {productCount}");

        connection.Close();
        Console.WriteLine("Connection closed successfully");
    }

    private static void InsertQuery(SqlConnection connection)
    {
        connection.Open();
        Console.WriteLine("Connection opened successfully");

        string insertProductQuery = "INSERT INTO Products (Name, Category, Price, Stock) VALUES ('Printer', 'Electronics', 1800, 5)";

        using SqlCommand insertProductCommand = new SqlCommand(insertProductQuery, connection);

        int rowsAffected = insertProductCommand.ExecuteNonQuery();

        Console.WriteLine($"{rowsAffected} row(s) inserted successfully.");

        connection.Close();
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
