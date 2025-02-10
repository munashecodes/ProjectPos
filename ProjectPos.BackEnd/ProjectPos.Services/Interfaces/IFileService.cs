using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IFileService
    {
        public ServiceResponse<string> SaveFile(IFormFile file);
        public ServiceResponse<IFormFile> GetFile(string fileName);
    }
}
