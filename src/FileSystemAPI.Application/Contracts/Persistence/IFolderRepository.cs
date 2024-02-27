using FileSystemAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Contracts.Persistence
{
    public interface IFolderRepository : IAsyncRepository<Folder>
    {
        public Task<bool> FolderExists(long folderId);

        public Task<bool> FolderNameExistsInParent(string folderName, long parentFolderId);

        public Task DeleteFolder(long folderId);

        public Task<List<Folder>> GetFoldersInDirectory(long parentFolderId);
    }
}
