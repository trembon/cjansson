using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.ViewModels.Blog
{
    public class BlogListViewModel
    {
        public IEnumerable<BlogListPostViewModel> Posts { get; set; }

        public BlogListViewModel()
        {
            this.Posts = new List<BlogListPostViewModel>();
        }
    }
}
