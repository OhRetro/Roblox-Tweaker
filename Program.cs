//Roblox Tweaker

using static RobloxTweaker.MainFiles.MainVariables;
using static RobloxTweaker.MainFiles.OtherUtils;
using static RobloxTweaker.MainFiles.TexturesManager;
using static RobloxTweaker.MainFiles.VersionDir;

namespace RobloxTweaker
{
    internal class Program
    {
        const string NAME = "Roblox Tweaker";
        const string VERSION = "3.1";
        const string AUTHOR = "OhRetro";
        const string NAME_VERSION = NAME + " v" + VERSION;
        const string REPOSITORY = "https://github.com/OhRetro/Roblox-Tweaker";

        //About
        static void About()
        {
            Console.WriteLine("[About]");
            Console.WriteLine(NAME_VERSION);
            Console.WriteLine("Made by {0}", AUTHOR);
            Console.WriteLine("Repository: {0}", REPOSITORY);

            Continue(true);
        }

        static void Main()
        {
            Console.Title = NAME_VERSION;
            Console.Clear();

            Console.WriteLine("{0} by {1}\n", NAME_VERSION, AUTHOR);
            ReadFile();

            Console.Clear();

            int menu;
            do
            {
                Console.WriteLine(NAME_VERSION);
                Console.WriteLine("[1] Delete Textures\n[2] List Textures\n[3] Update Version Directory");
                Console.WriteLine("[4] Restore Textures\n");
                Console.WriteLine("[5] About\n[0] Exit\n");
                Console.WriteLine("Current Version Directory:\n\"{0}\"\nType: {1}", ROBLOX_VERSION_DIR, ROBLOX_VERSION_DIR_TYPE);

                Console.Write(">");
                try
                {
                    menu = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    menu = -1;
                }

                Console.Clear();

                switch (menu)
                {
                    case 1:
                        Remove();
                        break;
                    case 2:
                        List();
                        break;
                    case 3:
                        Update();
                        break;
                    case 4:
                        Restore();
                        break;
                    case 5:
                        About();
                        break;
                    default:
                        break;
                }
            } while (menu != 0);
        }
    }
}