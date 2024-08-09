using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.ViewModels.Blog
{
    public class BlogListPostViewModel
    {
        public string Name { get; set; }

        public string URLSegment { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime Changed { get; set; }

        public DateTime Publish { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public bool IsPublished => this.Publish < DateTime.Now;

        // implement?
        //public string Hash
        //{
        //    get
        //    {
        //        using (MD5 md5Hash = MD5.Create())
        //        {
        //            string values = string.Format("{0},{1},{2}", ID, URLSegment, Changed);
        //            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(values));

        //            StringBuilder sBuilder = new StringBuilder();
        //            for (int i = 0; i < data.Length; i++)
        //                sBuilder.Append(data[i].ToString("x2"));

        //            return sBuilder.ToString();
        //        }
        //    }
        //}
    }
}
