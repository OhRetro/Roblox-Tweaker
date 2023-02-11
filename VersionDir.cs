using System;
using System.IO;
using System.Linq;
using System.Threading;
using static RobloxTweaker.MainVariables;
using static RobloxTweaker.OtherUtils;
using static RobloxTweaker.TexturesManager;

namespace RobloxTweaker
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
                ROBLOX_VERSION_DIR = File.ReadAllLines(SETTINGS_FILE)[0];
                Validate();
                ROBLOX_TEXTURES_DIR = ROBLOX_VERSION_DIR + PATH_TO_TEXTURES_DIR;
                ROBLOX_CONTENT_DIR = ROBLOX_VERSION_DIR + PATH_TO_CONTENT_DIR;
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
        public static bool Select(bool canCancel = false)
        {
            Console.Clear();

            int useCancel = -1;

            if (canCancel)
            {
                useCancel = 1;
            }

            string[] dirs;
            try
            {
                dirs = Directory.GetDirectories(ROBLOX_VERSIONS_DIR);
            }
            catch (DirectoryNotFoundException)
            {
                dirs = null;

                Console.WriteLine("Roblox isn't installed...");
                Thread.Sleep(3000);

                Environment.Exit(2);
            }

            string[] valid_dirs = Array.Empty<string>();

            for (int i = 0; i < dirs.Length; i++)
            {
                string version_folder = dirs[i].Split('\\').Last();
                if (version_folder == ROBLOX_VERSION_DIR.Split('\\').Last())
                {
                    continue;
                }
                else if (version_folder.StartsWith("version-") && version_folder.Length == 24)
                {
                    valid_dirs = valid_dirs.Append(dirs[i]).ToArray();
                }
            }

            if (valid_dirs.Length == 0)
            {
                Console.WriteLine("No Version was found, please try reinstalling Roblox...");
                Thread.Sleep(3000);

                Environment.Exit(2);
            }

            string title = "Select a Roblox version to use:";
            string[] options = Array.Empty<string>();

            for (int i = 0; i < valid_dirs.Length; i++)
            {
                try
                {
                    string option = string.Format(
                        "{0} | {1} | Surface Textures: {2} | {3}",
                        valid_dirs[i].Split('\\').Last(),
                        Type(valid_dirs[i]),
                        CountTextures(valid_dirs[i] + PATH_TO_TEXTURES_DIR),
                        Directory.GetCreationTime(valid_dirs[i])
                    );

                    options = options.Append(option).ToArray();
                } catch {
                    continue;
                }
            }

            string[] extras = Array.Empty<string>();

            if (File.Exists(SETTINGS_FILE))
            {
                extras = extras.Append(string.Format("Current Selected Version: {0}\nDirectory: {1}", ROBLOX_VERSION_DIR_TYPE, ROBLOX_VERSION_DIR)).ToArray();        
            }

            int choice = Menu(title, options, extras, 0, useCancel, "\n");

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
            string old_dir = ROBLOX_VERSION_DIR.Split('\\').Last();
            string old_type = ROBLOX_VERSION_DIR_TYPE;

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
                string.Format("Old: {0} | {1}", old_dir, old_type)
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
            string version_folder = ROBLOX_VERSION_DIR.Split('\\').Last();
            if (!ROBLOX_VERSION_DIR.StartsWith(ROBLOX_VERSIONS_DIR) || !version_folder.StartsWith("version-") || version_folder.Length != 24 || !Directory.Exists(ROBLOX_VERSION_DIR))
            {
                status = "Invalid/Outdated";
                valid = false;
            }
            else if (ROBLOX_VERSION_DIR.StartsWith(ROBLOX_VERSIONS_DIR) && version_folder.StartsWith("version-") && version_folder.Length == 24)
            {
                status = "Valid";
                valid = true;
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
