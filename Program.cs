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
        private const string VERSION = "3.4";
        private const string AUTHOR = "OhRetro";
        private const string NAME_VERSION = NAME + " v" + VERSION;
        private const string REPOSITORY = "https://github.com/OhRetro/Roblox-Tweaker";
        private static string NEW_VERSION = "";

        //About
        private static void About()
        {
            string[] extras = {
                NAME_VERSION,
                string.Format("Made by {0}", AUTHOR),
                string.Format("Repository: {0}", REPOSITORY)
            };

            _ = GenerateMenu("[About]", Array.Empty<string>(), extras, 1);
        }

        //Check for Update
        private static bool CheckForUpdate()
        {
            return false;
        }

        private static void Main()
        {
            Console.Title = NAME_VERSION;
            Console.Clear();

            Console.WriteLine("{0} by {1}\n", NAME_VERSION, AUTHOR);

            if (CheckForUpdate())
            {

            }

            ReadFile();
            VerifyCustomDir();

            int menu;
            do
            {
                string[] options = {
                    "Delete Surface Textures",
                    "Restore Surface Textures",
                    "Apply Custom GUI Textures",
                    "List Remaining Surface Textures",
                    "Update Version Directory\n",
                    "About"
                };
                string[] extras = {
                    string.Format("Current Selected Version: {0}", ROBLOX_VERSION_DIR_TYPE),
                    string.Format("Directory: {0}", ROBLOX_VERSION_DIR)
                };
                menu = GenerateMenu(NAME_VERSION+NEW_VERSION, options, extras, 0, 0, "\n");

                Console.Clear();

                switch (menu)
                {
                    case 1:
                        Remove();
                        break;
                    case 2:
                        Restore();
                        break;
                    case 3:
                        Replace();
                        break;
                    case 4:
                        List();
                        break;
                    case 5:
                        Update(true);
                        break;
                    case 6:
                        About();
                        break;
                    default:
                        break;
                }
            } while (menu != 0);
        }
    }
}