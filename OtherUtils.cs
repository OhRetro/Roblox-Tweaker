using System;
using System.IO;
using System.Net.Http;

namespace RobloxTweaker
{
    internal class OtherUtils
    {
        //Copy Folder
        public static void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        //Continue
        public static void Continue(bool clear = false)
        {
            Console.Write("\n[Press Any key to continue]");
            Console.ReadKey();
            if (clear)
            {
                Console.Clear();
            }
        }

        //Clear Previuos Line
        public static void ClearPreviousLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        //Menu
        public static int Menu(string title, string[] options, string[] extras, int inputType = 0, int optionZero = -1, string optionsZeroExtra = "")
        {
            int choice;
            int minChoiceNumber = 1;
            
            if (optionZero >= 0)
            {
                minChoiceNumber = 0;
            } 

            do
            {
                choice = GenerateMenu(title, options, extras, inputType, optionZero, optionsZeroExtra);
                Console.Clear();
            } while (choice < minChoiceNumber || choice > options.Length);
            return choice;
        }

        //Generate Menu
        public static int GenerateMenu(string title, string[] options, string[] extras, int inputType = 0, int optionZero = -1, string optionsZeroExtra = "")
        {
            int menu = -1;
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < options.Length; i++)
            {
                if (options.Length == 0)
                {
                    break;
                }
                Console.WriteLine("[{0}] {1}", i + 1, options[i]);
            }

            if (optionZero > -1)
            {
                string[] ZeroText = { "Exit", "Cancel", "Go Back" };
                Console.WriteLine("[0] {0} {1}", ZeroText[optionZero], optionsZeroExtra);
            }

            for (int i = 0; i < extras.Length; i++)
            {
                if (extras.Length == 0)
                {
                    break;
                }
                Console.WriteLine("{0}", extras[i]);
            }

            if (inputType == 0)
            {
                menu = MenuInput();
            }
            else
            {
                Continue(true);
            }

            return menu;
        }
        
        //Menu Input
        public static int MenuInput()
        {
            int menu;
            Console.Write(">");
            try
            {
                menu = Convert.ToInt16(Console.ReadLine());
            } catch (Exception)
            {
                menu = -1;
            }
            return menu;
        }

        //Request
        public static HttpResponseMessage RequestURL(string URL)
        {
            var HTTPClient = new HttpClient();
            HTTPClient.DefaultRequestHeaders.Add("User-Agent", "request");
            return HTTPClient.GetAsync(URL).Result;
        }
    }
}
