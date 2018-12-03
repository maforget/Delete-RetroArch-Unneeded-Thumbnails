using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Retroarch_Delete_Unneeded_Thumnails
{
    class Playlist
    {
        public string PlaylistPath { get; set; }
        public string PlaylistName { get; set; }
        public List<PlaylistEntry> PlaylistEntries { get; set; }
        public List<string> MissingFiles { get; set; }

        public Playlist(string path)
        {
            PlaylistPath = path;
            PlaylistName = Path.GetFileNameWithoutExtension(PlaylistPath);
            string[] RawPlaylist = File.ReadAllLines(PlaylistPath);
            PlaylistEntries = new List<PlaylistEntry>();

            for (int i = 0; i < RawPlaylist.Length; i+=6)
            {
                string[] RawPlaylistEntry = new string[6];
                Array.Copy(RawPlaylist, i, RawPlaylistEntry, 0, 6);

                PlaylistEntry entry = new PlaylistEntry(RawPlaylistEntry);
                PlaylistEntries.Add(entry);
            }
        }
    }
}
