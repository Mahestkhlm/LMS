﻿using System;
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


        //public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IList<IFormFile> files,  string domain, string id)
        {
            long size = files.Sum(f => f.Length);
            var filePath="";
            string[] permittedExtensions = { ".txt", ".pdf", ".doc", ".docx", ".xls" };
            foreach (IFormFile source in files)
            {

                string uploadedFileName = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.ToString().Trim('"');
                string untrustedFileName = Path.GetFileName(uploadedFileName);
                var ext = Path.GetExtension(uploadedFileName).ToLowerInvariant();
                if (!string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext))
                {
                    var filename = untrustedFileName; // Path.GetTempFileName();
                    filename = this.EnsureCorrectFilename(filename);
                    filePath =  this.GetPathAndFilename(filename, domain);

                    //using (var stream = new FileStream(filePath, FileMode.Create)
                    using FileStream output = System.IO.File.Create(filePath);
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

        private string GetPathAndFilename(string filename, string domain)
        {
            //var path = Path.Combine(_config["StoredFilesPath"],
            //string path = this.hostingEnvironment.WebRootPath + "\\uploads\\";

            string path = Path.Combine(this.hostingEnvironment.WebRootPath, "uploads");
            path = Path.Combine(path, domain);
            if (domain=="users") path = Path.Combine(path, User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, filename);
        }
    }
}