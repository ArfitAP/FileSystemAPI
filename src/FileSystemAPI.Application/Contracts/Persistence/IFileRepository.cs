using FileSystemAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Contracts.Persistence
{
    public interface IFileRepository : IAsyncRepository<Domain.Entities.File>
    {
        public Task<bool> FileNameExistsInParent(string fileName, long parentFolderId);

        public Task<Domain.Entities.File?> GetFileById(long fileId);

        public Task<Domain.Entities.File> GetFileByNameAndParent(string fileName, long parentFolderId);

        public Task<Domain.Entities.File> AddFile(Domain.Entities.File file);

        public Task<Domain.Entities.File> UpdateFile(Domain.Entities.File file, long newSize);

        public Task DeleteFile(Domain.Entities.File file);

        public Task<List<Domain.Entities.File>> GetFilesInDirectory(long parentFolderId);

        public Task<List<Domain.Entities.File>> SearchFile(long startFolderId, string searchString);
    }
}
