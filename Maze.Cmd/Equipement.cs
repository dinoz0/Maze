namespace Maze.Cmd
{
    public enum TypeEquipement
    {
        Casque = 0, Jetpack = 1, Bouclier = 2, Pomme = 3
    }
    public class Equipement
    {
        public TypeEquipement Type { get; }
        public static Equipement Generate()
        {
                var randNum = Dice.Roll(1, 8);
                switch (randNum)
                {
                    case 1:
                    case 2:
                        return new Equipement(TypeEquipement.Casque);
                    case 3:
                    case 4:
                    return new Equipement(TypeEquipement.Jetpack);
                case 5:
                    case 6:
                    return new Equipement(TypeEquipement.Bouclier);
                case 7:
                    return new Equipement(TypeEquipement.Pomme);
                default:
                        return null;
            }
        }
        private Equipement(TypeEquipement type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return $"{Type}";
        }
    }
}