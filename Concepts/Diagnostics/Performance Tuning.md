# Performance Tuning and Diagnostics

In .NET, several tools and techniques are used to diagnose performance issues like CPU spikes, memory leaks, and thread starvation.

## 1. CPU Hotspots
A CPU hotspot occurs when a specific method or loop consumes an excessive amount of CPU cycles, starving the rest of the application.
- **Diagnosis**: Use `dotnet-trace` to capture a trace of the running process.
- **Command**: `dotnet-trace collect -p <process_id> --duration 00:00:20 -o cpu-hotspot.nettrace`
- **Analysis**: Open the `.nettrace` file in Visual Studio or PerfView to inspect the call tree and see which methods are consuming the most CPU time.

## 2. Memory Leaks
A memory leak occurs when objects are no longer needed but are still referenced by something (like a static list), preventing the [Garbage Collector](../C%23/Garbage%20Collection.md) from cleaning them up.
- **Example**: Appending large byte arrays to a static `List<byte[]>` that is never cleared.

## 3. Async Bottlenecks (Thread Pool Starvation)
This happens when you block asynchronous threads synchronously.
- **Anti-pattern**: Using `Thread.Sleep()` inside a `Task.Run()`. `Thread.Sleep()` blocks the underlying thread pool thread, meaning the runtime must spawn new threads to handle incoming work.
- **Solution**: Always use `await Task.Delay()` for asynchronous waiting, which frees the thread back to the thread pool while waiting.

See [Diagnostics_Program.cs](../../Code%20Examples/Day%204%20-%20Diagnostics/Diagnostics_Program.cs) for code examples generating these issues.
