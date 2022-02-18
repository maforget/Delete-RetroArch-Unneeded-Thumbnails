using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Retroarch_Delete_Unneeded_Thumbnails
{
    class ThumbnailsEntry
    {
        public string BoxartPath { get; set; }
        public string SnapPath { get; set; }
        public string TitlePath { get; set; }
        public string RomName { get; set; }
        public bool FilesExists { get; set; }
        public List<string> MissingFiles { get; set; }

        public ThumbnailsEntry(string playlist, string romName)
        {

            string basePath = Paths.ThumbnailsBasePath;
            MissingFiles = new List<string>();

            RomName = Regex.Replace(romName, @"[&*:`<>?|\/\\]", "_");//Change Illegal Char to _

            try
            {
                BoxartPath = Path.Combine(basePath, playlist, "Named_Boxarts", RomName) + ".png";
                SnapPath = Path.Combine(basePath, playlist, "Named_Snaps", RomName) + ".png";
                TitlePath = Path.Combine(basePath, playlist, "Named_Titles", RomName) + ".png";
            }
            catch (ArgumentException)
            {
                Console.Error.WriteLine($"Illegal Character in {playlist}\\{romName}");
            }

            VerifyFilesExists(BoxartPath, SnapPath, TitlePath);
        }

        public void VerifyFilesExists(string boxart, string snap, string title)
        {
            FilesExists = false;

            if (CheckMissingFiles(boxart) | CheckMissingFiles(snap) | CheckMissingFiles(title))
            {
                FilesExists = true;
            }
        }

        public bool CheckMissingFiles(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                MissingFiles.Add(path);
            }

            return false;
        }

    }
}
