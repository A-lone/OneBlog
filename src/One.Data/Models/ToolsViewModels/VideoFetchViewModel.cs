using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Models.ToolsViewModels
{
    public class VideoFetchViewModel
    {
        public string Url { get; set; }

        public string Prompt { get; set; }

        public List<VideoUrl> VideoUrls { get; set; } = new List<VideoUrl>();
    }

    public class VideoUrl
    {
        public string File { get; set; }

        public string Size { get; set; }

        public string Seconds { get; set; }
    }
}
