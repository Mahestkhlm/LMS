using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Linq;

namespace LMSLexicon20.Controllers
{
    public class UploaderController : Controller
    {
        private IWebHostEnvironment hostingEnvironment;

        public UploaderController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }


        //public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files, string activid)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IList<IFormFile> files, string activid)
        {
            long size = files.Sum(f => f.Length);
            var filePath="";
            string[] permittedExtensions = { ".txt", ".pdf" };
            foreach (IFormFile source in files)
            {

                string uploadedFileName = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.ToString().Trim('"');
                string untrustedFileName = Path.GetFileName(uploadedFileName);
                var ext = Path.GetExtension(uploadedFileName).ToLowerInvariant();
                if (!string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext))
                {
                    var filename = Path.GetTempFileName();
                    filePath = this.EnsureCorrectFilename(filename);

                    //using (var stream = new FileStream(filePath, FileMode.Create)
                    using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filePath)))
                        await source.CopyToAsync(output);
                }
            }


            return Ok(new { count = files.Count, size, filePath });
            //return this.View();
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {

            //var path = Path.Combine(_config["StoredFilesPath"],
            //    Path.GetRandomFileName());

            //string path = this.hostingEnvironment.WebRootPath + "\\uploads\\";
            string path = Path.Combine(this.hostingEnvironment.WebRootPath, "uploads");

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path + filename;
        }
    }
}