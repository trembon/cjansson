using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.Models
{
    public class JsonBlogPost
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Publish { get; set; }

        public string Secret { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public JsonBlogPost()
        {
            this.Tags = new string[0];
        }
    }
}
