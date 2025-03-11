Console.WriteLine("Hello, World!");

await WaitAllTasksInSequenceAsync();
await WaitAllTasksAsCompletedAsync();
await DotNetNineWhenEachExampleAsync();

async Task DotNetNineWhenEachExampleAsync()
{
    List<Task<int>> tasks = Enumerable.Range(1, 5).Select(CalculateAsync).ToList();

    await foreach (var task in Task.WhenEach(tasks))
    {
        Console.WriteLine($"{nameof(DotNetNineWhenEachExampleAsync)}: Task = {await task}");
    }
}

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

async Task WaitAllTasksInSequenceAsync()
{
    List<Task<int>> tasks = Enumerable.Range(1, 5).Select(CalculateAsync).ToList();

    // Waits for all tasks in the 'tasks' list to complete and retrieves their results.
    var result = await Task.WhenAll(tasks);

    // Iterates through the results of each completed task.
    foreach (var item in result)
    {
        // Prints the result (which is an integer in this case, representing the 'order' of the task) to the console.
        Console.WriteLine($"{nameof(WaitAllTasksInSequenceAsync)}: Task = {item}");
    }
}

async Task<int> CalculateAsync(int order)
{
    var randomDelay = new Random().Next(500, 4000);

    await Task.Delay(randomDelay);

    //Console.WriteLine($"Task {order} completed after {randomDelay}ms");

    return order;
}
