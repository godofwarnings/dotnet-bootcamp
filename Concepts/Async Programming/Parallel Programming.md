---
tags: [csharp, parallel, multithreading, concurrency, locks]
aliases: [Parallel, Parallel.For, Parallel.ForEach, lock]
---
# Parallel Programming

While `async`/`await` is meant for I/O-bound waits (non-blocking), **Parallel Programming** is used for **CPU-bound** work where you actually want to use multiple threads simultaneously to crunch data faster.

See [Program.cs](../../DONE/Program.cs) for concrete implementations.

## The `Parallel` Class

The `System.Threading.Tasks.Parallel` class provides data parallelism.

### Parallel.ForEach
Executes a `foreach` loop in parallel.

```csharp
List<string> files = GetFiles();

// Multi-threaded version:
// The runtime decides how many threads to spawn based on CPU cores.
Parallel.ForEach(files, file =>
{
    ProcessFile(file);
});
```

### Parallel.For
Executes a `for` loop in parallel.

```csharp
// Unsafe parallel processing if updating shared variables
Parallel.For(0, 10000, i =>
{
    // Do work with index 'i'
});
```

## Thread Safety and Critical Sections

When multiple threads run simultaneously, they cannot safely update the same standard variable.

> [!WARNING]
> **The `counter++` Problem**
> If two threads read `counter` at `5` at the same exact time, and both increment it, they both write back `6`. You lose an increment. This is a **race condition**.

### The `lock` Statement
To prevent race conditions, you define a **critical section** using a `lock`. Only one thread can enter the lock at a time.

```csharp
int counter = 0;
object lockObject = new object(); // Must be a reference type

Parallel.For(0, 10000, i =>
{
    // Only one thread gets past here at a time
    lock (lockObject) 
    {
        counter++;
    }
});
```
*Note: While `lock` works, for simple integer increments, `Interlocked.Increment(ref counter)` is much faster.*

---
## See Also
- [Async vs Sync](Async%20vs%20Sync.md)
- [Thread Safe Collections](Thread%20Safe%20Collections.md)
