using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class ConcurrentDemo
{
    public static void Main()
    {
        ConcurrentBagDemo();
        ConcurrentQueueDemo();
    }

    public static void ConcurrentBagDemo()
    {
        ConcurrentBag<int> numbers = new ConcurrentBag<int>();

        Parallel.For(1, 10001, i =>
        {
            numbers.Add(i);
        });

        Console.WriteLine("ConcurrentBag Demo:");
        Console.WriteLine("Expected count = {0}", 10000);
        Console.WriteLine("Actual count = {0}", numbers.Count);
    }

    public static void ConcurrentQueueDemo()
    {
        Console.WriteLine("\nConcurrentQueue Demo:");
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
