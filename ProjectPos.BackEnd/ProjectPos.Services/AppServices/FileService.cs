using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProjectPos.Data.DbContexts;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices
{
    public class FileService : IFileService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FileService(
            ProjectPosDbContext context, 
            IWebHostEnvironment environment
            )
        {
            _context = context;
            _environment = environment;
        }

        public ServiceResponse<IFormFile> GetFile(string fileName)
        {
            try
            {
                var contentPath = _environment.ContentRootPath;
                var path = Path.Combine(contentPath
                    , "Uploads");

                var fileWithPath = Path.Combine(path, fileName);

                var file = File.Open(fileWithPath, FileMode.Open);
                
                return new ServiceResponse<IFormFile>
                {
                    Data = (IFormFile)file,
                    Message = "file uploaded successifuly",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IFormFile>
                {
                    Message = "Error Has Occured" + ex.Message,
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
        }

        public ServiceResponse<string> SaveFile(IFormFile file)
        {
            try
            {
                var contentPath = _environment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var ext = Path.GetExtension(file.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".pdf", ".webp" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));

                    return new ServiceResponse<string>
                    {
                        Message = msg,
                        Time = DateTime.Now,
                        IsSuccess = false
                    };
                }
                else
                {
                    string uniqueString = Guid.NewGuid().ToString();

                    var newFileName = uniqueString + ext;
                    var fileWithPath = Path.Combine(path, newFileName);
                    var stream = new FileStream(fileWithPath, FileMode.Create);
                    file.CopyTo(stream);
                    stream.Close();
                    return new ServiceResponse<string>
                    {
                        Data = newFileName,
                        Message = "file uploaded successifuly",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };

                }

            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>
                {
                    Message = "Error Has Occured" + ex.Message,
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
        }
    }
}
