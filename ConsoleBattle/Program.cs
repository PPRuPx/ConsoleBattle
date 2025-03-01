namespace ConsoleBattle
{

    internal enum AttackType { Direct, Self, Heal }
    
    internal class Program
    {
        
        static void Main(string[] args)
        {
            string? playerName;
            string? input;
            
            float currentPlayerHealth;
            float currentEnemyHealth;
            
            float maxPlayerHealth = 1000f;
            float maxEnemyHealth = 2000f;
            
            float playerMeleeDamage = 50f;
            float playerFireballDamage = 200f;
            float playerHealAmount = 100f;
            
            float enemyDamage = 55f;
            float enemyAbilityModifier = 3f;
            float enemyHealAmount = 100f;
            
            Random random = new Random();

            Console.WriteLine("Введите ваше имя:");
            playerName = Console.ReadLine();
            
            while (true)
            {
                Console.Clear();    
                Console.WriteLine($"Добро пожаловать в Console Battle, {playerName}!");
                
                Dictionary<AttackType, List<float>> playerAttacks = new();
                Dictionary<AttackType, List<float>> enemyAttacks = new();
                
                playerAttacks[AttackType.Direct] = new List<float>();
                playerAttacks[AttackType.Self] = new List<float>();
                playerAttacks[AttackType.Heal] = new List<float>();
                
                enemyAttacks[AttackType.Direct] = new List<float>();
                enemyAttacks[AttackType.Self] = new List<float>();
                enemyAttacks[AttackType.Heal] = new List<float>();
                
                currentPlayerHealth = maxPlayerHealth;
                currentEnemyHealth = maxEnemyHealth;
                    
                bool isLastEnemyMeleeAttack = false;
                
                while (currentPlayerHealth > 0 && currentEnemyHealth > 0)
                {
                    WriteHealth(playerName, currentPlayerHealth, maxPlayerHealth);
                    WriteHealth("Enemy", currentEnemyHealth, maxEnemyHealth);

                    Console.WriteLine($"{playerName}! Выберите действие:");
                    Console.WriteLine($"1. Удар мечом ({playerMeleeDamage} урона)");
                    Console.WriteLine($"2. Укрыться щитом (следующая атака противника не нанесет урона)");
                    Console.WriteLine($"3. Огненный шар ({playerFireballDamage} урона)");
                    if (isLastEnemyMeleeAttack)
                        Console.WriteLine($"4. Перевязка (восстановление {playerHealAmount} здоровья)");

                    bool isActionDone = false;
                    bool isPlayerShielded = false;
                    bool isPlayerMeleeAttack = false;

                    do
                    {
                        input = Console.ReadLine();
                        switch (input)
                        {
                            case "1":
                                // Удар мечом
                                isPlayerMeleeAttack = true;
                                currentEnemyHealth -= playerMeleeDamage;
                                playerAttacks[AttackType.Direct].Add(playerMeleeDamage);
                                isActionDone = true;
                                break;
                            case "2":
                                // Укрыться щитом
                                isPlayerShielded = true;
                                WriteShielded(playerName);
                                isActionDone = true;
                                break;
                            case "3":
                                // Огненный шар
                                currentEnemyHealth -= playerFireballDamage;
                                playerAttacks[AttackType.Direct].Add(playerFireballDamage);
                                isActionDone = true;
                                break;
                            case "4":
                                // Перевязка
                                if (isLastEnemyMeleeAttack)
                                {
                                    var actualHealAmount = currentPlayerHealth + playerHealAmount > maxPlayerHealth
                                        ? maxPlayerHealth - currentPlayerHealth : playerHealAmount;
                                    currentPlayerHealth += actualHealAmount;
                                    playerAttacks[AttackType.Heal].Add(actualHealAmount);
                                    isActionDone = true;
                                }
                                else
                                {
                                    Console.WriteLine("Действие недоступно, повторите ввод...");
                                }
                                break;
                            default:
                                Console.WriteLine("Команда не найдена, повторите ввод...");
                                break;
                        }
                    } while (!isActionDone);
                    
                    switch (random.Next(1, 4))
                    {
                        case 1:
                            // Удар мечом
                            if (!isPlayerShielded)
                            {
                                currentPlayerHealth -= enemyDamage;
                                enemyAttacks[AttackType.Direct].Add(enemyDamage);
                            }
                            isLastEnemyMeleeAttack = true;
                            break;
                        case 2:
                            // Сильный удар мечом
                            if (!isPlayerShielded)
                            {
                                currentEnemyHealth -= enemyDamage;
                                if (currentEnemyHealth <= 0) break;
                                currentPlayerHealth -= enemyDamage * enemyAbilityModifier;
                                enemyAttacks[AttackType.Self].Add(enemyDamage);
                                enemyAttacks[AttackType.Direct].Add(enemyDamage * enemyAbilityModifier);
                            }
                            isLastEnemyMeleeAttack = true;
                            break;
                        case 3:
                            // Лечение
                            if (isPlayerMeleeAttack)
                            {
                                currentEnemyHealth -= enemyHealAmount;
                                enemyAttacks[AttackType.Self].Add(enemyHealAmount);
                            }
                            else
                            {
                                var actualEnemyHealAmount = currentEnemyHealth + enemyHealAmount > maxEnemyHealth
                                    ? maxEnemyHealth - currentEnemyHealth : enemyHealAmount;
                                currentEnemyHealth += actualEnemyHealAmount;
                                enemyAttacks[AttackType.Heal].Add(actualEnemyHealAmount);
                            }
                            isLastEnemyMeleeAttack = false;
                            break;
                    }
                    
                    WriteAttacksFromTo(playerAttacks, playerName, "Enemy");
                    WriteAttacksFromTo(enemyAttacks, "Enemy", playerName);

                    EndTurn();
                }
            
                GameOver();
                if (!IsReplay()) break;
            }
            
            return;
            

            void EndTurn()
            {
                Console.WriteLine("Для продолжения нажмите любую клавишу...");
                Console.ReadKey();
                Console.Clear();
            }
            
            void WriteHealth(string target, float currentHealth, float maxHealth)
            {
                Console.Write($"Здоровье {target}: ");
                ConsoleColor color;
                if (currentHealth >= maxHealth * 0.6) 
                    color = ConsoleColor.Green;
                else if (currentHealth >= maxHealth * 0.3)
                    color = ConsoleColor.Yellow;
                else
                    color = ConsoleColor.Red;
                ConsoleWriteWithColor($"{currentHealth}\n", color);
            }
            
            void WriteDealDamage(string attacker, string target, float damage)
            {
                Console.Write($"{attacker} нанес ");
                ConsoleWriteWithColor($"{damage} ", ConsoleColor.Red);
                Console.WriteLine($"урона {target}");
            }
            
            void WriteSelfDamage(string target, float damage)
            {
                Console.Write($"{target} нанес сам себе ");
                ConsoleWriteWithColor($"{damage} ", ConsoleColor.Red);
                Console.WriteLine("урона");
            }
            
            void WriteHeal(string target, float amount)
            {
                Console.Write($"{target} восстановил себе ");
                ConsoleWriteWithColor($"{amount} ", ConsoleColor.Green);
                Console.WriteLine("здоровья");
            }
            
            void WriteShielded(string target)
            {
                ConsoleWriteWithColor($"{target} укрылся щитом\n", ConsoleColor.Yellow);
            }

            void WriteAttacksWithType(AttackType attackType, string attacker, string target, List<float> damages)
            {
                foreach (var damage in damages)
                {
                    switch (attackType)
                    {
                        case AttackType.Direct:
                            WriteDealDamage(attacker, target, damage);
                            break;
                        case AttackType.Self:
                            WriteSelfDamage(attacker, damage);
                            break;
                        case AttackType.Heal:
                            WriteHeal(attacker, damage);
                            break;
                    }
                }
                
                damages.Clear();
            }
            
            void WriteAttacksFromTo(Dictionary<AttackType, List<float>> attacks, string attacker, string target)
            {
                foreach (var attack in attacks)
                {
                    WriteAttacksWithType(attack.Key, attacker, target, attack.Value);
                }
            }
            
            void GameOver()
            {
                if (currentPlayerHealth <= 0)  
                    ConsoleWriteWithColor("Вы проиграли!\n", ConsoleColor.Red);
                else 
                    ConsoleWriteWithColor("Вы выиграли!\n", ConsoleColor.Green);
            }
            
            bool IsReplay()
            {
                Console.WriteLine("Хотите сыграть еще раз? (y/n)");
                input = Console.ReadLine();
                return input != "n";
            }
            
            void ConsoleWriteWithColor(string text, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                Console.Write(text);
                Console.ResetColor();
            }
        }
    }
}