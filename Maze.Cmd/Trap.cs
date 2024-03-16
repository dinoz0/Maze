namespace Maze.Cmd
{
    public enum TrapType
    {
        Pierres = 0, Trou = 1, Fleches = 2
    }
    public class Trap
    {
        public static Trap? Generate()
        {
            var randNum = Dice.Roll();
            switch (randNum)
            {
                case 1:
                case 2:
                    return new Trap(TrapType.Trou, Dice.Roll(10, 21));
                case 3:
                case 4:
                    return new Trap(TrapType.Pierres, Dice.Roll(5, 11));
                case 5:
                case 6:
                    return new Trap(TrapType.Fleches, Dice.Roll(1, 6));
                default:
                    return null;
            }
        }
        public int Damage { get; private set; }
        public TrapType Type { get; private set; }
        private Trap(TrapType type, int damage)
        {
            Type = type;
            Damage = damage;
        }
        public override string ToString()
        {
            return $"{Type}";
        }
    }
}