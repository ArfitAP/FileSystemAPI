using FileSystemAPI.Application.Requests.File;
using FileSystemAPI.Application.Responses.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Contracts.Services
{
    public interface IFileService
    {
        public Task<CreateNewFileResponse> CreateNewFile(CreateNewFileRequest createNewFileRequest);

        public Task<RenameFileResponse> RenameFile(RenameFileRequest renameFileRequest);

        public Task<DeleteFileResponse> DeleteFile(DeleteFileRequest deleteFileRequest);

        public Task<GetFileResponse> GetFile(GetFileRequest getFileRequest);
    }
}
