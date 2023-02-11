using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using static RobloxTweaker.MainVariables;
using static RobloxTweaker.OtherUtils;
using static RobloxTweaker.FileUtils;
using static RobloxTweaker.TexturesManager;
using static RobloxTweaker.VersionDir;

namespace RobloxTweaker
{
    internal class Program
    {
        private const string NAME = "Roblox Tweaker";
        private const string VERSION = "3.5";
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

        //Ask to Update
        private static void AskToUpdate(string newVersion, string newVersionPageURL, string newVersionSetupURL)
        {
            string title = string.Format("There's a New Version to Update!\nNew Version: v{0}\nCurrent Version: v{1}", newVersion, VERSION);

            string[] options = {
                    "Open New Version Page",
                    "Download New Version Setup & Run"
            };

            int choice = Menu(title, options, Array.Empty<string>(), 0, 1);

            if (choice == 1) {
                Process.Start(newVersionPageURL);
            }
            else if (choice == 2)
            {
                Console.WriteLine("Downloading...");
                DownloadFile(newVersionSetupURL, NEW_VERSION_SETUP_FILE);
                Process.Start(NEW_VERSION_SETUP_FILE);
                Environment.Exit(1);
            }

            NEW_VERSION = string.Format(" (New Version: v{0})", newVersion);
        }

        //Check for Update
        private static void CheckForUpdate(bool isManual=false)
        {
            if (File.Exists(NEW_VERSION_SETUP_FILE))
            {
                File.Delete(NEW_VERSION_SETUP_FILE);
            }

            var response = RequestURL(REPOSITORY.Replace("https://github.com", "https://api.github.com/repos") + "/releases/latest");
  
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("[CheckForUpdate]: Response from Github API Failed");
                Continue(true);
                return;
            }
            
            var json = response.Content.ReadAsStringAsync().Result;
            dynamic release = JsonConvert.DeserializeObject(json);
            string tag_name = release["tag_name"].ToString();
            string html_url = release["html_url"].ToString();
            string browser_download_url = release["assets"].ToObject<List<dynamic>>()[0]["browser_download_url"].ToString();

            var newVersion = new Version(tag_name);
            var currentVersion = new Version(VERSION);
            
            if (newVersion.CompareTo(currentVersion) > 0)
            {
                AskToUpdate(tag_name, html_url, browser_download_url);
                return;
            }

            if (isManual)
            {
                NEW_VERSION = string.Format(" (There's no New Version)");
            }
        }

        private static void Main()
        {
            Console.Title = NAME_VERSION;
            Console.Clear();

            Console.WriteLine("{0} by {1}\n", NAME_VERSION, AUTHOR);

            CheckForUpdate();

            ReadFile();
            VerifyCustomTexturesDir();

            string[] options = {
                "Delete Surface Textures",
                "Restore Surface Textures",
                "Apply Custom GUI Textures",
                "List Remaining Surface Textures",
                "Update Version Directory\n",
                "About",
                "Check for Update"
            };
            string[] extras = {
                string.Format("Current Selected Version: {0}", ROBLOX_VERSION_DIR_TYPE),
                string.Format("Directory: {0}", ROBLOX_VERSION_DIR)
            };

            int menu;
            do
            {
                menu = Menu(NAME_VERSION + NEW_VERSION, options, extras, 0, 0, "\n");

                switch (menu)
                {
                    case 1:
                        RemoveTextures();
                        break;
                    case 2:
                        RestoreTextures();
                        break;
                    case 3:
                        ReplaceTextures();
                        break;
                    case 4:
                        ListTextures();
                        break;
                    case 5:
                        Update(true);
                        break;
                    case 6:
                        About();
                        break;
                    case 7:
                        CheckForUpdate(true);
                        break;
                    default:
                        break;
                }
            } while (menu != 0);
        }
    }
}