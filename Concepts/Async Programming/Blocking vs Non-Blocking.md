---
tags: [csharp, async, blocking, thread-sleep, task-delay]
aliases: [Blocking, Non-Blocking, Task.Delay, Thread.Sleep]
---
# Blocking vs Non-Blocking (Task.Delay vs Thread.Sleep)

When writing async code, understanding the difference between blocking a thread and non-blocking waiting is critical for scalability.

## The Important Difference

### With `Thread.Sleep` (Blocking)
A thread is occupied for the whole wait time.

```csharp
Task.Run(() => Thread.Sleep(3000))
```
- A thread pool thread starts.
- It goes to sleep for 3 seconds.
- That thread is unusable for anything else during those 3 seconds. It is a **blocked resource**.

### With `Task.Delay` (Non-Blocking)
No thread is tied up during the wait.

```csharp
await Task.Delay(3000)
```
- The method starts.
- It sets up a timer.
- Control returns (no thread sits there waiting).
- When the delay finishes, the continuation resumes later.

## Why "Doing Nothing" Still Matters

You might think, "If the thread is just sleeping, it's doing no extra work, so what's the problem?"

A blocked thread still costs resources:
- Memory for the thread stack (usually ~1MB per thread).
- Scheduler overhead.
- Thread pool capacity.
- Reduced ability to process other work.

Think of it like this:
- `Thread.Sleep` is like making an employee sit in a chair for 3 hours doing nothing, but they are still unavailable to help anyone else.
- `Task.Delay` is like telling the employee, “you can leave and come back in 3 hours.”

## When to use which?
- Use `Thread.Sleep` **only** when you truly want to **block the current thread** on purpose (e.g., in a purely synchronous console app where scalability doesn't matter).
- Use `await Task.Delay` when you want to **wait without blocking a thread**.

---
## See Also
- [Async vs Sync](Async%20vs%20Sync.md)
- [Task vs Thread](Task%20vs%20Thread.md)
