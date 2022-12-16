using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Common;

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

        //Unzip
        public static void Unzip(string sourceFile, string destDir)
        {
            var archive = ArchiveFactory.Open(sourceFile);
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    Console.WriteLine("Extrating: {0}", entry.Key);
                    entry.WriteToDirectory(destDir, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                    ClearPreviousLine();
                }
            }
            archive.Dispose();
        }

        //Clear Previuos Line
        public static void ClearPreviousLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
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
                menu = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                menu = -1;
            }
            return menu;
        }

        //Request
        //TODO: finish it
        public static void RequestURL(string URL)
        {
            var HTTPClient = new HttpClient();
        } 

        //Download File
        public static void DownloadFile(string url, string destFile)
        {
            try
            {
                Task.Run(() => new Downloader().Download(url, destFile)).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("Download Fail at: {0}", url);
                Console.WriteLine("Error:\n{0}", e.Message);

                throw new FileLoadException();
            }
        }

        //File Size
        public static long FileSize(string file)
        {
            return new FileInfo(file).Length;
        }

        //File Downloader
        public class Downloader
        {
            public async Task Download(string url, string saveAs)
            {
                var httpClient = new HttpClient();
                var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                var parallelDownloadSuported = response.Headers.AcceptRanges.Contains("bytes");
                var contentLength = response.Content.Headers.ContentLength ?? 0;

                if (parallelDownloadSuported)
                {
                    const double numberOfParts = 5.0;
                    var tasks = new List<Task>();
                    var partSize = (long)Math.Ceiling(contentLength / numberOfParts);

                    File.Create(saveAs).Dispose();

                    for (var i = 0; i < numberOfParts; i++)
                    {
                        var start = i * partSize + Math.Min(1, i);
                        var end = Math.Min((i + 1) * partSize, contentLength);

                        tasks.Add(
                            Task.Run(() => DownloadPart(url, saveAs, start, end))
                            );
                    }

                    await Task.WhenAll(tasks);
                }
            }

            private async void DownloadPart(string url, string saveAs, long start, long end)
            {
                var httpClient = new HttpClient();
                var fileStream = new FileStream(saveAs, FileMode.Open, FileAccess.Write, FileShare.Write);
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                message.Headers.Add("Range", string.Format("bytes={0}-{1}", start, end));

                fileStream.Position = start;
                await httpClient.SendAsync(message).Result.Content.CopyToAsync(fileStream);
                fileStream.Close();
            }
        }
    }
}
