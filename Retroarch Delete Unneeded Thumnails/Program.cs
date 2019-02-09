using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ByteSizeLib;

namespace Retroarch_Delete_Unneeded_Thumbnails
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(160, 40);
            string ArgsPath = args.Length >= 1 ? args[0] : string.Empty;

            if (string.IsNullOrEmpty(ArgsPath))
            {
                Console.WriteLine("No argument passed, You need to pass the RetroArch path as an argument");
                Exit();
            }

            Paths.RetroarchBasePath = ArgsPath;
            Paths.PlaylistPath = Path.Combine(ArgsPath, "playlists");
            Paths.ThumbnailsBasePath = Path.Combine(ArgsPath, "thumbnails");

            if (!Directory.Exists(Paths.PlaylistPath) && !Directory.Exists(Paths.ThumbnailsBasePath))
            {
                Console.WriteLine("The playlists and/or thumbnails folder weren't found in the path, is the path really RetroArch?");
                Exit();
            }

            Console.WriteLine("Using path: {0}", Paths.RetroarchBasePath);
            List<string> PlaylistsFiles = Directory.EnumerateFiles(Paths.PlaylistPath, "*.lpl").ToList();
            List<Playlist> Playlists = new List<Playlist>();

            foreach (string path in PlaylistsFiles)
            {
                Playlist list = new Playlist(path);
                Playlists.Add(list);
            }

            Console.WriteLine("Found {0} playlists", PlaylistsFiles.Count);
            Console.WriteLine("Checking all thumbnails");
            var UnneededImages = Thumbnail.GetUnassociatedImages(Playlists);
            int MissingFiles = Thumbnail.MissingFiles.Count;

            if (MissingFiles > 0)
            {
                Console.WriteLine("There are missing files, they are the following:");
                foreach (var item in Thumbnail.MissingFiles)
                {
                    Console.WriteLine("- {0}", GetFileName(item));
                }
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("No missing files found");
            }

            int UnneededFiles = UnneededImages.Count;
            if (UnneededFiles > 0)
            {
                int TotalFiles = Thumbnail.AssociatedThumbs.Count + UnneededFiles;
                Console.WriteLine("Found {0} unneeded files from a total of {1} files", UnneededFiles, TotalFiles);

                ByteSize UnneededSize = ByteSize.FromBytes(GetSize(UnneededImages));
                ByteSize TotalSize = ByteSize.FromBytes(GetSize(Thumbnail.AssociatedThumbs)) + UnneededSize;
                Console.WriteLine("{0} on a total of {1} can be liberated", UnneededSize.ToString(), TotalSize.ToString());

                Console.WriteLine("Delete these files? Type (Y/y) for Yes or anything else for No");
                string confirmDelete = Console.ReadLine();

                if (confirmDelete == "Y" | confirmDelete == "y")
                {
                    Console.WriteLine("");

                    foreach (var item in UnneededImages)
                    {
                        Console.WriteLine("Deleting {0}", GetFileName(item));
                        DeleteFile(item);
                    }
                    Console.WriteLine("");
                    Console.WriteLine("Finished deleting");
                }
                else
                {
                    Exit();
                }
            }
            else
            {
                Console.WriteLine("No unneeded files found");
            }

            Exit();
        }

        private static void Exit()
        {
            Console.WriteLine("");
            Console.WriteLine("Press Any key to exit");
            Console.ReadLine();
            Environment.Exit(0);
        }

        private static string GetFileName(string item)
        {
            return item.Replace(Paths.ThumbnailsBasePath + "\\", "");
        }

        public static void DeleteFile(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static long GetSize(List<string> files)
        {
            long Size = 0;
            foreach (var item in files)
            {
                FileInfo info = new FileInfo(item);
                try
                {
                    Size += info.Length;
                }
                catch (Exception)
                {

                }
            }

            return Size;
        }
    }
}
