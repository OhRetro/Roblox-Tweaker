using System;
using System.IO;
using System.Linq;
using System.Threading;
using static RobloxTweaker.MainVariables;
using static RobloxTweaker.OtherUtils;

namespace RobloxTweaker
{
    internal class TexturesManager
    {
        static readonly string[] TEXTURES_FILE_MIRRORS = {
            "https://github.com/OhRetro/asset-files/releases/download/roblox-tweaker/textures_backup.zip"
        };

        //Remove
        public static void Remove()
        {
            int choice;

            string title = "Do you want to remove all textures or leave some necessary textures?";
            string[] options = {
                "Leave Necessary Textures",
                "Remove All"
            };

            do
            {
                choice = GenerateMenu(title, options, Array.Empty<string>(), 0, 2);

                Console.Clear();
            } while (choice < 0 || choice > options.Length);

            Console.Clear();

            if (choice == 0)
            {
                return;
            }

            string[] dirs = Directory.GetDirectories(ROBLOX_TEXTURE_DIR);
            string[] files = Directory.GetFiles(ROBLOX_TEXTURE_DIR);

            string[] list_textures = Array.Empty<string>();
            for (int i = 0; i < dirs.Length; i++)
            {
                list_textures = list_textures.Append(dirs[i]).ToArray();
            }
            for (int i = 0; i < files.Length; i++)
            {
                list_textures = list_textures.Append(files[i]).ToArray();
            }

            if (choice == 1)
            {
                for (int i = 0; i < EXCEPTION_TEXTURES.Length; i++)
                {
                    if (list_textures.Contains(ROBLOX_TEXTURE_DIR + "\\" + EXCEPTION_TEXTURES[i]))
                    {
                        list_textures = list_textures.Where(x => x != ROBLOX_TEXTURE_DIR + "\\" + EXCEPTION_TEXTURES[i]).ToArray();
                    }
                }
            }

            Console.WriteLine("[Removing Textures]\n");
            for (int i = 0; i < list_textures.Length; i++)
            {
                if (Directory.Exists(list_textures[i]))
                {
                    Directory.Delete(list_textures[i], true);
                }
                else if (File.Exists(list_textures[i]))
                {
                    File.Delete(list_textures[i]);
                }
                Console.WriteLine("Removed Texture: {0}", list_textures[i].Split('\\').Last());
            }
            Continue(true);
        }

        //List
        public static void List()
        {
            string[] dirs = Directory.GetDirectories(ROBLOX_TEXTURE_DIR);
            string[] files = Directory.GetFiles(ROBLOX_TEXTURE_DIR);

            string[] messages = { string.Format("Textures: {0}\n", Count(ROBLOX_TEXTURE_DIR)) };

            for (int i = 0; i < dirs.Length; i++)
            {
                messages = messages.Append(dirs[i].Split('\\').Last()).ToArray();
            }
            for (int i = 0; i < files.Length; i++)
            {
                messages = messages.Append(files[i].Split('\\').Last()).ToArray();
            }

            _ = GenerateMenu("[Textures List]", Array.Empty<string>(), messages, 1);
        }

        //Restore
        public static void Restore()
        {
            Console.WriteLine("[Restoring Textures]");

            if (!File.Exists(BACKUP_TEXTURE_FILE) || FileSize(BACKUP_TEXTURE_FILE) < 27920097)
            {
                bool downloadSuccess = DownloadBackupTextures();

                Thread.Sleep(1000);
                if (!downloadSuccess)
                {
                    return;
                }
            }

            try
            {
                Unzip(BACKUP_TEXTURE_FILE, ROBLOX_TEXTURE_DIR);
                Console.WriteLine("[Done]");
            }
            catch (Exception e)
            {
                Console.WriteLine("[Restore Failed]");
                Console.WriteLine("Error:\n{0}", e.Message);
            }

            Continue(true);
        }

        //Download Backup Textures
        public static bool DownloadBackupTextures()
        {
            Console.WriteLine("[Downloading Backup Textures]");

            int fails = 0;
            for (int i = 0; i < TEXTURES_FILE_MIRRORS.Length; i++)
            {
                try
                {
                    DownloadFile(TEXTURES_FILE_MIRRORS[i], BACKUP_TEXTURE_FILE);
                }
                catch (FileLoadException)
                {
                    fails++;
                    continue;
                }
                break;
            }
            if (fails == TEXTURES_FILE_MIRRORS.Length)
            {
                Console.WriteLine("[Download Failed]");
                return false;
            }
            else
            {
                Console.WriteLine("[Download Success]");
                return true;
            }
        }

        //Count
        public static int Count(string directory)
        {
            string[] dirs = Directory.GetDirectories(directory);
            string[] files = Directory.GetFiles(directory);
            int count = 0;
            for (int i = 0; i < dirs.Length; i++)
            {
                count++;
            }
            for (int i = 0; i < files.Length; i++)
            {
                count++;
            }
            return count;
        }
    }
}
