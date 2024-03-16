namespace Maze.Cmd
{
    public class FactoryPotions 
    {
        public static Potion GenerateRandomPotions()
        {
            return Dice.Roll(1, 3) == 1 ? LifePotion.Generate() : ExpPotion.Generate();
        }
    }
    public class Potion
    {
        public int Range;
        public virtual void Affect(Player p)
        {
            p.Regenerate(Range);
        }
    }
    public class LifePotion : Potion
    {
        public static Potion Generate()
        {
            return new LifePotion()
            {
                Range = Dice.Roll(1, 4) == 1 ? 10 : 20,
            };
        }
        public override string ToString()
        {
            return "potion de vie";
        }

    }
    public class ExpPotion : Potion
    {
        public static Potion Generate()
        {
            return new ExpPotion()
            {
                Range = Dice.Roll(1, 4) == 1 ? 10 : 20,
            };
        }
        public override void Affect(Player p)
        {
            p.Exp += Range;
        }

        public override string ToString()
        {
            return "potion d'expérience";
        }

    }
}