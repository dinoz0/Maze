namespace Maze.Cmd
{
    public enum Difficulty
    {
        easy = 1,
        normal = 2,
        hard = 3
    }

    public class Game
    {
        public Player Player;
        public Difficulty difficulty;

        public Game(Player p)
        {
            Player = p;
        }

        public void Start()
        {
            Console.WriteLine("Dans quel difficulté voulez vous jouer ? [1: easy, 2: normal, 3: hard]");
            string dif = Console.ReadLine();
            int testDif;
            if (string.IsNullOrEmpty(dif) || !int.TryParse(dif, out testDif) || testDif is not > 0 and < 4)
            {
                Start();
            }
            testDif = int.Parse(dif);
            difficulty = (Difficulty)testDif;
            Console.WriteLine($"La difficulté est: {difficulty}");
            Play();
        }

        public void Play()
        {
            while (!Player.IsDead)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(Player);
                Console.ResetColor();
                Console.WriteLine("Voulez vous lancer le dé ? [Y/n] Ou boire une potion ? [p]");
                var res = Console.ReadLine();
                if (res.ToLower() == "n")
                {
                    Console.WriteLine("Boouuuuh le nul !");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                if (res.ToLower() == "p")
                {
                    int index = 0;
                    foreach (var item in Player.GetPotionInventory())
                    {
                        Console.WriteLine($"[{index}] {item}");
                        index++;
                    }
                    Console.WriteLine("Choisissez une potion");
                    var resIndex = Console.ReadLine();
                    try
                    {
                        var numIndex = int.Parse(resIndex);
                        Player.DrinkPotion(Player.GetPotionInventory()[numIndex]);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cette potion n'existe pas !");
                        continue;
                    }
                }
                var result = Dice.Roll(1, 9);
                Console.WriteLine($"Vous avez fait {result}");
                Console.ReadLine();
                switch (result)
                {
                    case 1:
                    case 2:
                        Player.FallTrap(Trap.Generate());
                        break;

                    case 3:
                        Player.Walk("Vous allez a gauche...");
                        break;

                    case 4:
                    case 5:
                        Player.Walk("Vous allez tout droit...");
                        break;

                    case 6:
                        Player.Walk("Vous allez a droite...");
                        break;

                    case 7:
                    case 8:
                        var entity = EntityFactory.Generate(Player.Level);
                        Encounter(entity);
                        break;
                }
                if (Player.IsDead)
                {
                    Lose();
                }
            }
        }

        private void Lose()
        {
            Console.WriteLine("Vous etes mort !");
            Console.WriteLine($"Vous avez gagné {Player.Exp} exp !");
            Replay();
        }

        private void Replay()
        {
            Console.WriteLine("Voulez vous rejouer ? [Y/n]");
            string resDead = Console.ReadLine();
            if (resDead.ToLower() == "n")
            {
                Console.WriteLine("Boouuuuh le nul !");
                Console.ReadLine();
                Environment.Exit(0);
            }
            Start();
        }

        private void Encounter(Entity entity)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ho non un monstre : {entity.Name} !!");
            while (!entity.IsDead)
            {
                Console.WriteLine("Que voulez vous faire ? [A : attack / f : fuir]");
                Console.ResetColor();
                var res = Console.ReadLine();
                if (res == "f")
                {
                    var luck = Dice.Roll(1, 11);
                    if (luck == 10)
                    {
                        Console.WriteLine("Vous vous échapper !");
                        Console.ReadLine();
                        return;
                    }
                    Console.WriteLine("Vous n'avez pas réussi a vous echapper !");
                    Console.ReadLine();
                }
                else
                {
                    Player.Attack(entity);
                }
                if (!entity.IsDead)
                {
                    Console.WriteLine("Il riposte !");
                    entity.Attack(Player);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(entity);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(Player);
                    Console.ResetColor();
                }
                if (Player.IsDead)
                {
                    Lose();
                }
            }
        }
    }
}