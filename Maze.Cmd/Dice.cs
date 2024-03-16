namespace Maze.Cmd
{
    public static class Dice
    {
        private  readonly static Random rand = new Random();
        public static  int Roll(int mini = 1, int max = 7)
        {
            return rand.Next(mini, max);
        }
    }
}