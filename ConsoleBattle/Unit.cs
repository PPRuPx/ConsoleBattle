namespace ConsoleBattle;

public class Unit
{
    public int Damage { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public bool IsAlive => CurrentHealth > 0;

    public int ShieldCount = 0;
    public bool LastDamageFromWeapon { get; set; }

    public List<string> AbilitiesDescriptions = new();
    public Dictionary<AttackType, List<float>> DamageHistory;
    
    public Unit(int maxHealth, int damage)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        Damage = damage;
        
        DamageHistory = new();
        DamageHistory[AttackType.Direct] = new();
        DamageHistory[AttackType.Self] = new();
        DamageHistory[AttackType.Heal] = new();
    }
    
    public void TakeDamage(Unit origin, int damage, bool isWeaponDamage = false)
    {
        LastDamageFromWeapon = isWeaponDamage;
        
        bool isHeal = damage < 0;
        AttackType attackType = AttackType.Direct;
        if (isHeal) 
            attackType = AttackType.Heal;
        else if (origin == this) 
            attackType = AttackType.Self;
        
        if (IsShielded() && !isHeal)
        {
            ShieldCount--;
            DamageHistory[attackType].Add(0);
            return;
        }
        
        CurrentHealth -= damage;
        DamageHistory[attackType].Add(damage);
        
        if (CurrentHealth > MaxHealth) 
            CurrentHealth = MaxHealth;
    }
    
    public string GetAbilityDescription(int abilityNumber)
    {
        return AbilitiesDescriptions[abilityNumber];
    }
    
    public int GetAbilityCount()
    {
        return AbilitiesDescriptions.Count;
    }
    
    public bool IsShielded()
    {
        return ShieldCount > 0;
    }
    
}