using static RobloxTweaker.MainFiles.MainVariables;
using static RobloxTweaker.MainFiles.OtherUtils;
using static RobloxTweaker.MainFiles.TexturesManager;

namespace RobloxTweaker.MainFiles
{
    internal class VersionDir
    {
        //Write File
        public static void WriteFile()
        {
            Console.WriteLine("[Writing File]");
            File.WriteAllText(ROBLOX_VERSION_DIR_FILE, ROBLOX_VERSION_DIR);
            Thread.Sleep(300);
        }

        //Read File
        public static void ReadFile()
        {
            if (File.Exists(ROBLOX_VERSION_DIR_FILE))
            {
                Console.WriteLine("[Reading File]");
                ROBLOX_VERSION_DIR = File.ReadAllText(ROBLOX_VERSION_DIR_FILE);
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
        public static void Select()
        {
            Console.Clear();
            string[] dirs = Directory.GetDirectories(ROBLOX_VERSIONS_DIR);
            string[] valid_dirs = Array.Empty<string>();
            for (int i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Split('\\').Last().StartsWith("version-"))
                {
                    valid_dirs = valid_dirs.Append(dirs[i]).ToArray();
                }
            }
            int choice;
            do
            {
                Console.WriteLine("Select a Roblox version to use:");
                for (int i = 0; i < valid_dirs.Length; i++)
                {
                    Console.Write("[{0}] {1} | ", i, valid_dirs[i].Split('\\').Last());
                    Console.Write("{0} | ", Type(valid_dirs[i]));
                    Console.Write("Textures: {0} | ", Count(valid_dirs[i] + PATH_TO_TEXTURES_DIR));
                    Console.Write("{0}", Directory.GetCreationTime(valid_dirs[i]));
                    if (valid_dirs[i] == ROBLOX_VERSION_DIR)
                    {
                        Console.Write(" <- Current Selected\n");
                    }
                    else
                    {
                        Console.Write("\n");
                    }
                }

                Console.Write(">");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    choice = -1;
                }

                Console.Clear();

            } while (choice < 0 || choice >= valid_dirs.Length);
            ROBLOX_VERSION_DIR = valid_dirs[choice];
        }

        //Update
        public static void Update()
        {
            OLD_ROBLOX_VERSION_DIR = ROBLOX_VERSION_DIR;
            OLD_ROBLOX_VERSION_DIR_TYPE = ROBLOX_VERSION_DIR_TYPE;

            Select();
            WriteFile();
            ReadFile();
            Console.Clear();
            Console.WriteLine("[Directory Updated]");
            Console.WriteLine("New: {0} | {1}", ROBLOX_VERSION_DIR.Split('\\').Last(), ROBLOX_VERSION_DIR_TYPE);
            Console.WriteLine("Old: {0} | {1}", OLD_ROBLOX_VERSION_DIR.Split('\\').Last(), OLD_ROBLOX_VERSION_DIR_TYPE);

            Continue(true);
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
            if (!ROBLOX_VERSION_DIR.StartsWith(ROBLOX_VERSIONS_DIR) || !ROBLOX_VERSION_DIR.Split('\\').Last().StartsWith("version-"))
            {
                status = "Invalid";
                valid = false;
            }
            else if (!Directory.Exists(ROBLOX_VERSION_DIR))
            {
                status = "Outdated";
                valid = false;
            }
            else
            {
                status = "Valid";
                valid = true;
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
