---
tags: [csharp, async, await, concurrency]
aliases: [Async, Sync]
---
# Async vs Sync

Asynchronous programming (`async`/`await`) allows your program to start an operation and then continue doing other work while waiting for that operation to finish.

## What `async` Really Means
A common misunderstanding is that "async means run on another thread." This is **not** what async means.

**Question (from chat):** If multiple tasks were spawned by `Task.WhenAll`, wouldn't synchronous `Thread.Sleep` and async be basically the same, since that new thread is doing no extra work anyway?
**Answer:** No.

`async` means:
- This method may pause at `await`.
- Control can return to the caller.
- The rest of the method continues later when the awaited operation finishes.

The continuation may run:
- On the same thread later
- On another thread
- On a UI context
- On a thread pool thread

> [!IMPORTANT]
> The point of async is not "new thread." The point is **non-blocking waiting**.

## CPU-Bound vs I/O-Bound Work

This is the rule that clears up most confusion.

### If the work is CPU-bound
**Examples:** Calculations, image processing, compression, encryption.
Async does **not** magically help here. The CPU must actually do work. You should use:
- Synchronous code
- `Task.Run` if you want to move it off the calling thread
- Parallelism (`Parallel.For`, etc.) if appropriate

### If the work is wait-bound / I/O-bound
**Examples:** HTTP requests, database calls, file reads/writes, timers.
Async helps immensely because the program spends much of its time **waiting**, not computing.
This is the perfect use case for `async`, `await`, and `Task.WhenAll`.

## Code Example: Converting Sync to Async

### Synchronous Version
```csharp
public static void Main()
{
    DownloadFile("File 1");
    DownloadFile("File 2");
    DownloadFile("File 3");
}

static void DownloadFile(string filename)
{
    Thread.Sleep(3000); // Blocks a real thread
}
```

### Asynchronous Version (Concurrent)
```csharp
public static async Task Main()
{
    // These start concurrently
    Task file1 = DownloadFileAsync("File 1");
    Task file2 = DownloadFileAsync("File 2");
    Task file3 = DownloadFileAsync("File 3");

    // Wait for all to finish
    await Task.WhenAll(file1, file2, file3);
}

static async Task DownloadFileAsync(string filename)
{
    await Task.Delay(3000); // Does NOT block a thread
}
```

---
## See Also
- [Blocking vs Non-Blocking](Blocking%20vs%20Non-Blocking.md)
- [Task vs Thread](Task%20vs%20Thread.md)
