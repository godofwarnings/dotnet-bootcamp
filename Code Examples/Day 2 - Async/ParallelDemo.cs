using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class ParallelDemo
{
    public static void Main()
    {
        // RunParallelFileProcessingDemo();
        RunParallelCounterDemo();
    }

    public static void RunParallelFileProcessingDemo()
    {
        List<string> files = new List<string>
        {
            "File1", "File2", "File3", "File4", "File5"
        };

        Console.WriteLine("Parallel file processing started...");

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

    public static void RunParallelCounterDemo()
    {
        int counter = 0;

        // Unsafe parallel processing:
        Parallel.For(0, 10000, i =>
        {
            counter++;
        });

        Console.WriteLine("Unsafe Parallel Processing:");
        Console.WriteLine("Expected count = {0}", 10000);
        Console.WriteLine("Actual count = {0}", counter);

        // Thread-safe version using lock:
        counter = 0;
        object lockObject = new object();

        Parallel.For(0, 10000, i =>
        {
            lock (lockObject)
            {
                counter++;
            }
        });

        Console.WriteLine("\nThread-safe Parallel Processing:");
        Console.WriteLine("Expected count = {0}", 10000);
        Console.WriteLine("Actual count = {0}", counter);

        Console.WriteLine("Parallel process finished.");
    }
}
