using System.Security.Cryptography;
using System.Xml.Linq;

namespace Maze.Cmd
{
    public static class EntityFactory
    {
        public static Entity Generate(int Level)
        {
            var luck = Dice.Roll(1, 11);
            return luck > 1 ? new Squelette((int)Math.Round(luck*(Level*0.75) + 20), luck, luck) : new Boss(500, "Boss", 20, 10);
        }
    }
    public abstract class Entity
    {
        private int hp;

        public int Hp
        {
            get
            {
                return hp;
            }
            set
            {
                if (value > HpMax)
                {
                    hp = HpMax;
                    return;
                }
                if (value <= 0)
                {
                    IsDead = true;
                    hp = 0;
                    return;
                }
                hp = value;
                if (hp < 0)
                {
                    IsDead = true;
                }
            }
        }
        public bool IsDead = false;
        public int HpMax { get; set; }
        public string Name { get; set; }
        public int Offense { get; set; }
        public int Defense { get; set; }
        public Entity(int hp, string name, int offense, int defense)
        {
            HpMax= hp;
            Hp = hp;
            Name = name;
            Offense = offense;
            Defense = defense;
        }
        public void Attack(Player p)
        {
            var luck = Dice.Roll(1, 11);
            switch (luck)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Coup Critique ! [{Offense * 2}]");
                    Console.ResetColor();
                    p.Hp -= Offense * 2;
                    break;
                case 10:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Vous esquivez ! [0]");
                    Console.ResetColor();
                    p.Hp -= 0;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Il vous inflige {Offense} de dégat !");
                    Console.ResetColor();
                    p.Hp -= Offense;
                    break;
            }

        }
        public int Drop()
        {
            return (int)Math.Round(Hp*0.75);
        }
        public override string ToString()
        {
            return $"{Name} [{Hp}/{HpMax} HP] [{Offense} Off] [{Defense} Def]";
        }

    }
    public class Squelette : Entity
    {
        public Squelette(int hp, int offense, int defense, string name = "Squelette") : base(hp, name, offense, defense)
        {

        }
    }
    public class Boss : Entity
    {
        public Boss(int hp, string name, int offense, int defense) : base(hp, name, offense, defense)
        {

        }
    }
}