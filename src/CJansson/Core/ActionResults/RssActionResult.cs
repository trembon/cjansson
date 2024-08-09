using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CJansson.Core.ActionResults
{
    public class RssActionResult : ActionResult
    {
        private string name;
        private string description;
        private string url;

        private List<RssItem> rssItems;

        public string MimeType { get; set; }

        public RssActionResult(string name, string description = null, string url = null)
        {
            this.name = name;
            this.description = description;
            this.url = url;

            rssItems = new List<RssItem>();

            MimeType = "application/rss+xml";
        }

        public void AddItem(string title, string link, DateTime pubDate, string description)
        {
            this.rssItems.Add(new RssItem { Title = title, Link = link, PublishedDate = pubDate, Description = description });
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.ContentType = MimeType;

            // xml writer settings
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.Encoding = Encoding.UTF8;

            using (XmlWriter rssFeed = XmlWriter.Create(context.HttpContext.Response.Body, settings))
            {
                // write headers
                rssFeed.WriteStartElement("rss");
                rssFeed.WriteStartAttribute("version");
                rssFeed.WriteString("2.0");
                rssFeed.WriteEndAttribute();

                rssFeed.WriteAttributeString("xmlns", "atom", null, "http://www.w3.org/2005/Atom");

                rssFeed.WriteStartElement("channel");

                rssFeed.WriteStartElement("title");
                rssFeed.WriteString(this.name);
                rssFeed.WriteEndElement();

                string url = this.url;
                if (url.StartsWith("/"))
                    url = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{url}";

                rssFeed.WriteStartElement("link");
                rssFeed.WriteString(url);
                rssFeed.WriteEndElement();

                rssFeed.WriteStartElement("description");
                rssFeed.WriteCData(this.description);
                rssFeed.WriteEndElement();

                rssFeed.WriteStartElement("link", "http://www.w3.org/2005/Atom");
                rssFeed.WriteAttributeString("href", $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}");
                rssFeed.WriteAttributeString("rel", "self");
                rssFeed.WriteAttributeString("type", "application/rss+xml");
                rssFeed.WriteEndElement();

                // write all items
                foreach (var item in this.rssItems)
                {
                    string itemUrl = item.Link;
                    if (itemUrl.StartsWith("/"))
                        itemUrl = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{itemUrl}";

                    // create item
                    rssFeed.WriteStartElement("item");

                    // add elements to the item
                    rssFeed.WriteStartElement("title");
                    rssFeed.WriteString(item.Title);
                    rssFeed.WriteEndElement();

                    rssFeed.WriteStartElement("link");
                    rssFeed.WriteString(itemUrl);
                    rssFeed.WriteEndElement();

                    rssFeed.WriteStartElement("guid");
                    rssFeed.WriteString(itemUrl);
                    rssFeed.WriteEndElement();

                    rssFeed.WriteStartElement("pubDate");
                    rssFeed.WriteString(item.PublishedDate.ToUniversalTime().ToString("R"));
                    rssFeed.WriteEndElement();

                    rssFeed.WriteStartElement("description");
                    rssFeed.WriteCData(item.Description);
                    rssFeed.WriteEndElement();

                    rssFeed.WriteEndElement(); // item
                }

                // end headers
                rssFeed.WriteEndElement(); // channel
                rssFeed.WriteEndElement(); // rss
            }
        }

        private class RssItem
        {
            public string Title { get; set; }

            public string Link { get; set; }

            public DateTime PublishedDate { get; set; }

            public string Description { get; set; }
        }
    }
}
