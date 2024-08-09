using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CJansson.Services;
using Microsoft.AspNetCore.Mvc;

namespace CJansson.Controllers
{
    public class FilesController : Controller
    {
        private readonly IBlogService blogService;

        public FilesController(IBlogService blogService)
        {
            this.blogService = blogService;
        }
        
        [HttpGet("files/{blogUrl}/image/{image}")]
        public IActionResult Image(string blogUrl, string image)
        {
            byte[] fileData = blogService.GetImage(blogUrl, image);
            if (fileData == null)
                return NotFound();

            string mime = "image/jpeg";
            if (image.EndsWith(".png"))
                mime = "image/png";

            FileContentResult fileResult = new FileContentResult(fileData, mime);
            fileResult.FileDownloadName = image;

            return fileResult;
        }
        
        [HttpGet("files/{blogUrl}/code/{code}")]
        public IActionResult Code(string blogUrl, string code)
        {
            Tuple<string, string> codeFile = blogService.GetCodeFile(blogUrl, code);
            if (codeFile == null)
                return NotFound();

            byte[] codeBytes = Encoding.UTF8.GetBytes(codeFile.Item2);
            FileContentResult fileResult = new FileContentResult(codeBytes, "application/octet-stream");
            fileResult.FileDownloadName = codeFile.Item1;

            return fileResult;
        }
    }
}