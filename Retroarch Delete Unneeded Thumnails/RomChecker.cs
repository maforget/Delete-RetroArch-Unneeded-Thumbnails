using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Retroarch_Delete_Unneeded_Thumbnails
{
    internal static class RomChecker
    {
        public static List<string> CheckNotDetectedRoms(List<Playlist> playlists)
        {
            List<string> MissingRoms = new List<string>();

            foreach (Playlist playlist in playlists)
            {
                string RomDir = string.Empty;

                //Determine the highest path where the roms are placed
                foreach (PlaylistEntry file in playlist.PlaylistEntries)
                {
                    if (!string.IsNullOrEmpty(file.AbsoluteZipPath))
                    {
                        RomDir = GetParent(file.AbsoluteZipPath, RomDir);
                    }
                }

                //Enumerate all the files inside the path
                if (!string.IsNullOrEmpty(RomDir))
                {
                    string[] excluded = { ".srm", ".dsv", ".state", ".png", "\\user\\", "\\psp\\", "\\citra\\", ".mcr" };
                    var Roms = Directory.EnumerateFiles(RomDir, "*.*", SearchOption.AllDirectories).Where(i => !excluded.Any(x => i.ToLower().Contains(x)));

                    foreach (string romPath in Roms)
                    {
                        var pathInPlaylists = playlist.PlaylistEntries.Where(x => x.AbsoluteZipPath == romPath);

                        if (pathInPlaylists.Count() == 0)
                        {
                            //The Playlists Path was not found inside the RomFolder
                            MissingRoms.Add(romPath);
                        }
                    }
                }
            }

            return MissingRoms;
        }

        private static string GetParent(string ZipName, string previousRomDir)
        {
            var currentRomDir = new DirectoryInfo(Directory.GetParent(ZipName).FullName);

            if (string.IsNullOrEmpty(previousRomDir))
                return currentRomDir.FullName;

            string currentRomParent = currentRomDir.Parent.FullName;
            if (previousRomDir == currentRomParent)
            {
                //So we are now in a subdir so we know that the RomPath is the Highest and it shoulod be kept to the previous value (or return it's parent)
                return previousRomDir;
            }
            else
            {
                var previousParent = Directory.GetParent(previousRomDir);
                //If the previous path was a subdir we are no longer in one, return the currentDir
                //Else the currentDir is the same as the previsouDir we can return either one
                return currentRomDir.FullName;
            }
        }
    }
}