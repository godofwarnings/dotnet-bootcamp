// Code Examples/Basic Product Class.cs
using System;

namespace ConsoleApp1
{
    public class Product
    {
        // Private fields store the product data.
        // They cannot be changed directly from outside the class.
        private int id;
        private string name;
        private decimal price;
        private int stock;

        // Constructor runs when a new Product object is created.
        // It assigns the initial values for the product.
        public Product(int id, string name, decimal price, int stock)
        {
            // 'this' refers to the current object.
            // Use 'this.id' to refer to the field in the class,
            // and 'id' to refer to the parameter passed into the constructor.
            // Since both names are the same, 'this' helps avoid confusion.
            this.id = id;
            this.name = name;
            this.price = price;
            this.stock = stock;
        }

        // Displays all product details in a readable format.
        public void DisplayDetails()
        {
            Console.WriteLine("Product Details:");
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Price: {price}");
            Console.WriteLine($"Stock: {stock}");
            Console.WriteLine("----------------------");
        }

        // Returns true if the product has stock available.
        public bool IsAvailable()
        {
            return stock > 0;
        }
    }
}
