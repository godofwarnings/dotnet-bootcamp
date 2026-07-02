---
tags: [csharp, async, tasks, threads]
aliases: [Task, Thread]
---
# Task vs Thread

This is one of the most important concepts in C# async programming. **A Task is not a thread.**

A `Task` is better thought of as:
- A promise of future completion.
- A representation of an operation.

Sometimes a task uses a thread. Sometimes it does not.

## Example A: Task that uses a thread
```csharp
Task t = Task.Run(() =>
{
    Thread.Sleep(3000);
});
```
Here, `Task.Run` schedules work on a thread pool thread, and that thread is actually used (and blocked by `Thread.Sleep`).

## Example B: Task that does not block a thread
```csharp
Task t = Task.Delay(3000);
```
No thread is sitting around for 3 seconds. The runtime uses a timer. This task represents waiting, but not by occupying a thread.

## Example C: Real async I/O
```csharp
var response = await httpClient.GetAsync(url);
```
During most of the network wait, your code is not using a thread. The OS/network stack handles the waiting. When data is ready, the continuation resumes later.

## How `Task.WhenAll` Actually Works

> [!WARNING]
> **Common Misunderstanding:**
> `Task.WhenAll` does **not** mean "create threads" or "make code parallel automatically."

`Task.WhenAll` simply:
1. Takes multiple tasks.
2. Returns one combined task.
3. Completes when all of them complete.

It is a **coordinator**, not a thread creator. The concurrency happens because you start the tasks *before* awaiting them.

---
## See Also
- [Async vs Sync](Async%20vs%20Sync.md)
- [Blocking vs Non-Blocking](Blocking%20vs%20Non-Blocking.md)
