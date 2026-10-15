using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _3ИСИП_424
{
    internal class Program
    {
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
            public string Name { get; set; } = "Меч";
            public int Damage { get; set; } = 10;
        }
        public class Shield : Armor
        {
            public string Name { get; set; } = "Щит";
            public int Defense { get; set; } = 5;
        }

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
            }
            public bool IsAlive => HP > 0;
            public void EquipWeapon(Weapon weapon)
            {
                EquippedWeapon = weapon;
                Console.WriteLine($"{Name} экипировал {weapon.Name}.");
            }
            public void EquipArmor(Armor armor)
            {
                EquippedArmor = armor;
                Console.WriteLine($"{Name} экипировал {armor.Name}.");
            }
            public void TakeDamage(int incomingDomage, bool ignoreArmor = false)
            {
                int defense = 0;
                if (EquippedArmor != null && !ignoreArmor)
                {
                    defense = EquippedArmor.Defense;
                }
                int finalDamage = Math.Max(0, incomingDomage - defense);
                HP -= finalDamage;
                Console.WriteLine($"{Name} получает {finalDamage} урона (Защита: {defense}). Осталось HP: {HP}");
                if (!IsAlive) Console.WriteLine($"{Name} умер!");
            }
            public int Attack()
            {
                if (EquippedWeapon != null)
                {
                    return 1;
                }
                return EquippedWeapon.Damage;
            }
            public void StartTurn()
            {
                if (IsFrozen)
                {
                    Console.WriteLine($"{Name} заморожен и пропускает ход!");
                    IsFrozen = false;
                }
            }
        }
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
                int actualDamage = Math.Max(0, damage - Defense);
                HP -= actualDamage;
                Console.WriteLine($"{Name} получает {actualDamage} урона. Осталось HP: {HP}");
            }
            public virtual AttackResult Attack(Player target)
            {
                return new AttackResult
                {
                    Damage = AttackPower,
                    IsCritical = false,
                    ApplyFreeze = false
                };
            }

        }
        public class AttackResult
        {
            public int Damage { get; set; }
            public bool IsCritical { get; set; }
            public bool ApplyFreeze { get; set; }
        }
        public class Goblin : Enemy
        {
            public Goblin() : base("Гоблин", 50, 8, 2) { }
            public override AttackResult Attack(Player target)
            {
                var result = base.Attack(target);

                if (random.Next(100) < 25)
                {
                    result.Damage *= 2;
                    result.IsCritical = true;
                }
                return result;
            }
        }
        public class Skeleton : Enemy
        {
            public Skeleton() : base("Скилет", 40, 12, 5) { }
            public override AttackResult Attack(Player target)
            {
                var result = base.Attack(target);
                return result;
            }
        }
        public int GetDamageIgnoringArmor()
        {
            return AttackPower;
        }
        public class Mage : Enemy
        {
            public Mage() : base("Маг", 35, 15, 0) { }

            public override AttackResult Attack(Player target)
            {
                var result = base.Attack(target);

                if (random.Next(100) < 30)
                {
                    result.ApplyFreeze = true;
                }
                return result;
            }
        }
            static void Main(string[] args)
            {

            }
        
    }

}
