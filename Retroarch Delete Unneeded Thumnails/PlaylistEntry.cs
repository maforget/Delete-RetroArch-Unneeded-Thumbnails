using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Retroarch_Delete_Unneeded_Thumbnails
{
    [JsonObject]
    class PlaylistEntry
    {
        public string RomPath { get; set; }
        public string RomName { get; set; }
        public string CorePath { get; set; }
        public string CoreName { get; set; }
        public string Checksum { get; set; }
        public string PlaylistName { get; set; }
        public string AbsoluteZipPath { get; set; }
        public ThumbnailsEntry AssociatedThumb { get; set; }

        //New Style for version 1.7.6+
        public PlaylistEntry(PlaylistJson PlaylistEntry)
        {
            RomPath = PlaylistEntry.path;
            RomName = PlaylistEntry.label;
            CorePath = PlaylistEntry.core_path;
            CoreName = PlaylistEntry.core_name;
            Checksum = PlaylistEntry.crc32;
            PlaylistName = PlaylistEntry.db_name.Substring(0, PlaylistEntry.db_name.Length - 4);//Remove .lpl at the end of the string

            AbsoluteZipPath = GetAbsoluteZipPath(RomPath);
            AssociatedThumb = Thumbnail.NewThumbnailEntry(PlaylistName, RomName);
        }

        //Old Style for version pre 1.7.6
        public PlaylistEntry(string[] rawPlaylistEntry)
        {
            if (rawPlaylistEntry.Length == 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    string line = rawPlaylistEntry[i];

                    switch (i)
                    {
                        case 0:
                            RomPath = line;
                            break;
                        case 1:
                            RomName = line;
                            break;
                        case 2:
                            CorePath = line;
                            break;
                        case 3:
                            CoreName = line;
                            break;
                        case 4:
                            Checksum = line;
                            break;
                        case 5:
                            PlaylistName = line.Substring(0, line.Length - 4);//Remove .lpl at the end of the string
                            break;

                        default:
                            break;
                    }
                }
            }

            AbsoluteZipPath = GetAbsoluteZipPath(RomPath);
            AssociatedThumb = Thumbnail.NewThumbnailEntry(PlaylistName, RomName);
        }


        private string GetAbsoluteZipPath(string romPath)
        {
            string zipPath = romPath.Split('#')[0];

            if (Directory.GetCurrentDirectory() != Paths.RetroarchBasePath && File.Exists(zipPath))
            {
                return zipPath;
            }
            else
            {
                string absPath = Path.Combine(Paths.RetroarchBasePath, zipPath);
                if (File.Exists(absPath))
                {
                    return absPath;
                }
            }

            //File Doesn't Exists
            return null;
        }
    }
}
