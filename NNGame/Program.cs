using System;

namespace NNGame
{
    /// <summary>
    /// Entry thread
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main thread entry point.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            using var game = new Main();
            game.Run();
        }
    }
}