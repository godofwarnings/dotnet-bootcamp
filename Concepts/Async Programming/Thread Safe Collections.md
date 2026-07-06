---
tags: [csharp, collections, thread-safe, concurrentqueue]
aliases: [Thread Safe Collections, ConcurrentQueue]
---
# Thread Safe Collections

Standard C# collections (like `List<T>`, `Queue<T>`, `Dictionary<TKey, TValue>`) are **not** thread-safe. If multiple threads try to read and write to them at the same time, the internal state can become corrupted.

.NET provides thread-safe equivalents in the `System.Collections.Concurrent` namespace.

## ConcurrentQueue<T>
A thread-safe FIFO (First-In-First-Out) collection. 

```csharp
ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
queue.Enqueue(1);
queue.Enqueue(2);

// Thread-safe dequeue
while (queue.TryDequeue(out int order))
{
    Console.WriteLine($"Processing Order {order}");
}
```

## ConcurrentBag<T>
A thread-safe, unordered collection of objects. It is highly optimized for scenarios where the same thread is both producing and consuming data.

```csharp
ConcurrentBag<int> numbers = new ConcurrentBag<int>();

Parallel.For(1, 10001, i =>
{
    numbers.Add(i); // Safe to call from multiple threads concurrently
});
```
*(See `ConcurrentBagDemo` in [ConcurrentDemo.cs](../../Code%20Examples/Day%202%20-%20Async/ConcurrentDemo.cs))*

### Understanding `TryDequeue`
With a normal `Queue`, calling `Dequeue()` on an empty queue throws an exception. `ConcurrentQueue` uses the safer "Try pattern":

- `TryDequeue` tries to remove the next item.
- If successful, it puts the removed value into the `out` variable (`order` in this case).
- It returns `true` if it worked, or `false` if the queue was empty.

### Nullable Types in Queues (`out int?` vs `out int`)
You might wonder if you should use `out int? order` instead of `out int order`.

- `int?` means nullable integer (can hold an `int` or `null`).
- In `ConcurrentQueue<int>`, the queue stores regular integers.
- The `bool` return value of `TryDequeue` already handles the "maybe there is no value" case. 
- Therefore, `out int order` is the correct and simpler choice.

> [!NOTE]
> A zero-length string (`""`) is **not** the same thing as `null`. An empty string is a valid item in a queue of strings.

### What Thread Safety Actually Means Here
It means multiple threads can call `Enqueue` and `TryDequeue` at the exact same time without you needing to add your own `lock` around the queue operations.

> [!WARNING]
> The queue being thread-safe does **not** make all your other shared variables thread-safe. If you are updating a shared counter like `processedCount++` inside your queue processing logic, you still need to use thread-safe operations like `Interlocked.Increment(ref processedCount)`.

---
## See Also
- [Async vs Sync](Async%20vs%20Sync.md)
