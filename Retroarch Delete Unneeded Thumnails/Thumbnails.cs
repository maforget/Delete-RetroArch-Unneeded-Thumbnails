using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Retroarch_Delete_Unneeded_Thumbnails
{
    class Thumbnail
    {
        public static List<string> AssociatedThumbs = new List<string>();
        public static List<string> MissingFiles = new List<string>();
        public static ThumbnailsEntry NewThumbnailEntry(string playlist, string romName)
        {
            ThumbnailsEntry thumb = new ThumbnailsEntry(playlist, romName);

            if (thumb.FilesExists == true)
            {
                AssociatedThumbs.Add(thumb.BoxartPath);
                AssociatedThumbs.Add(thumb.SnapPath);
                AssociatedThumbs.Add(thumb.TitlePath);
            }

            MissingFiles.AddRange(thumb.MissingFiles);
            return thumb;
        }

        public static List<string> GetUnassociatedImages(List<Playlist> playlists)
        {
            string basePath = Paths.ThumbnailsBasePath;
            List<string> UnassociatedImages = new List<string>();
            List<string> Allimages = new List<string>();

            foreach (var item in playlists.Select(x => x.PlaylistName))
            {
                string path = Path.Combine(basePath, item);
                Allimages.AddRange(Directory.EnumerateFiles(path, "*.png", SearchOption.AllDirectories).ToList());
            }

            foreach (var item in Allimages)
            {
                bool imageNeeded = AssociatedThumbs.Contains(item.ToLower(), StringComparer.OrdinalIgnoreCase);
                if (imageNeeded == false)
                {
                    UnassociatedImages.Add(item);
                }
            }

            return UnassociatedImages;
        }
    }
}
