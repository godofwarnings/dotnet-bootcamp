using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Product_1
    {
        // constructor
        public Product_1(int id, string name, decimal price, int stock_)
        {
            // when to use this and when not to use it??

            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Stock = stock_;
        }

        // set attributes private
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        private int stock;
        public int Stock
        {
            get
            {
                return stock;
            }
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Stock value cannot be negative.");
                    return;
                }
                stock = value;
            }
        }

        public void DisplayDetails()
        {
            Console.WriteLine("Product Details:");
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Price: {Price}");
            Console.WriteLine($"Stock: {stock}");
            Console.WriteLine("----------------------");
        }

        // business logic
        public bool isAvailable()
        {
            return (Stock > 0);
        }
    }

    public abstract class Product2
    {
        public Product2(int id, string name, decimal price, int stock)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Stock = stock;
        }

        public abstract decimal CalculateDiscount();

        public decimal GetFinalPrice()
        {
            return Price - CalculateDiscount();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        private int stock;
        public int Stock
        {
            get { return stock; }
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Stock value cannot be negative.");
                    return;
                }
                stock = value;
            }
        }

        public virtual void DisplayDetails()
        {
            Console.WriteLine("Product Details:");
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Price: {Price}");
            Console.WriteLine($"Stock: {stock}");
            Console.WriteLine($"Discounted: {CalculateDiscount()}");
            Console.WriteLine($"Final Price: {GetFinalPrice()}");
        }

        public bool isAvailable()
        {
            return (Stock > 0);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Product(int id, string name, string category, decimal price, int stock)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
        }

        public void Display()
        {
            Console.WriteLine($"{Id} - {Name} - {Category} - {Price} - Stock: {Stock}");
        }
    }

    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string message) : base(message) 
        {
        }
    }
}
