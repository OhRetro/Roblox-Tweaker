//Roblox Tweaker

using static RobloxTweaker.Workings.MainVariables;
using static RobloxTweaker.Workings.OtherUtils;
using static RobloxTweaker.Workings.TexturesManager;
using static RobloxTweaker.Workings.VersionDir;

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
            string[] extras = {
                NAME_VERSION,
                string.Format("Made by {0}", AUTHOR),
                string.Format("Repository: {0}", REPOSITORY)
            };

            _ = GenerateMenu("[About]", Array.Empty<string>(), extras, 1);
        }

        static void Main()
        {
            Console.Title = NAME_VERSION;
            Console.Clear();

            Console.WriteLine("{0} by {1}\n", NAME_VERSION, AUTHOR);
            ReadFile();

            int menu;
            string[] options = {
                "Delete Textures",
                "List Textures",
                "Update Version Directory",
                "Restore Textures\n",
                "About"
            };
            string[] extras = {
                string.Format("Current Version Directory: {0}", ROBLOX_VERSION_DIR),
                string.Format("Type: {0}", ROBLOX_VERSION_DIR_TYPE)
            };
            do
            {
                menu = GenerateMenu(NAME_VERSION, options, extras, 0, 0, "\n");

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
                        Update(true);
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