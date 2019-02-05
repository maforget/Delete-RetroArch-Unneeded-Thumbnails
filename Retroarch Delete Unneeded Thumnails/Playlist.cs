using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

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
            PlaylistEntries = new List<PlaylistEntry>();

            //For New Json Playlists
            try
            {
                var doc = JsonConvert.DeserializeObject<JsonRoot>(File.ReadAllText(path));
                foreach (PlaylistJson RawPlaylistEntry in doc.items)
                {
                    PlaylistEntry entry = new PlaylistEntry(RawPlaylistEntry);
                    PlaylistEntries.Add(entry);
                }
            }
            catch (JsonReaderException)
            {
                //For Old playlists style
                string[] RawPlaylist = File.ReadAllLines(PlaylistPath);
                for (int i = 0; i < RawPlaylist.Length; i += 6)
                {
                    string[] RawPlaylistEntry = new string[6];
                    Array.Copy(RawPlaylist, i, RawPlaylistEntry, 0, 6);

                    PlaylistEntry entry = new PlaylistEntry(RawPlaylistEntry);
                    PlaylistEntries.Add(entry);
                }
            }
        }
    }
}
