using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retroarch_Delete_Unneeded_Thumnails
{
    class JsonRoot
    {
        public string version { get; set; }
        public List<PlaylistJson> items { get; set; }
    }

    class PlaylistJson
    {
        public string path { get; set; }
        public string label { get; set; }
        public string core_path { get; set; }
        public string core_name { get; set; }
        public string crc32 { get; set; }
        public string db_name { get; set; }
    }
}
