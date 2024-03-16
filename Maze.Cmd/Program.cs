namespace Maze.Cmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Bienvenue sur Maze !");
            Console.ResetColor();
            Console.WriteLine("Quel est votre nom jeune aventurier ?");
            var name = Console.ReadLine();
            Player Player = new Player(name);
            Player.LevelUp += Player_Level_Up;
            Player.PotionFound += Player_Found_Potion;
            Player.EquipementFound += Player_Found_Equipement;
            Player.Trap += Player_Trap;
            var game = new Game(Player);
            game.Start();
        }

        private static void Player_Level_Up(object? sender, LevelUpEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{e.Name} est niveau {e.Level} !");
            Console.ResetColor();
        }
        private static void Player_Trap(object? sender, TrapEventArgs e)
        {
            if (e.Equipement != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{e.PlayerName} a évité {e.Trap.Type} ! Il utilise {e.Equipement} !");
                Console.ResetColor();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{e.PlayerName} est tombé dans {e.Trap.Type} ! ");
            Console.ResetColor();
        }
        private static void Player_Found_Potion(object? sender, ItemFoundEventArgs<Potion> e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{e.PlayerName} a trouvé une {e.Item} !");
            Console.ResetColor();
        }

        private static void Player_Found_Equipement(object? sender, ItemFoundEventArgs<Equipement> e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{e.PlayerName} a trouvé {e.Item.Type} !");
            Console.ResetColor();
        }
    }
}