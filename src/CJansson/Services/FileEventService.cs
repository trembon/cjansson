using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CJansson.Services
{
    public interface IFileEventService
    {
        void Bind();
    }

    public class FileEventService : IFileEventService
    {
        private readonly IBlogService blogService;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger<FileEventService> logger;
        private CancellationTokenSource cancellationToken;
        private object triggerLock;

        public FileEventService(IBlogService blogService, IWebHostEnvironment hostingEnvironment, ILogger<FileEventService> logger)
        {
            this.blogService = blogService;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
            this.cancellationToken = new CancellationTokenSource();
            this.triggerLock = new object();
        }

        public void Bind()
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = Path.Combine(hostingEnvironment.ContentRootPath, "posts");
                watcher.IncludeSubdirectories = true;

                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                watcher.Filter = "*";

                // Add event handlers.
                watcher.Changed += Trigger;
                watcher.Created += Trigger;
                watcher.Deleted += Trigger;
                watcher.Renamed += Trigger;

                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to start file system watcher on posts folder");
            }
        }

        private void Trigger(object sender, EventArgs e)
        {
            lock (triggerLock)
            {
                if (cancellationToken != null)
                {
                    cancellationToken.Cancel();
                    cancellationToken = new CancellationTokenSource();
                }

                Task.Delay(1000, cancellationToken.Token).ContinueWith(t => blogService.Build());
            }
        }
    }
}