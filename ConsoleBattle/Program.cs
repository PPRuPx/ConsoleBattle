namespace ConsoleBattle;

internal class Program
{
        
    static void Main(string[] args)
    {
        Input input = new Input();
        ConsoleWriter writer = new ConsoleWriter();
        
        Console.WriteLine("Введите ваше имя:");
        var playerName = Console.ReadLine();
        
        do
        {
            Unit playerUnit = new Unit(1000, 50);
            Unit enemyUnit = new Unit(2000, 55);
            
            Player player = new Player(input, writer, playerUnit, enemyUnit, playerName);
            Enemy enemy = new Enemy(input, playerUnit, enemyUnit);
            
            Console.Clear();
            writer.WriteWelcome(playerName);

            while (playerUnit.IsAlive && enemyUnit.IsAlive)
            {
                player.Turn();
                enemy.Turn();

                writer.WriteDamagesFromTo(enemyUnit.DamageHistory, playerName, "Enemy");
                writer.WriteDamagesFromTo(playerUnit.DamageHistory, "Enemy", playerName);
                    
                input.GetAnyKeyToProceed();
            }

            writer.WriteResult(playerUnit.IsAlive);
        } while (input.WannaReplay());
    }
}