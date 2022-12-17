using System;
using System.IO;
using System.Linq;
using System.Threading;
using static RobloxTweaker.MainVariables;
using static RobloxTweaker.OtherUtils;
using static RobloxTweaker.FileUtils;

namespace RobloxTweaker
{
    internal class TexturesManager
    {
        static readonly string[] TEXTURES_FILE_MIRRORS = 
        {
            "https://github.com/OhRetro/asset-files/releases/download/roblox-tweaker/textures_backup.zip"
        };

        //Remove
        public static void Remove()
        {
            int choice;

            string title = "Do you want to remove all surface textures or leave the necessaries?";
            string[] options = 
            {
                "Leave Necessary Surface Textures [RECOMMENDED]",
                "Remove All Surface Textures"
            };

            do
            {
                choice = GenerateMenu(title, options, Array.Empty<string>(), 0, 2);

                Console.Clear();
            } while (choice < 0 || choice > options.Length);

            if (choice == 0)
            {
                return;
            }

            string[] dirs = Directory.GetDirectories(ROBLOX_TEXTURES_DIR);
            string[] files = Directory.GetFiles(ROBLOX_TEXTURES_DIR);

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
                    if (list_textures.Contains(ROBLOX_TEXTURES_DIR + "\\" + EXCEPTION_TEXTURES[i]))
                    {
                        list_textures = list_textures.Where(x => x != ROBLOX_TEXTURES_DIR + "\\" + EXCEPTION_TEXTURES[i]).ToArray();
                    }
                }
            }

            Console.WriteLine("[Removing Surface Textures]\n");
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
                Console.WriteLine("Removed Surface Texture: {0}", list_textures[i].Split('\\').Last());
            }
            Continue(true);
        }

        //List
        public static void List()
        {
            string[] dirs = Directory.GetDirectories(ROBLOX_TEXTURES_DIR);
            string[] files = Directory.GetFiles(ROBLOX_TEXTURES_DIR);

            string[] messages = { string.Format("Remaining Surface Textures: {0}\n", Count(ROBLOX_TEXTURES_DIR)) };

            for (int i = 0; i < dirs.Length; i++)
            {
                messages = messages.Append(dirs[i].Split('\\').Last()).ToArray();
            }
            for (int i = 0; i < files.Length; i++)
            {
                messages = messages.Append(files[i].Split('\\').Last()).ToArray();
            }

            _ = GenerateMenu("[Surface Textures Remaining List]", Array.Empty<string>(), messages, 1);
        }

        //Restore
        public static void Restore()
        {
            Console.WriteLine("[Restoring Surface Textures]");

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
                UnzipFile(BACKUP_TEXTURE_FILE, ROBLOX_TEXTURES_DIR);
                Console.WriteLine("[Done]");
            }
            catch (Exception e)
            {
                Console.WriteLine("[Restoration Failed]");
                Console.WriteLine("Error:\n{0}", e.Message);
            }
            Continue(true);
        }

        //Replace
        public static void Replace()
        {
            VerifyCustomDir();
            
            string[] customs_textures = Directory.GetDirectories(CUSTOM_TEXTURES_DIR);

            if (customs_textures.Length == 0)
            {
                Console.WriteLine("No Custom Texture Pack was found...");
                Thread.Sleep(2000);
                return;
            }

            string[] valid_customs_textures = Array.Empty<string>();

            for (int i = 0; i < customs_textures.Length; i++)
            {
                if (File.Exists(customs_textures[i] + CUSTOM_TEXTURES_TARGET_FILE))
                {
                    valid_customs_textures = valid_customs_textures.Append(customs_textures[i]).ToArray();
                }
            }

            string title = "Select a Custom Texture Pack to apply:";
            string[] options = Array.Empty<string>();
            for (int i = 0; i < valid_customs_textures.Length; i++)
            {
                string option = valid_customs_textures[i].Split('\\').Last();

                options = options.Append(option).ToArray();
            }

            int choice;
            do
            {
                choice = GenerateMenu(title, options, Array.Empty<string>(), 0, 1, "\n");
            } while (choice < 0 || choice > valid_customs_textures.Length);

            Console.Clear();

            if (choice == 0)
            {
                return;
            }

            string selected_custom_textures = valid_customs_textures[choice - 1];
            string target_file = selected_custom_textures + CUSTOM_TEXTURES_TARGET_FILE;
            string[] target_file_lines = File.ReadAllLines(target_file);

            Console.WriteLine("[Applying: {0}]\n", selected_custom_textures.Split('\\').Last());

            for (int i = 0; i < target_file_lines.Length; i++)
            {
                try
                {
                    string[] custom_textures = target_file_lines[i].Split(';');
                    File.Delete(ROBLOX_VERSION_DIR + PATH_TO_CONTENT_DIR + custom_textures[1]);
                    File.Copy(selected_custom_textures + custom_textures[0], ROBLOX_VERSION_DIR + PATH_TO_CONTENT_DIR + custom_textures[1]);
                    Console.WriteLine("Applied On: {0}", custom_textures[1].Split('\\').Last());
                }
                catch (Exception)
                {
                    Console.WriteLine("[ERROR] Invalid format on line {0}", i + 1);
                    continue;
                }
            }

            Continue(true);
        }

        public static void VerifyCustomDir()
        {
            if (!Directory.Exists(CUSTOM_TEXTURES_DIR))
            {
                Directory.CreateDirectory(CUSTOM_TEXTURES_DIR);
            }
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
