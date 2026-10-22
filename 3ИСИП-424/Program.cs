using System;
using System.Collections.Generic;

namespace _3ИСИП_424
{
    // ИНТЕРФЕЙСЫ И ПРЕДМЕТЫ
    public interface Equipment
    {
        string Name { get; }
    }

    public interface Weapon : Equipment
    {
        int Damage { get; }
    }

    public interface Armor : Equipment
    {
        int Defense { get; }
    }

    public class Sword : Weapon
    {
        public string Name { get; set; } = "Ржавый меч";
        public int Damage { get; set; } = 10;

        public Sword(string name, int damage) { Name = name; Damage = damage; }
        public Sword() { }
    }

    public class Shield : Armor
    {
        public string Name { get; set; } = "Кожаный щит";
        public int Defense { get; set; } = 5;

        public Shield(string name, int defense) { Name = name; Defense = defense; }
        public Shield() { }
    }

    // ИГРОК
    public class Player
    {
        public string Name { get; set; }
        public int HP { get; private set; }
        public int maxHP { get; private set; }
        public Weapon EquippedWeapon { get; private set; }
        public Armor EquippedArmor { get; private set; }
        public bool IsFrozen { get; set; } = false;

        public Player(string name, int hp)
        {
            Name = name;
            maxHP = hp;
            HP = hp;
            EquippedWeapon = new Sword("Кулаки", 150); // Базовое оружие
            EquippedArmor = new Shield("Одежда", 5); // Базовая броня
        }

        public bool IsAlive => HP > 0;

        public void EquipWeapon(Weapon weapon)
        {
            EquippedWeapon = weapon;
            Console.WriteLine($"Вы экипировали {weapon.Name}.");
        }

        public void EquipArmor(Armor armor)
        {
            EquippedArmor = armor;
            Console.WriteLine($"Вы экипировали {armor.Name}.");
        }

        public void Heal(int amount)
        {
            HP += amount;
            if (HP > maxHP) HP = maxHP;
            Console.WriteLine($"Вы восстановили здоровье! Текущее HP: {HP}");
        }

        public void TakeDamage(int incomingDamage, bool ignoreArmor = false)
        {
            int defense = 0;
            if (EquippedArmor != null && !ignoreArmor)
            {
                defense = EquippedArmor.Defense;
            }
            int finalDamage = Math.Max(0, incomingDamage - defense);
            HP -= finalDamage;
            Console.WriteLine($"{Name} получает {finalDamage} урона (Защита: {defense}). Осталось HP: {HP}");
            if (!IsAlive) Console.WriteLine($"{Name} погиб!");
        }

        public int GetAttackDamage()
        {
            return EquippedWeapon != null ? EquippedWeapon.Damage : 1;
        }

        // Логика защиты игрока
        public int Defend()
        {
            Random rnd = new Random();
            // 40% шанс полного уклонения
            if (rnd.Next(100) < 40)
            {
                Console.WriteLine("Вы успешно уклонились от атаки!");
                return -1; // -1 = сигнал "полное уклонение"
            }

            int defenseValue = EquippedArmor?.Defense ?? 0;
            double reductionPercent = rnd.Next(70, 101) / 100.0;
            int blockedAmount = (int)(defenseValue * reductionPercent);
            Console.WriteLine($"Вы заблокировали часть урона ({blockedAmount}).");
            return blockedAmount;
        }
    }

    // ВРАГИ
    public abstract class Enemy
    {
        public string Name { get; protected set; }
        public int HP { get; protected set; }
        public int MaxHP { get; protected set; }
        public int AttackPower { get; protected set; }
        public int Defense { get; protected set; }
        protected Random random = new Random();

        public Enemy(string name, int hp, int attack, int defense)
        {
            Name = name;
            MaxHP = hp;
            HP = hp;
            AttackPower = attack;
            Defense = defense;
        }

        public bool IsAlive => HP > 0;

        public virtual void TakeDamage(int damage)
        {
            int actualDamage = Math.Max(1, damage - Defense);
            HP -= actualDamage;
            Console.WriteLine($"{Name} получает {actualDamage} урона. Осталось HP: {HP}");
        }

        public virtual AttackResult Attack(Player target)
        {
            return new AttackResult
            {
                Damage = AttackPower,
                IsCritical = false,
                ApplyFreeze = false,
                IgnoreArmor = false
            };
        }
    }

    public class AttackResult
    {
        public int Damage { get; set; }
        public bool IsCritical { get; set; }
        public bool ApplyFreeze { get; set; }
        public bool IgnoreArmor { get; set; }
    }

    public class Goblin : Enemy
    {
        public Goblin() : base("Гоблин", 50, 8, 2) { }
        public override AttackResult Attack(Player target)
        {
            var result = base.Attack(target);
            // Шанс крита 25%
            if (random.Next(100) < 25)
            {
                result.Damage *= 2;
                result.IsCritical = true;
                Console.WriteLine("КРИТИЧЕСКИЙ УДАР!");
            }
            return result;
        }
    }

    public class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", 40, 12, 5) { }
        public override AttackResult Attack(Player target)
        {
            var result = base.Attack(target);
            result.IgnoreArmor = true; // Игнорирует защиту
            return result;
        }
    }

    public class Mage : Enemy
    {
        public Mage() : base("Маг", 35, 15, 0) { }

        public override AttackResult Attack(Player target)
        {
            var result = base.Attack(target);
            // Шанс заморозки 30%
            if (random.Next(100) < 30)
            {
                result.ApplyFreeze = true;
                Console.WriteLine("Маг накладывает заморозку!");
            }
            return result;
        }
    }
    // БОССЫ
    // 1. ВВГ (Гоблин) - х2 HP, x1.5 Атака, x1.2 Защита, +10% крит
    public class GoblinBoss : Goblin
    {
        public GoblinBoss()
        {
            Name = "ВВГ (Босс)";
            MaxHP = (int)(50 * 2.0);
            HP = MaxHP;
            AttackPower = (int)(8 * 1.5);
            Defense = (int)(2 * 1.2);
        }

        public override AttackResult Attack(Player target)
        {
            var result = base.Attack(target);
            // Базовый шанс крита у гоблина был 25%, добавляем 10% -> 35%
            return result;
        }
    }
    // 2. Ковальский (Скелет) - х2.5 HP, x1.3 Атака, x1.4 Защита, Игнор брони
    public class SkeletonBoss : Skeleton
    {
        public SkeletonBoss()
        {
            Name = "Ковальский (Босс)";
            MaxHP = (int)(40 * 2.5);
            HP = MaxHP;
            AttackPower = (int)(12 * 1.3);
            Defense = (int)(5 * 1.4);
        }
    }
    // 3. Архимаг C++ (Мар) - х1.8 HP, x1.6 Атака, x1.1 Защита, +10% заморозка
    public class MageBoss : Mage
    {
        public MageBoss()
        {
            Name = "Архимаг C++ (Босс)";
            MaxHP = (int)(35 * 1.8);
            HP = MaxHP;
            AttackPower = (int)(15 * 1.6);
            Defense = (int)(0 * 1.1);
        }
        public override AttackResult Attack(Player target)
        {
            var result = base.Attack(target);
            if (!result.ApplyFreeze && new Random().Next(100) < 10)
            {
                result.ApplyFreeze = true;
                Console.WriteLine("Архимаг C++ накладывает заморозку!");
            }
            return result;
        }
    }
    public class PestovBoss : Skeleton
    {
        public PestovBoss()
        {
            Name = "Пестов (Босс)";
            MaxHP = (int)(40 * 1.3);
            HP = MaxHP;
            AttackPower = (int)(12 * 1.8);
            Defense = (int)(5 * 0.6);
        }

        public override AttackResult Attack(Player target)
        {
            var result = base.Attack(target);
            // Шанс заморозки 15%
            if (new Random().Next(100) < 15)
            {
                result.ApplyFreeze = true;
                Console.WriteLine("Пестов накладывает заморозку!");
            }
            return result;
        }
    }

    // ИГРОВАЯ ЛОГИКА
    internal class Program
    {
        static Random rnd = new Random();
        static Player player;
        static int turnCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в Игру!");
            Console.Write("Введите имя героя: ");
            string name = Console.ReadLine();
            player = new Player(name, 100);

            while (player.IsAlive)
            {
                turnCount++;
                Console.WriteLine($"\nХод {turnCount}");
                Console.WriteLine($"HP: {player.HP}/{player.maxHP} Оружие: {player.EquippedWeapon.Name} ({player.EquippedWeapon.Damage})" +
                    $" Броня: {player.EquippedArmor.Name} ({player.EquippedArmor.Defense})");
                // Проверка на босса каждые 10 ходов
                if (turnCount % 10 == 0)
                {
                    Console.WriteLine("ВНИМАНИЕ! Появляется БОСС!");
                    Enemy boss = CreateRandomBoss();
                    StartBattle(boss);
                }
                else
                {
                    // 50 на 50: Сундук или Враг
                    if (rnd.Next(2) == 0)
                    {
                        OpenChest();
                    }
                    else
                    {
                        Enemy enemy = CreateRandomEnemy();
                        StartBattle(enemy);
                    }
                }
                if (!player.IsAlive) break;
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            Console.WriteLine($"\nИгра окончена. Вы продержались {turnCount} ходов.");
        }
        static Enemy CreateRandomEnemy()
        {
            int type = rnd.Next(3);
            switch (type)
            {
                case 0: return new Goblin();
                case 1: return new Skeleton();
                default: return new Mage();
            }
        }
        static Enemy CreateRandomBoss()
        {
            int type = rnd.Next(4);
            switch (type)
            {
                case 0: return new GoblinBoss();
                case 1: return new SkeletonBoss();
                case 2: return new MageBoss();
                default: return new PestovBoss();
            }
        }
        static void OpenChest()
        {
            Console.WriteLine("Вы нашли сундук!");
            int drop = rnd.Next(3); // 0 - зелье, 1 - оружие, 2 - броня
            if (drop == 0)
            {
                Console.WriteLine("В сундуке лечебное зелье!");
                player.Heal(50);
            }
            else if (drop == 1)
            {
                Weapon newWeapon = GenerateWeapon();
                Console.WriteLine($"В сундуке оружие: {newWeapon.Name} (Урон: {newWeapon.Damage})");
                Console.WriteLine($"Текущее оружие: {player.EquippedWeapon.Name} (Урон: {player.EquippedWeapon.Damage})");
                Console.Write("Взять новое оружие? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    player.EquipWeapon(newWeapon);
                }
            }
            else
            {
                Armor newArmor = GenerateArmor();
                Console.WriteLine($"В сундуке броня: {newArmor.Name} (Защита: {newArmor.Defense})");
                Console.WriteLine($"Текущая броня: {player.EquippedArmor.Name} (Защита: {player.EquippedArmor.Defense})");
                Console.Write("Взять новую броню? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    player.EquipArmor(newArmor);
                }
            }
        }
        static Weapon GenerateWeapon()
        {
            string[] names = { "Стальной меч", "Топор", "Копье", "Кинжал" };
            string name = names[rnd.Next(names.Length)];
            int damage = rnd.Next(10, 25);
            return new Sword(name, damage);
        }
        static Armor GenerateArmor()
        {
            string[] names = { "Кольчуга", "Стальной щит", "Латы", "Мантия" };
            string name = names[rnd.Next(names.Length)];
            int defense = rnd.Next(5, 15);
            return new Shield(name, defense);
        }
        static void StartBattle(Enemy enemy)
        {
            Console.WriteLine($"\nВы встретили врага: {enemy.Name} (HP: {enemy.HP}, Атака: {enemy.AttackPower}, Защита: {enemy.Defense})");
            while (enemy.IsAlive && player.IsAlive)
            {
                // Ход игрока
                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                }
                else
                {
                    Console.WriteLine("\nВаш ход. Выберите действие:");
                    Console.WriteLine("1. Атака");
                    Console.WriteLine("2. Защита (шанс уклонения 40% или блок)");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        int dmg = player.GetAttackDamage();
                        // Крит игрока
                        enemy.TakeDamage(dmg);
                    }
                    else if (choice == "2")
                    {
                        // Игрок защищается. 
                        Console.WriteLine("Вы готовитесь к защите...");
                        int blocked = player.Defend(); // Возвращает 0 при уклонении или величину блока

                        if (blocked == 0 && rnd.Next(100) < 40)
                        {
                            continue;
                        }
                        else
                        {
                            playerDefending = true;
                        }
                    }
                }
                if (!enemy.IsAlive) break;
                // Ход врага
                Console.WriteLine($"\n{enemy.Name} атакует!");
                AttackResult result = enemy.Attack(player);
                int finalDamage = result.Damage;
                // Если игрок защищался
                if (playerDefending)
                {
                    int block = player.Defend();
                    if (block < 0)
                        finalDamage = 0; // уклонение
                    else
                        finalDamage = Math.Max(0, finalDamage - block);
                }
                if (finalDamage > 0)
                {
                    player.TakeDamage(finalDamage, result.IgnoreArmor);
                }
                else
                {
                    Console.WriteLine("Атака не нанесла урона!");
                }

                if (result.ApplyFreeze)
                {
                    player.IsFrozen = true;
                }
            }
            if (player.IsAlive)
            {
                Console.WriteLine($"Вы победили {enemy.Name}!");
            }
        }
        static bool playerDefending = false;
    }
}
