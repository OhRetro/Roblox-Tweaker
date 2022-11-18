using System;
using static RobloxTweaker.MainVariables;
using static RobloxTweaker.OtherUtils;
using static RobloxTweaker.TexturesManager;
using static RobloxTweaker.VersionDir;

namespace RobloxTweaker
{
    internal class Program
    {
        private const string NAME = "Roblox Tweaker";
        private const string VERSION = "3.3";
        private const string AUTHOR = "OhRetro";
        private const string NAME_VERSION = NAME + " v" + VERSION;
        private const string REPOSITORY = "https://github.com/OhRetro/Roblox-Tweaker";

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
            do
            {
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