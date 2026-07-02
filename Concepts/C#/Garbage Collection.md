# Garbage Collection

In C#, the Common Language Runtime (CLR) uses a **generational garbage collection (GC)** model divided into three physical generations (0, 1, and 2) to optimize memory management. 

This is based on the "infant mortality" heuristic: most newly created objects have a short lifespan, while objects that survive initial collection cycles tend to remain in memory for a long time. By segregating memory based on object age, the system can clean short-lived objects very frequently without wasting time scanning long-lived data.

## The Three Core Generations

| Generation | Object Lifespan | Common Examples | Collection Frequency & Performance |
| :--- | :--- | :--- | :--- |
| **Generation 0 (Gen 0)** | Very short-lived; brand new objects. | Local variables, loops, parameters, temporary objects. | Most frequent. Takes milliseconds as it fits in CPU cache. |
| **Generation 1 (Gen 1)** | Short-to-medium lifespan. | Buffers or state objects spanning across async calls. | Medium frequency. Acts as a transitional buffer. |
| **Generation 2 (Gen 2)** | Very long-lived. | Static variables, application config caches, singletons. | Least frequent. Triggering this causes a "Full GC", cleaning all generations. High performance cost. |

## How Promotion Works
When a GC cycle runs, objects that are still being referenced (not garbage) survive the collection. Objects that survive Gen 0 are promoted to Gen 1. Objects that survive Gen 1 are promoted to Gen 2.
