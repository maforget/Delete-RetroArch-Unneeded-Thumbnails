using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retroarch_Delete_Unneeded_Thumnails
{
    class PlaylistEntry
    {
        public string RomPath { get; set; }
        public string RomName { get; set; }
        public string CorePath { get; set; }
        public string CoreName { get; set; }
        public string Checksum { get; set; }
        public string PlaylistName { get; set; }
        public ThumbnailsEntry AssociatedThumb { get; set; }

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
