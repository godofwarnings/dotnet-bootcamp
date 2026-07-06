using System;
using System.Threading;
using System.Threading.Tasks;

public class AsyncDemo
{
    public static async Task Main()
    {
        // RunSynchronousDownloadDemo();
        await RunAsynchronousDownloadDemo();
    }

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

        Console.WriteLine($"Downloaded {fileName}");
    }
}
