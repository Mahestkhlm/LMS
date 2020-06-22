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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LMSLexicon20.Data;
using Microsoft.VisualBasic.CompilerServices;
using LMSLexicon20.Models;

namespace LMSLexicon20.Controllers
{
    public class UploadsController : Controller
    {
        private IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public UploadsController(IWebHostEnvironment hostingEnvironments, ApplicationDbContext context)
        {
            _env = hostingEnvironments;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult>PostDoc(IList<IFormFile> files=null,  string domain="", string id="")
        {
            long size = files.Sum(f => f.Length);
            var filePath="";
            string[] permittedExtensions = { ".txt", ".pdf", ".doc", ".docx", ".xls" };
            foreach (IFormFile source in files)
            {

                try
                {

                    string uploadedFileName = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.ToString().Trim('"');
                    string untrustedFileName = Path.GetFileName(uploadedFileName);
                    var ext = Path.GetExtension(uploadedFileName).ToLowerInvariant();
                    if (!string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext))
                    {
                        var filename = untrustedFileName; // Path.GetTempFileName();
                        filename = this.EnsureCorrectFilename(filename);
                        filePath = this.GetPathAndFilename(filename, domain, id);

                        //using (var stream = new FileStream(filePath, FileMode.Create)
                        using FileStream output = System.IO.File.Create(filePath);
                        await source.CopyToAsync(output);
                        filePath = Path.GetRelativePath(_env.ContentRootPath, filePath);
                        filePath=filePath.Replace("wwwroot\\", ""); //Directory.GetCurrentDirectory()  HttpRuntime.AppDomainAppPath



                        if (!await LinkDocToDomainAsync(filePath, domain, id))
                        {
                            TempData["FailText"] = $"Kunde inte koppla {domain} / {id} till filen {filePath}. Upladdning avbryts";
                            System.IO.File.Delete(filePath);
                        }
                    }
                     
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Kunde inte skapa {filePath}", ex);
                }

            }

            return Ok(new { count = files.Count, size= size });
            //return View();
        }

        private async Task<bool> LinkDocToDomainAsync(string filepath, string domain, string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            var DocumetId = _context.Documents.Where(d => d.Path == filepath).Select(d => d.Id).FirstOrDefault();
            //var DocumetId = _context.Set<Document>().Where(d => d.Path == filepath).Select(d => d.Id).FirstOrDefault();
 
            


            if (DocumetId > 0) return false; // same file
            var filename = Path.GetFileName(filepath);

            switch (domain)
            {
                case "course":
                    _context.Documents.Add(new Document { CourseId= int.Parse(id), Path= filepath, Name= filename,Date=DateTime.Now });
                    //_context.Set<Document>().Add(new Document { CourseId = int.Parse(id), Path = filepath, Name = filename, Date = DateTime.Now });
                    
                    break;
                case "module":
                    _context.Documents.Add(new Document { ModuleId = int.Parse(id), Path = filepath, Name = filename, Date = DateTime.Now });
                    break;
                case "activity":
                    _context.Documents.Add(new Document { ActivityId = int.Parse(id), Path = filepath, Name = filename, Date = DateTime.Now });
                    break;
                case "assignment":
                    var UserIdUploader = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _context.Documents.Add(new Document { UserId = UserIdUploader, ActivityId = int.Parse(id), Path = filepath, Name = filename, Date = DateTime.Now });
                    break;
                default:
                    return false;
                    //break;
            }
       
            await _context.SaveChangesAsync();
            return true;
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename, string domain, string id)
        {
            //var path = Path.Combine(_config["StoredFilesPath"],
            //string path = _env.WebRootPath + "\\uploads\\";

            string path = Path.Combine(_env.WebRootPath, "uploads");
            path = Path.Combine(path, domain);
            path = Path.Combine(path, id);
            if (domain == "assignment") path = Path.Combine(path, User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, filename);
        }
    }
}