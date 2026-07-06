using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;

public class Day1_Program
{
    public static void Main()
    {
        RunDeliveryProductAndExceptionDemo();
    }

    public static void RunDeliveryProductAndExceptionDemo()
    {
        RunDeliveryExamples();
        RunProductExamples();
        RunUserInputExample();
        RunOrderPlacementExample();
    }

    private static void RunDeliveryExamples()
    {
        decimal orderAmount = 1000;
        PrintDeliveryBills(orderAmount);

        orderAmount = 2500;
        PrintDeliveryBills(orderAmount);
    }

    public static void PrintDeliveryBills(decimal orderAmount)
    {
        Delivery[] deliveryOptions = new Delivery[]
        {
            new StandardDelivery(orderAmount),
            new ExpressDelivery(orderAmount),
            new InternationalDelivery(orderAmount),
            new FreeDelivery(orderAmount)
        };

        foreach (Delivery deliveryOption in deliveryOptions)
        {
            Console.WriteLine($"\n{deliveryOption.GetType().Name}");
            deliveryOption.DisplayBill();
        }
    }

    private static void RunProductExamples()
    {
        List<Product> products = new List<Product>
        {
            new Product(1, "Laptop", "Electronics", 55000, 10),
            new Product(2, "Mouse", "Electronics", 700, 50),
            new Product(3, "Keyboard", "Electronics", 1500, 25),
            new Product(4, "T-Shirt", "Clothing", 799, 100),
            new Product(5, "Jeans", "Clothing", 1999, 40),
            new Product(6, "Rice Bag", "Grocery", 1200, 30),
            new Product(7, "Cooking Oil", "Grocery", 1800, 15),
            new Product(8, "Chair", "Furniture", 3500, 8),
            new Product(9, "Table", "Furniture", 7500, 5),
            new Product(10, "Headphones", "Electronics", 2500, 0)
        };

        var expensiveProducts = products.Where(product => product.Price > 5000);

        Console.WriteLine("\nExpensive products:");
        foreach (var product in expensiveProducts)
        {
            product.Display();
        }

        decimal totalStockValue = products.Sum(product => product.Price * product.Stock);

        Console.WriteLine($"\nTotal Stock value is {totalStockValue}");
    }

    private static void RunUserInputExample()
    {
        try
        {
            Console.WriteLine("\nEnter a number:");
            int quantity = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Number entered: {quantity}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid format. Please enter a non negative integer");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }
    }

    private static void RunOrderPlacementExample()
    {
        try
        {
            PlaceOrder(10, 11);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void PlaceOrder(int availableStock, int quantity)
    {
        if (quantity <= 0)
        {
            throw new Exception("Quantity must be greater than 0");
        }

        if (quantity > availableStock)
        {
            throw new InsufficientStockException("Insufficient Stock.");
        }

        Console.WriteLine("Order Placed Successfully");
    }
}
