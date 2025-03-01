namespace ConsoleBattle;

public class Player
{
    public Input Input;
    public ConsoleWriter Writer;
    public Unit PlayerUnit;
    public Unit EnemyUnit;
    public string Name;
    
    public int playerFireballDamage = 200;
    public int playerHealAmount = 100;
    
    public Player(Input input, ConsoleWriter writer, Unit playerUnit, Unit enemyUnit, string name)
    {
        Input = input;
        Writer = writer;
        PlayerUnit = playerUnit;
        EnemyUnit = enemyUnit;
        Name = name;
        
        playerUnit.AbilitiesDescriptions.Add($"Удар мечом ({playerUnit.Damage} урона)");
        playerUnit.AbilitiesDescriptions.Add($"Щит (следующая атака противника не нанесет урона)");
        playerUnit.AbilitiesDescriptions.Add($"Огненный шар ({playerFireballDamage} урона)");
        playerUnit.AbilitiesDescriptions.Add($"Лечение (восстанавливает {playerHealAmount} здоровья, " +
                                             $"если прошлый удар противника был нанесен не оружием)");
    }
    
    public void Turn()
    {
        Writer.WriteStatus(Name, PlayerUnit);
        Writer.WriteStatus("Enemy", EnemyUnit);
        
        Writer.WriteAllAbilities($"{Name}! Выберите действие: ", PlayerUnit);

        switch (Input.GetNumber(4))
        {
            case 1:
                EnemyUnit.TakeDamage(PlayerUnit, PlayerUnit.Damage, true);
                break;
            case 2:
                PlayerUnit.ShieldCount = 1;
                Writer.WriteShielded(Name);
                break;
            case 3:
                EnemyUnit.TakeDamage(PlayerUnit, playerFireballDamage);
                break;
            case 4:
                PlayerUnit.TakeDamage(PlayerUnit,
                    PlayerUnit.LastDamageFromWeapon ? 0 : -playerHealAmount);
                break;
        }
    }
}