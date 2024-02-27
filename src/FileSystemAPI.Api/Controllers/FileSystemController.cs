using FileSystemAPI.Application.Contracts.Services;
using FileSystemAPI.Application.Requests.File;
using FileSystemAPI.Application.Requests.Folder;
using FileSystemAPI.Application.Responses.File;
using FileSystemAPI.Application.Responses.Folder;
using Microsoft.AspNetCore.Mvc;

namespace FileSystemAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileSystemController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFolderService _folderService;

        public FileSystemController(IFileService fileService, IFolderService folderService) 
        {
            _fileService = fileService;
            _folderService = folderService;
        }


        [HttpPost("CreateNewFolder", Name = "CreateNewFolder")]
        public async Task<ActionResult<CreateNewFolderResponse>> CreateNewFolder([FromBody] CreateNewFolderRequest createNewFolderRequest)
        {
            var response = await _folderService.CreateNewFolder(createNewFolderRequest);

            if(response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("RenameFolder", Name = "RenameFolder")]
        public async Task<ActionResult<RenameFolderResponse>> RenameFolder([FromBody] RenameFolderRequest renameFolderRequest)
        {
            var response = await _folderService.RenameFolder(renameFolderRequest);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete("DeleteFolder", Name = "DeleteFolder")]
        public async Task<ActionResult<DeleteFolderResponse>> DeleteFolder([FromBody] DeleteFolderRequest deleteFolderRequest)
        {
            var response = await _folderService.DeleteFolder(deleteFolderRequest);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("ListDirectory/{folderId}", Name = "ListDirectory")]
        public async Task<ActionResult<ListDirectoryResponse>> ListDirectory([FromRoute] int folderId)
        {
            ListDirectoryRequest listDirectoryRequest = new() { FolderID = folderId };

            var response = await _folderService.ListDirectory(listDirectoryRequest);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpPost("CreateNewFile", Name = "CreateNewFile")]
        public async Task<ActionResult<CreateNewFileResponse>> CreateNewFile([FromBody] CreateNewFileRequest createNewFileRequest)
        {
            var response = await _fileService.CreateNewFile(createNewFileRequest);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("RenameFile", Name = "RenameFile")]
        public async Task<ActionResult<RenameFileResponse>> RenameFile([FromBody] RenameFileRequest renameFileRequest)
        {
            var response = await _fileService.RenameFile(renameFileRequest);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete("DeleteFile", Name = "DeleteFile")]
        public async Task<ActionResult<DeleteFileResponse>> DeleteFile([FromBody] DeleteFileRequest deleteFileRequest)
        {
            var response = await _fileService.DeleteFile(deleteFileRequest);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpGet("GetFile/{fileId}", Name = "GetFile")]
        public async Task<ActionResult<GetFileResponse>> GetFile([FromRoute] int fileId)
        {
            GetFileRequest getFileRequest = new() { FileID = fileId };

            var response = await _fileService.GetFile(getFileRequest);

            if (response.Success)
            {
                return File(response.Bytes, "application/octet-stream", response.FileName);
            }
            else
            {
                return BadRequest(response);
            }
        }

    }
}
