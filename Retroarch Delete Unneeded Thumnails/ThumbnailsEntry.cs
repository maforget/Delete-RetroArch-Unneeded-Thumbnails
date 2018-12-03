using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Retroarch_Delete_Unneeded_Thumnails
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
            RomName = Regex.Replace(romName, @"[&*:`<>?|\/\\]", "_");//Change Illegal Char to _

            BoxartPath = Path.Combine(basePath, playlist, "Named_Boxarts", RomName) + ".png";
            SnapPath = Path.Combine(basePath, playlist, "Named_Snaps", RomName) + ".png";
            TitlePath = Path.Combine(basePath, playlist, "Named_Titles", RomName) + ".png";

            VerifyFilesExists(BoxartPath, SnapPath, TitlePath);
        }

        public bool VerifyFilesExists(string boxart, string snap, string title)
        {
            FilesExists = false;
            MissingFiles = new List<string>();

            if (CheckMissingFiles(boxart) | CheckMissingFiles(snap) | CheckMissingFiles(title))
            {
                FilesExists = true;
            }

            return FilesExists;
        }

        public bool CheckMissingFiles(string path)
        {
            FilesExists = false;

            if (File.Exists(path))
            {
                FilesExists = true;
            }
            else
            {
                MissingFiles.Add(path);
            }

            return FilesExists;
        }

    }
}
