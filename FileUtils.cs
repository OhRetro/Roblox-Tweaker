using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Common;
using static RobloxTweaker.OtherUtils;

namespace RobloxTweaker
{
    internal class FileUtils
    {
        //File Size
        public static long FileSize(string file)
        {
            return new FileInfo(file).Length;
        }
        
        //Unzip File
        public static void UnzipFile(string sourceFile, string destDir)
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

        //File Download
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

        //File Downloader
        private class Downloader
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

