using static RobloxTweaker.MainFiles.MainVariables;
using static RobloxTweaker.MainFiles.OtherUtils;

namespace RobloxTweaker.MainFiles
{
    internal class TexturesManager
    {
        //Remove
        public static void Remove()
        {
            int choice;
            do
            {
                Console.WriteLine("Do you want to remove all textures or leave some necessary textures?");
                Console.WriteLine("[1] Leave Necessary Textures \n[2] Remove All\n[0] Cancel");

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
            } while (choice < 0 || choice > 2);

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
            Console.WriteLine("[Textures List]");
            Console.WriteLine("Textures: {0}\n", Count(ROBLOX_TEXTURE_DIR));
            string[] dirs = Directory.GetDirectories(ROBLOX_TEXTURE_DIR);
            string[] files = Directory.GetFiles(ROBLOX_TEXTURE_DIR);
            string item;
            for (int i = 0; i < dirs.Length; i++)
            {
                item = dirs[i].Split('\\').Last();
                Console.WriteLine("{0}", item);
            }
            for (int i = 0; i < files.Length; i++)
            {
                item = files[i].Split('\\').Last();
                Console.WriteLine("{0}", item);
            }

            Continue(true);
        }

        //Restore
        public static void Restore()
        {
            Console.WriteLine("[Restoring Textures]");

            try
            {
                Unzip(BACKUP_TEXTURE_FILE, ROBLOX_TEXTURE_DIR);
                Console.WriteLine("[Done]");
            }
            catch (Exception e)
            {
                Console.WriteLine("[Restore Failed]");
                Console.WriteLine("[Error]\n{0}", e.Message);
            }

            Continue(true);
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
