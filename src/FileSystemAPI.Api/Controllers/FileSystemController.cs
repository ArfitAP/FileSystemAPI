using FileSystemAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileSystemAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileSystemController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly FolderService _folderService;

        public FileSystemController(FileService fileService, FolderService folderService) 
        {
            _fileService = fileService;
            _folderService = folderService;
        }

    }
}
