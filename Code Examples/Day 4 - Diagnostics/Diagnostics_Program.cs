using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    private static List<byte[]> leakedMemory = new List<byte[]>();

    public static async Task Main()
    {
        Console.WriteLine("DotNet Diagnostics Demo");
        Console.WriteLine("-----------------------");
        Console.WriteLine("1. CPU Hotspot");
        Console.WriteLine("2. Memory Leak");
        Console.WriteLine("3. Async Bottleneck");
        Console.WriteLine();
        Console.WriteLine("Enter option:");

        string option = Console.ReadLine()!;

        if (option == "1")
        {
            RunCpuHotspot();
        }
        else if (option == "2")
        {
            RunMemoryLeak();
        }
        else if (option == "3")
        {
            await RunAsyncBottleneck();
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }

    private static void RunCpuHotspot()
    {
        Console.WriteLine("CPU Hotspot started.");
        Console.WriteLine("Process Id: " + Environment.ProcessId);
        Console.WriteLine("Run dotnet-trace or PerfView now.");
        // dotnet-trace collect -p 18732 --duration 00:00:20 -o cpu-hotspot.nettrace
        // 18732 is the process id
        Console.WriteLine("Press Ctrl + C to stop.");

        while (true)
        {
            CalculatePrimeNumbers(50000);
        }
    }

    private static int CalculatePrimeNumbers(int max)
    {
        int count = 0;

        for (int number = 2; number <= max; number++)
        {
            bool isPrime = true;

            for (int divisor = 2; divisor < number; divisor++)
            {
                if (number % divisor == 0)
                {
                    isPrime = false;
                    break;
                }
            }

            if (isPrime)
            {
                count++;
            }
        }

        return count;
    }

    private static void RunMemoryLeak()
    {
        Console.WriteLine("Memory Leak started.");
        Console.WriteLine("Process Id: " + Environment.ProcessId);
        Console.WriteLine("Memory will keep increasing.");
        Console.WriteLine("Press Ctrl + C to stop.");

        int counter = 1;

        while (true)
        {
            byte[] data = new byte[5 * 1024 * 1024];

            leakedMemory.Add(data);

            Console.WriteLine($"Leak cycle: {counter}, Total leaked chunks: {leakedMemory.Count}");

            counter++;

            Thread.Sleep(1000);
        }
    }

    private static async Task RunAsyncBottleneck()
    {
        Console.WriteLine("Async Bottleneck started.");
        Console.WriteLine("Process Id: " + Environment.ProcessId);
        Console.WriteLine("This code blocks ThreadPool threads using Task.Run + Thread.Sleep.");
        Console.WriteLine("Press Ctrl + C to stop.");

        while (true)
        {
            List<Task> tasks = new List<Task>();

            for (int i = 1; i <= 200; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    Thread.Sleep(3000);
                }));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Batch completed at " + DateTime.Now.ToString("HH:mm:ss"));
        }
    }
}
