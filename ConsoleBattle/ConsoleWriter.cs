namespace ConsoleBattle;

public class ConsoleWriter
{
    
    public void WriteStatus(string unitName, Unit unit)
    {
        Console.Write($"{unitName}: ");
        ConsoleColor color;
        if (unit.CurrentHealth >= unit.MaxHealth * 0.6) 
            color = ConsoleColor.Green;
        else if (unit.CurrentHealth >= unit.MaxHealth * 0.3)
            color = ConsoleColor.Yellow;
        else
            color = ConsoleColor.Red;
        WriteWithColor($"{unit.CurrentHealth} HP ", color);
        if (unit.ShieldCount > 0)
            WriteWithColor($"[Щит: {unit.ShieldCount}] ", ConsoleColor.Yellow);
        if (unit.LastDamageFromWeapon)
            WriteWithColor($"(последний урон был нанесен оружием)", ConsoleColor.DarkGray);
        Console.WriteLine();
    }
    
    public void WriteDealDamage(string attacker, string target, float damage)
    {
        Console.Write($"{attacker} нанес ");
        WriteWithColor($"{damage} ", ConsoleColor.Red);
        Console.WriteLine($"урона {target}");
    }
    
    public void WriteSelfDamage(string target, float damage)
    {
        Console.Write($"{target} нанес сам себе ");
        WriteWithColor($"{damage} ", ConsoleColor.Red);
        Console.WriteLine("урона");
    }
    
    public void WriteHeal(string target, float amount)
    {
        Console.Write($"{target} восстановил себе ");
        WriteWithColor($"{amount} ", ConsoleColor.Green);
        Console.WriteLine("здоровья");
    }
    
    public void WriteShielded(string target)
    {
        WriteWithColor($"{target} укрылся щитом\n", ConsoleColor.Yellow);
    }

    public void WriteAttacksWithType(AttackType attackType, string attacker, string target, List<float> damages)
    {
        foreach (var damage in damages)
        {
            if (damage == 0) continue;
            switch (attackType)
            {
                case AttackType.Direct:
                    WriteDealDamage(attacker, target, damage);
                    break;
                case AttackType.Self:
                    WriteSelfDamage(target, damage);
                    break;
                case AttackType.Heal:
                    WriteHeal(target, -damage);
                    break;
            }
        }
        
        damages.Clear();
    }
    
    public void WriteDamagesFromTo(Dictionary<AttackType, List<float>> attacks, string attacker, string target)
    {
        foreach (var attack in attacks)
        {
            WriteAttacksWithType(attack.Key, attacker, target, attack.Value);
        }
    }

    public void WriteResult(bool isPlayerWin)
    {
        if (isPlayerWin)
            WriteWithColor("Вы выиграли!\n", ConsoleColor.Green);
        else
            WriteWithColor("Вы проиграли!\n", ConsoleColor.Red);
    }
    
    public void WriteWithColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
    
    public void WriteWelcome(string playerName)
    {
        Console.WriteLine($"Добро пожаловать в Console Battle, {playerName}!");
    }
    
    public void WriteAllAbilities(string message, Unit unit)
    {
        Console.WriteLine(message);
        for (int i = 0; i < unit.GetAbilityCount(); i++)
        {
            Console.WriteLine($"{i + 1}. {unit.GetAbilityDescription(i)}");
        }
    }
      
}