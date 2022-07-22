using SharpCompress.Archives;
using SharpCompress.Common;

namespace RobloxTweaker.MainFiles
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

        //Unzip
        public static void Unzip(string sourceFile, string destDir)
        {
            using var archive = ArchiveFactory.Open(sourceFile);
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    Console.WriteLine("Extrating: {0}", entry.Key);
                    entry.WriteToDirectory(destDir, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                    ClearPreviousLine();
                }
            }
        }

        //Clear Previuos Line
        public static void ClearPreviousLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
