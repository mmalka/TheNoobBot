using System;

namespace meshDisplay
{

    static class Program
    {
        public static Game Game { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Game = new Game();
            using (Game)
            {
                Game.Run();
            }
        }
    }

}

