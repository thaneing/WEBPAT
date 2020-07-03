using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Helpers
{
    public class OCROpenCV
    {
        public static string DownloadAndExtractLanguagePack()
        {
            //Source path to the zip file
            string langPackPath = "https://github.com/tesseract-ocr/tessdata/archive/3.04.00.zip";
            string zipFileName = AppDomain.CurrentDomain.BaseDirectory + "\\tessdata.zip";
            string tessDataFolder = AppDomain.CurrentDomain.BaseDirectory + "\\tessdata"; ;
            //Check and download the source file
            if (!File.Exists(zipFileName))
            {
                WebClient client = new WebClient();
                client.DownloadFile(langPackPath, zipFileName);
            }
            //Extract the zip to tessdata folder
            if (string.IsNullOrWhiteSpace(tessDataFolder))
            {
                ZipFile.ExtractToDirectory(zipFileName, AppDomain.CurrentDomain.BaseDirectory);
                var extractedDir = Directory.EnumerateDirectories(AppDomain.CurrentDomain.BaseDirectory).FirstOrDefault(x => (x.Contains("tessdata")));
                Directory.Move(extractedDir, tessDataFolder);
            }
            return tessDataFolder;
        }

       }
}
