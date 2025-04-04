using System;

namespace DotNet9TasksConsoleApp;

public class ManipulationDelegates
{
    private MultiplyByConstanDelegate MultiplyByTwo { get; } = static number =>
    {
        return number * 2;
    };

    private MultiplyByConstanDelegate MultiplyByThree { get; } = static number =>
    {
        return number * 3;
    };

    delegate int MultiplyByConstanDelegate(int number);

    public ManipulationDelegates()
    {
        var result = MultiplyByTwo(5);
        Console.WriteLine(result);

        result = MultiplyByThree(5);
        Console.WriteLine(result);
    }

    void Manipulate(int number, MultiplyByConstanDelegate multiplyByConstanDelegate)
    {
        var result = multiplyByConstanDelegate(number);
        Console.WriteLine(result);
    }
}
