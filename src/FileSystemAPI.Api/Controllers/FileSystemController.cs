using EasyCaching.Core;
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
        private readonly IEasyCachingProviderFactory _factory;
        private readonly IConfiguration _configuration;

        public FileSystemController(IFileService fileService, IFolderService folderService, IEasyCachingProviderFactory factory, IConfiguration configuration) 
        {
            _fileService = fileService;
            _folderService = folderService;
            _factory = factory;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a new Folder in file system.
        /// </summary>
        /// <param name="createNewFolderRequest">The Folder item to create.</param>
        /// <returns>The newly created Folder item.</returns>
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

        /// <summary>
        /// Renames a Folder.
        /// </summary>
        /// <param name="renameFolderRequest">The Folder item to rename.</param>
        /// <returns>The updated Folder item.</returns>
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

        /// <summary>
        /// Deletes a Folder by ID.
        /// </summary>
        /// <param name="deleteFolderRequest">The request containing ID of the Folder to delete.</param>
        /// <returns>Default API response with status.</returns>
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

        /// <summary>
        /// Lists all items in Folder.
        /// </summary>
        /// <param name="folderId">The ID of the Folder to list items from.</param>
        /// <returns>List of all items in Folder with item details.</returns>
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

        /// <summary>
        /// Creates a new File in file system.
        /// </summary>
        /// <param name="folderId">The Folder ID to create File in.</param>
        /// <param name="file">The File binary data.</param>
        /// <returns>The newly created File item.</returns>
        [HttpPost("CreateNewFile/{folderId}", Name = "CreateNewFile")]
        public async Task<ActionResult<CreateNewFileResponse>> CreateNewFile([FromRoute] int folderId, IFormFile file)
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                buffer = ms.ToArray();
            }

            CreateNewFileRequest createNewFileRequest = new();
            createNewFileRequest.FileName = file.FileName;
            createNewFileRequest.FolderID = folderId;
            createNewFileRequest.bytes = buffer;

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

        /// <summary>
        /// Renames a File.
        /// </summary>
        /// <param name="renameFileRequest">The File item to rename.</param>
        /// <returns>The updated File item.</returns>
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

        /// <summary>
        /// Deletes a File by ID.
        /// </summary>
        /// <param name="deleteFileRequest">The request containing ID of the File to delete.</param>
        /// <returns>Default API response with status.</returns>
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

        /// <summary>
        /// Get File binary data for downloading.
        /// </summary>
        /// <param name="fileId">The ID of the File to download.</param>
        /// <returns>File response.</returns>
        [HttpGet("GetFile/{fileId}", Name = "GetFile")]
        public async Task<IActionResult> GetFile([FromRoute] int fileId)
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

        /// <summary>
        /// Searches for a File in specific folder or across whole repository. 
        /// Caches results for specific time so next searches are faster 
        /// (useful for searching in search box while typing because of lot of searches in short period of time).
        /// </summary>
        /// <param name="searchFileRequest">Search data: string and folder to search in.</param>
        /// <returns>File search top 10 results.</returns>
        [HttpPost("SearchFile", Name = "SearchFile")]
        public async Task<ActionResult<SearchFileResponse>> SearchFile([FromBody] SearchFileRequest searchFileRequest)
        {
            var provider = _factory.GetCachingProvider("default");

            string cacheKey = searchFileRequest.ToString();

            if(provider.Exists(cacheKey))
            {
                var response = provider.Get<SearchFileResponse>(cacheKey).Value;

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                var response = await _fileService.SearchFile(searchFileRequest);

                provider.Set(cacheKey, response, TimeSpan.FromMinutes(int.Parse(_configuration["easycaching:cachingDurationMinutes"]!)));

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            
        }
    }
}
