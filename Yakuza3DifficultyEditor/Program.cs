using LibYakuza3Difficulty;
using System.Diagnostics;
using System.Reflection;

namespace Yakuza3DifficultyEditor
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                if (args.Length >= 3 || args.Length <= 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid Arguments: " + args.ToString());
                }
                Y3Save.ChangeDifficulty(args[1], args[0].ToLower());
            }
            catch(Exception e)
            {
                PrintHelp(e.Message);
                return;
            }
        }

        private static void PrintHelp(string exception)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string? fullName = Assembly.GetEntryAssembly().Location;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            string appName = Path.GetFileNameWithoutExtension(fullName);

            if (!String.IsNullOrEmpty(exception))
            {
                Console.WriteLine("Error: " + exception);
            }
            else
            {
                Console.WriteLine("Simple tool to change the difficulty of a Yakuza 3 save file.");
            }
            
            Console.WriteLine("usage: " + appName + " [difficulty] <.sav file from Yakuza 3>");
            Console.WriteLine(" ");
            Console.WriteLine("Valid Difficulties:  Easy, Normal, Hard, Legendary");
        }
    }
}