using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _service;
        public FileController(IFileService service)
        {
            _service = service;
        }

        [HttpPost("api/saveFile")]
        public ActionResult SaveFile([FromForm] FileUploadModel model)
        {
            var _file = _service.SaveFile(model.File!);
            return Ok(_file);
        }

        [HttpGet("api/getFile")]
        public ActionResult GetFile([FromQuery]string file)
        {
            var _file = _service.GetFile(file);
            return Ok(_file);
        }
    }

    public class FileUploadModel
    {
        public IFormFile? File { get; set; }
    }
}
