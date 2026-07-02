using ConsoleApp1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    public static void Main()
    {
        // Run one demo at a time depending on what you want to execute.

        // RunDeliveryProductAndExceptionDemo();
        // RunSynchronousDownloadDemo();
        // RunParallelFileProcessingDemo();
        //RunParallelCounterDemo();
        //ConcurrentBagDemo();
        ConcurrentQueueDemo();

        // If you want to run the async demo directly from Main,
        // change Main to:
        // public static async Task Main()
        // Then call:
        // await RunAsynchronousDownloadDemo();
    }

    // ==================================================
    // DEMO 1: Delivery, LINQ, user input, and exceptions
    // ==================================================
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
        // Create different delivery types for the same order amount
        // to compare how each one behaves.
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
        // Sample product data used for LINQ filtering and aggregation.
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

        // Filter products whose price is greater than 5000.
        var expensiveProducts = products.Where(product => product.Price > 5000);

        Console.WriteLine("\nExpensive products:");
        foreach (var product in expensiveProducts)
        {
            product.Display();
        }

        // Calculate total inventory value:
        // price of each product multiplied by available stock.
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
            // Example: available stock is 10, but requested quantity is 11.
            PlaceOrder(10, 11);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void PlaceOrder(int availableStock, int quantity)
    {
        // Quantity must be greater than zero.
        if (quantity <= 0)
        {
            throw new Exception("Quantity must be greater than 0");
        }

        // If requested quantity is more than available stock,
        // throw an exception.
        if (quantity > availableStock)
        {
            throw new InsufficientExecutionStackException("Insufficient Stock.");
        }

        Console.WriteLine("Order Placed Successfully");
    }

    // =====================================
    // DEMO 2: Synchronous file downloading
    // =====================================
    public static void RunSynchronousDownloadDemo()
    {
        Console.WriteLine("Starting...");

        // These run one after another.
        DownloadFile("File 1");
        DownloadFile("File 2");
        DownloadFile("File 3");
    }

    private static void DownloadFile(string fileName)
    {
        Console.WriteLine($"Downloading {fileName}...");

        // Thread.Sleep blocks the current thread for 3 seconds.
        Thread.Sleep(3000);

        Console.WriteLine($"Downloaded {fileName}");
    }

    // =====================================
    // DEMO 3: Asynchronous file downloading
    // =====================================
    public static async Task RunAsynchronousDownloadDemo()
    {
        Console.WriteLine("Starting...");

        // Start all download tasks first.
        Task file1 = DownloadFileAsync("File 1");
        Task file2 = DownloadFileAsync("File 2");
        Task file3 = DownloadFileAsync("File 3");

        // Wait until all downloads finish.
        await Task.WhenAll(file1, file2, file3);

        Console.WriteLine("Downloads Finished.");
    }

    private static async Task DownloadFileAsync(string fileName)
    {
        Console.WriteLine($"Downloading {fileName}...");

        // Task.Delay waits asynchronously without blocking the thread.
        await Task.Delay(3000);

        // This was left commented intentionally because Thread.Sleep
        // would block the thread and defeat the purpose of the async example.
        // Thread.Sleep(3000);

        Console.WriteLine($"Downloaded {fileName}");
    }

    // =========================================
    // DEMO 4: Parallel processing of file names
    // =========================================
    public static void RunParallelFileProcessingDemo()
    {
        List<string> files = new List<string>
        {
            "File1",
            "File2",
            "File3",
            "File4",
            "File5"
        };

        Console.WriteLine("Parallel file processing started...");

        // Single-threaded version:
        // foreach (string file in files)
        // {
        //     ProcessFile(file);
        // }

        // Multi-threaded version:
        Parallel.ForEach(files, file =>
        {
            ProcessFile(file);
        });

        Console.WriteLine("Parallel file processing completed.");
    }

    private static void ProcessFile(string fileName)
    {
        Console.WriteLine($"{fileName} is being processed on thread No. {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(3000);
        Console.WriteLine($"{fileName} is processed.");
    }

    // ==========================================
    // DEMO 5: Parallel counter and thread safety
    // ==========================================
    public static void RunParallelCounterDemo()
    {
        int counter = 0;

        // Unsafe parallel processing:
        // many threads try to update the same variable at the same time.
        Parallel.For(0, 10000, i =>
        {
            counter++;
        });

        Console.WriteLine("Expected count = {0}", 10000);
        Console.WriteLine("Actual count = {0}", counter);

        // Thread-safe version using lock:
        // only one thread can enter the critical section at a time.
        counter = 0;
        object lockObject = new object();

        Parallel.For(0, 10000, i =>
        {
            lock (lockObject)
            {
                counter++;
            }
        });

        Console.WriteLine("Expected count = {0}", 10000);
        Console.WriteLine("Actual count = {0}", counter);

        Console.WriteLine("Parallel process finished.");
    }

    // ===============================================
    // DEMO 6: Thread Safe collection - Concurrent Bag
    // ===============================================
    public static void ConcurrentBagDemo()
    {
        ConcurrentBag<int> numbers = new ConcurrentBag<int>();

        Parallel.For(1, 10001, i =>
        {
            numbers.Add(i);
        });

        Console.WriteLine("Expected count = {0}", 10000);
        Console.WriteLine("Actual count = {0}", numbers.Count);

    }

    // ===============================================
    // DEMO 7: Thread Safe collection - Concurrent Queue
    // ===============================================
    public static void ConcurrentQueueDemo()
    {
        ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);

        while (queue.TryDequeue(out int order))
        {
            Console.WriteLine("Processing Order {0}", order);
        }


    }
}
