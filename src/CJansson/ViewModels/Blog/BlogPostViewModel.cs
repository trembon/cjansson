using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.ViewModels.Blog
{
    public class BlogPostViewModel : BlogListPostViewModel
    {
        public string SplashImage { get; set; }

        public string MainBody { get; set; }
    }
}
