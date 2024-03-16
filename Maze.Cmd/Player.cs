namespace Maze.Cmd
{
    public class Player
    {
        public event EventHandler<ItemFoundEventArgs<Equipement>> EquipementFound;
        public List<Equipement> equipements;
        public event EventHandler<ItemFoundEventArgs<Potion>> PotionFound;
        public List<Potion> potions;
        private int exp;

        public string Name { get; }
        public int Offense { get; set; }
        public Dictionary<TrapType, TypeEquipement> Counter { get; }
        public int Exp
        {
            get => exp;
            set
            {
                exp = value;
                if (exp >= ExpMax)
                {
                    ExpMax += GetLevel(1);
                    Level++;
                    HpMax += 10;
                    Offense += 2;
                    Hp = HpMax;
                    sumOfExp += GetLevel(-1);
                    LevelUp?.Invoke(this, new LevelUpEventArgs() { Level = Level, Name = Name });
                }
            }
        }
        private int hp { get; set; }
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
            }
        }
        public event EventHandler<LevelUpEventArgs> LevelUp;
        public int Level { get; private set; }
        /* Délégué standard du c# 
        /Action -> void ()
        /Action<T> -> void (T)
        /Func<T> -> T ()
        /Func<T,U> -> U (T)
        /EventHandler -> void (object sender, EventArgs args)
        */
        public int ExpMax { get; private set; }
        public int sumOfExp { get; private set; }
        public int HpMax { get; private set; }
        public bool IsDead { get; private set; }
        public event EventHandler<TrapEventArgs> Trap;

        public Player(string name, int hp = 100)
        {
            Counter = new Dictionary<TrapType, TypeEquipement>
            {
                { TrapType.Pierres, TypeEquipement.Casque },
                {TrapType.Trou, TypeEquipement.Jetpack },
                {TrapType.Fleches, TypeEquipement.Bouclier }
            };
            Offense = 5;
            equipements = new List<Equipement>();
            potions = new List<Potion>();
            Name = name;
            Level = 1;
            ExpMax = GetLevel();
            Exp = 0;
            IsDead = false;
            HpMax = hp;
            Hp = hp;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Salut {name} !");
            Console.ResetColor();
        }

        public int GetLevel(int delta = 0)
        {
            return (int)Math.Round(0.8 * Math.Pow((Level + 1 + delta), 3));
        }

        public void GetEquipement(Equipement equipement)
        {
            EquipementFound?.Invoke(this, new ItemFoundEventArgs<Equipement>() { Item = equipement, PlayerName = Name });
            if (equipement.Type == TypeEquipement.Pomme)
            {
                Regenerate(Dice.Roll(1, 10));
                return;
            }
            equipements.Add(equipement);
            Console.WriteLine($"Vous avez {string.Join(',', (Object[])equipements.ToArray())}");
        }

        public void GetPotion(Potion potion)
        {
            potions.Add(potion);
            PotionFound?.Invoke(this, new ItemFoundEventArgs<Potion>() { Item = potion, PlayerName = Name });
        }

        public void DrinkPotion(Potion potion)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Vous buvez une {potion}");
            Console.ResetColor();
            potion.Affect(this);
            potions.Remove(potion);
        }

        public Potion[] GetPotionInventory()
        {
            return potions.ToArray();
        }

        public void DestroyEquipement(Equipement eq)
        {
            equipements.Remove(eq);
        }

        public void Walk(string direction)
        {
            Console.WriteLine(direction);
            Console.ReadLine();
            Exp += 10;
            var rand = Dice.Roll(1, 100);
            if (rand < 11)
            {
                GetPotion(FactoryPotions.GenerateRandomPotions());
                return;
            }
            if (rand >= 75)
            {
                GetEquipement(Equipement.Generate());
                return;
            }
        }
        public void Regenerate(int hp)
        {
            Hp += hp;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Vous avez gagner {hp} HP !");
            Console.ResetColor();
            Console.ReadLine();
        }

        public void FallTrap(Trap? trap)
        {
            if (trap == null) throw new ArgumentNullException(nameof(trap));
            Equipement? counter = equipements.FirstOrDefault(item =>
            {
                return Counter[trap.Type] == item.Type;
            });
            if (counter == null)
            {
                Trap?.Invoke(this, new TrapEventArgs() { PlayerName = Name, Trap = trap });
                Hp -= trap.Damage;

                return;
            }
            Trap?.Invoke(this, new TrapEventArgs() { PlayerName = Name, Trap = trap, Equipement = counter });
            DestroyEquipement(counter);
        }
        public void Attack(Entity entity)
        {
            var luck = Dice.Roll(1, 11);
            switch (luck)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Coup Critique ! [{Offense * 2}]");
                    Console.ResetColor();
                    entity.Hp -= Offense * 2;
                    break;
                case 10:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"il esquive ! [0]");
                    Console.ResetColor();
                    entity.Hp -= 0;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Vous infligez {Offense} de dégat !");
                    Console.ResetColor();
                    entity.Hp -= Offense;
                    break;
            }
            if (entity.IsDead)
            {
                Console.WriteLine("Vous avez gagné !!");
                var loot = entity.Drop();
                Exp += loot;
            }
        }

        public override string ToString()
        {
            return $"[{Name}] [LVL {Level}] [{Hp}/{HpMax} HP] [{Exp - sumOfExp}/{(Level == 1 ? ExpMax : (ExpMax - sumOfExp))} XP]";
        }
    }
}