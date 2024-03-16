namespace Maze.Cmd
{
    public class ItemFoundEventArgs<T>
    {
        public T? Item;
        public string PlayerName { get; set; } = "";
    }
    public class TrapEventArgs: EventArgs
    {
        public Trap Trap;
        public Equipement? Equipement;
        public string PlayerName { get; set; }
    }

    public class LevelUpEventArgs : EventArgs
    {
        public int Level { get; set; }
        public string Name { get; set; } = "";
    }
}