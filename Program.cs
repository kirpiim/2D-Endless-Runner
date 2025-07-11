using System;

namespace _2DEndlessRunner
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}

//using var game = new _2DEndlessRunner.Game1();
//game.Run();
