using static RobloxTweaker.Workings.MainVariables;
using static RobloxTweaker.Workings.OtherUtils;
using static RobloxTweaker.Workings.TexturesManager;

namespace RobloxTweaker.Workings
{
    internal class VersionDir
    {
        //Write File
        public static void WriteFile()
        {
            Console.WriteLine("[Writing File]");
            File.WriteAllText(SETTINGS_FILE, ROBLOX_VERSION_DIR);
            Thread.Sleep(300);
        }

        //Read File
        public static void ReadFile()
        {
            if (File.Exists(SETTINGS_FILE))
            {
                Console.WriteLine("[Reading File]");
                ROBLOX_VERSION_DIR = File.ReadAllText(SETTINGS_FILE);
                Validate();
                ROBLOX_TEXTURE_DIR = ROBLOX_VERSION_DIR + PATH_TO_TEXTURES_DIR;
                ROBLOX_VERSION_DIR_TYPE = Type(ROBLOX_VERSION_DIR);
                Thread.Sleep(300);
            }
            else
            {
                Console.WriteLine("[File not found]");
                Thread.Sleep(1000);
                Select();
                WriteFile();
                ReadFile();
            }
        }

        //Select Directory
        public static bool Select(bool cancel = false)
        {
            Console.Clear();

            int useCancel = -1;
            int minOptionN = 1;

            if (cancel)
            {
                useCancel = 1;
                minOptionN = 0;
            }

            string[] dirs = Directory.GetDirectories(ROBLOX_VERSIONS_DIR);
            string[] valid_dirs = Array.Empty<string>();

            for (int i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Split('\\').Last().StartsWith("version-"))
                {
                    valid_dirs = valid_dirs.Append(dirs[i]).ToArray();
                }
            }

            string title = "Select a Roblox version to use:";
            string[] options = Array.Empty<string>();

            for (int i = 0; i < valid_dirs.Length; i++)
            {
                string option = string.Format(
                    "{0} | {1} | Textures: {2} | {3}",
                    valid_dirs[i].Split('\\').Last(),
                    Type(valid_dirs[i]),
                    Count(valid_dirs[i] + PATH_TO_TEXTURES_DIR),
                    Directory.GetCreationTime(valid_dirs[i])
                );

                if (valid_dirs[i] == ROBLOX_VERSION_DIR)
                {
                    option = string.Concat(option, " <- Current Selected");
                }
                options = options.Append(option).ToArray();
            }

            int choice;
            do
            {
                choice = GenerateMenu(title, options, Array.Empty<string>(), 0, useCancel);
            } while (choice < minOptionN || choice > valid_dirs.Length);

            Console.Clear();

            if (choice == 0)
            {
                return false;
            }

            ROBLOX_VERSION_DIR = valid_dirs[choice - 1];

            return true;
        }

        //Update
        public static void Update(bool canCancel = false)
        {
            OLD_ROBLOX_VERSION_DIR = ROBLOX_VERSION_DIR;
            OLD_ROBLOX_VERSION_DIR_TYPE = ROBLOX_VERSION_DIR_TYPE;

            bool selected = Select(canCancel);
            if (!selected)
            {
                return;
            }
            WriteFile();
            ReadFile();

            string[] extras =
            {
                string.Format("New: {0} | {1}", ROBLOX_VERSION_DIR.Split('\\').Last(), ROBLOX_VERSION_DIR_TYPE),
                string.Format("Old: {0} | {1}", OLD_ROBLOX_VERSION_DIR.Split('\\').Last(), OLD_ROBLOX_VERSION_DIR_TYPE)
            };

            _ = GenerateMenu("[Directory Updated]", Array.Empty<string>(), extras, 1);
        }

        //Get Type
        public static string Type(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Split('\\').Last() == "RobloxPlayerBeta.exe")
                {
                    return "Roblox Player";
                }
                else if (files[i].Split('\\').Last() == "RobloxStudioBeta.exe")
                {
                    return "Roblox Studio";
                }
            }
            return "Unknown";
        }

        //Validate Directory
        public static void Validate()
        {
            string status;
            bool valid;
            if (!ROBLOX_VERSION_DIR.StartsWith(ROBLOX_VERSIONS_DIR) || !ROBLOX_VERSION_DIR.Split('\\').Last().StartsWith("version-") || ROBLOX_VERSION_DIR.Split('\\').Last().Length != 24)
            {
                status = "Invalid";
                valid = false;
            }
            else if (ROBLOX_VERSION_DIR.StartsWith(ROBLOX_VERSIONS_DIR) || ROBLOX_VERSION_DIR.Split('\\').Last().StartsWith("version-"))
            {
                if (!Directory.Exists(ROBLOX_VERSION_DIR))
                {
                    status = "Outdated";
                    valid = false;
                }
                else
                {
                    status = "Valid";
                    valid = true;
                }
            }
            else
            {
                status = "Invalid?";
                valid = false;
            }

            Console.WriteLine("[Directory: {0}]", status);

            if (!valid)
            {
                Thread.Sleep(2000);
                Update();
            }
        }
    }
}
