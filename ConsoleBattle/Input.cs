namespace ConsoleBattle;

public class Input
{
    private Random _random;

    public Input()
    {
        _random = new();
    }

    public int GetNumber(int maxCommandNumber)
    {
        int commandNumber;
        string? input = Console.ReadLine();
        while (!int.TryParse(input, out commandNumber) || commandNumber < 1 || commandNumber > maxCommandNumber)
        {
            Console.WriteLine("Команда не распознана, попробуйте еще раз...");
            input = Console.ReadLine();
        }

        return commandNumber;
    }

    public int GetEnemyCommandNumber(int maxCommandNumber)
    {
        return _random.Next(1, maxCommandNumber + 1);
    }

    public void GetAnyKeyToProceed()
    {
        Console.WriteLine("Для продолжения нажмите любую клавишу...");
        Console.ReadKey();
        Console.Clear();
    }

    public bool WannaReplay()
    {
        Console.WriteLine("Хотите сыграть еще раз? (y/n)");
        string? input = Console.ReadLine();
        return input != "n";
    }
}