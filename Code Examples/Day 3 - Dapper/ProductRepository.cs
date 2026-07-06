using ConsoleApp3;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

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
}
