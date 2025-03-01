namespace ConsoleBattle;

public class Enemy
{
    public Input Input;
    public Unit PlayerUnit;
    public Unit EnemyUnit;
    public int AbilitiesCount;
    public int SecondAbilityModifier;
    public int ThirdAbilityValue;

    public Enemy(Input input, Unit playerUnit, Unit enemyUnit)
    {
        Input = input;
        PlayerUnit = playerUnit;
        EnemyUnit = enemyUnit;
        AbilitiesCount = 3;
        SecondAbilityModifier = 3;
        ThirdAbilityValue = 100;
    }

    public void Turn()
    {
        switch (Input.GetEnemyCommandNumber(AbilitiesCount))
        {
            case 1:
                PlayerUnit.TakeDamage(EnemyUnit, EnemyUnit.Damage, true);
                break;
            case 2:
                EnemyUnit.TakeDamage(EnemyUnit, EnemyUnit.Damage);
                PlayerUnit.TakeDamage(EnemyUnit, EnemyUnit.Damage * SecondAbilityModifier);
                break;
            case 3:
                EnemyUnit.TakeDamage(EnemyUnit,
                    EnemyUnit.LastDamageFromWeapon ? ThirdAbilityValue : -ThirdAbilityValue);
                break;
        }
    }
}