using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Retroarch_Delete_Unneeded_Thumbnails
{
    [JsonObject]
    class PlaylistEntry
    {
        [JsonProperty("path")]
        public string RomPath { get; set; }
        [JsonProperty("label")]
        public string RomName { get; set; }
        [JsonProperty("core_path")]
        public string CorePath { get; set; }
        [JsonProperty("core_name")]
        public string CoreName { get; set; }
        [JsonProperty("crc32")]
        public string Checksum { get; set; }
        [JsonProperty("db_name")]
        public string PlaylistName { get; set; }
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

            AssociatedThumb = Thumbnail.NewThumbnailEntry(PlaylistName, RomName);
        }
    }
}
