using FileSystemAPI.Application.Requests.Folder;
using FileSystemAPI.Application.Responses.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Contracts.Services
{
    public interface IFolderService
    {
        public Task<CreateNewFolderResponse> CreateNewFolder(CreateNewFolderRequest createNewFolderRequest);

        public Task<RenameFolderResponse> RenameFolder(RenameFolderRequest renameFolderRequest);

        public Task<DeleteFolderResponse> DeleteFolder(DeleteFolderRequest deleteFolderRequest);

        public Task<ListDirectoryResponse> ListDirectory(ListDirectoryRequest listDirectoryRequest);

    }
}
