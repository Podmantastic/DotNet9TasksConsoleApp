# Task Completion Strategies in C# (.NET)

This project demonstrates various ways to handle the completion of multiple asynchronous tasks in C#, showcasing older methods and highlighting the new `Task.WhenEach` feature introduced in .NET 9.

## The Challenge of Multiple Tasks

When working with asynchronous operations, it's common to launch multiple tasks concurrently.  The challenge lies in managing these tasks and processing their results as they complete. We need efficient ways to:

1.  **Wait for all tasks to finish:** Ensure all operations are done before proceeding.
2.  **Process results as they become available:**  Handle results as soon as a task completes, rather than waiting for all tasks to complete.
3. **Manage potentially long running operations:** We dont want the UI to freeze, so we don't want to block the main thread waiting for the tasks to complete.

## Approaches

This project demonstrates three distinct approaches to handling task completion:

### 1. `Task.WhenAll` (Sequential Waiting)

-   **How it works:**
    -   `Task.WhenAll(tasks)` takes a collection of `Task` objects.
    -   It returns a single `Task` that represents the completion of *all* tasks in the collection.
    -   You `await` this single task to ensure that *all* tasks are complete before continuing.
    -   The result of await `Task.WhenAll` is a collection of all the individual results.
-   **Pros:**
    -   Simple and straightforward for scenarios where you *must* wait for all tasks to complete before proceeding.
    -   Guarantees all tasks have finished and provides a convenient way to access all results at once.
-   **Cons:**
    -   Inefficient if you could start processing results as they become available. You're forced to wait for the slowest task to complete, even if others have finished much earlier.
    -   It doesn't allow for the processing of individual task results as they're finished, only in a batch at the end.
- **Code Example:**
    ```csharp
    async Task WaitAllTasksInSequenceAsync()
    {
        List<Task<int>> tasks = Enumerable.Range(1, 5).Select(CalculateAsync).ToList();

        var result = await Task.WhenAll(tasks);

        foreach (var item in result)
        {
            Console.WriteLine($"{nameof(WaitAllTasksInSequenceAsync)}: Task = {item}");
        }
    }
    ```

### 2. `Task.WhenAny` (Container/As-Completed Strategy)

-   **How it works:**
    -   Uses a `while` loop and a container (the `tasks` list).
    -   `Task.WhenAny(tasks)` returns a `Task` that represents the *first* task in the collection to complete.
    -   You `await` this "first to complete" task.
    -   After awaiting it, you remove the completed task from the `tasks` list.
    -   The loop continues until the `tasks` list is empty (all tasks completed).
-   **Pros:**
    -   Allows you to process results as they become available, leading to potentially faster overall processing.
    -   More responsive compared to `Task.WhenAll` in scenarios with tasks of varying durations.
-   **Cons:**
    -   More complex to write and understand than `Task.WhenAll`.
    -   Requires manual management of the task list (adding, removing completed tasks).
    -   Still requires a loop to iterate, which could be more concise.
- **Code Example:**
    ```csharp
    async Task WaitAllTasksAsCompletedAsync()
    {
        List<Task<int>> tasks = Enumerable.Range(1, 5).Select(CalculateAsync).ToList();

        while(tasks.Any())
        {
            var finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);

            Console.WriteLine($"{nameof(WaitAllTasksAsCompletedAsync)}: Task = {await finishedTask}");
        }
    }
    ```

### 3. `Task.WhenEach` (.NET 9) (Efficient As-Completed)

-   **How it works:**
    -   `Task.WhenEach(tasks)` is a new feature in .NET 9.
    -   It returns an `IAsyncEnumerable<Task<T>>`, allowing you to use `await foreach` to iterate through the tasks as they complete.
    - Each item you receive is a Task that has completed.
    -   This means you no longer have to manage the task list manually.
-   **Pros:**
    -   **Clean and Concise:** The `await foreach` syntax is significantly more readable and easier to write than the `Task.WhenAny` loop approach.
    -   **Efficient:** Processes results as they become available, just like the `Task.WhenAny` approach.
    -   **No Manual Management:** Eliminates the need to manage a task list manually.
    -   **Built-in:** A standard part of .NET 9, making it a more robust and reliable solution.
-   **Cons:**
    -   Requires .NET 9 or later.
- **Code Example:**
    ```csharp
    async Task DotNetNineWhenEachExampleAsync()
    {
        List<Task<int>> tasks = Enumerable.Range(1, 5).Select(CalculateAsync).ToList();

        await foreach (var task in Task.WhenEach(tasks))
        {
            Console.WriteLine($"{nameof(DotNetNineWhenEachExampleAsync)}: Task = {await task}");
        }
    }
    ```

## Conclusion

-   **`Task.WhenAll`:** Ideal when you absolutely need to wait for *all* tasks to complete before proceeding and are fine with waiting for the slowest task.
-   **`Task.WhenAny` (Container):** A step up when you need to process results as they become available, but it's more complex to manage.
-   **`Task.WhenEach` (.NET 9):** The clear winner for most scenarios involving processing task results as they complete. It provides the best combination of efficiency, readability, and maintainability.

This project provides a practical comparison of these approaches, demonstrating the advantages of `Task.WhenEach` in .NET 9.
