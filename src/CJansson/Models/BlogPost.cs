using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.Models
{
    public class BlogPost
    {
        public string FullPath { get; set; }


        public string Name { get; set; }

        public string URLSegment { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string SplashImage { get; set; }


        public DateTime Created { get; set; }

        public DateTime Changed { get; set; }

        public DateTime Publish { get; set; }

        public string Secret { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public Dictionary<string, byte[]> Images { get; set; }

        public Dictionary<string, Tuple<string, string>> Files { get; set; }


        public bool IsPublished => this.Publish < DateTime.Now && string.IsNullOrWhiteSpace(Secret);

        public BlogPost()
        {
            this.Tags = new string[0];
            this.Images = new Dictionary<string, byte[]>();
            this.Files = new Dictionary<string, Tuple<string, string>>();
        }
    }
}
