using FileSystemAPI.Application.Contracts.Persistence;
using FileSystemAPI.Domain.Entities;
using FileSystemAPI.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Persistence.Repositories
{
    public class FolderRepository : BaseRepository<Folder>, IFolderRepository
    {
        public FolderRepository(FileSystemDbContext dbContext) : base(dbContext)
        {
        }

        public async Task DeleteFolder(long folderId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                Folder? folder = await _dbContext.Folders.Where(f => f.Id == folderId).SingleOrDefaultAsync();

                if (folder is not null)
                {
                    var size = folder.Size;

                    folder.DeletedDate = DateTime.Now;
                    folder.Active = false;

                    await this.UpdateAsync(folder);

                    long? parentId = folder.ParentFolderId;
                    while (parentId != null)
                    {
                        var parent = await _dbContext.Folders.Where(f => f.Id == parentId).SingleOrDefaultAsync();
                        if (parent is not null)
                        {
                            parent.Size -= size;
                            await _dbContext.SaveChangesAsync();

                            parentId = parent.ParentFolderId;
                        }
                        else parentId = null;
                    }

                    await DeleteSubFolders(folderId);
                    await _dbContext.SaveChangesAsync();
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private async Task DeleteSubFolders(long folderId)
        {
            var files = await _dbContext.Files.Where(f => f.FolderId == folderId && f.Active == true).ToListAsync();
            foreach (var file in files) 
            {
                file.Active = false;
                file.DeletedDate = DateTime.Now;
            }

            var folders = await _dbContext.Folders.Where(f => f.ParentFolderId == folderId && f.Active == true).ToListAsync();
            foreach (var folder in folders)
            {
                folder.Active = false;
                folder.DeletedDate = DateTime.Now;

                await DeleteSubFolders(folder.Id);
            }
        }

        public async Task<bool> FolderNameExistsInParent(string folderName, long parentFolderId)
        {
            return await _dbContext.Folders
                                   .AsNoTracking()
                                   .Where(f => f.ParentFolderId == parentFolderId && f.FolderName == folderName && f.Active == true)
                                   .AnyAsync();
        }

        public async Task<bool> FolderExists(long folderId)
        {
            return await _dbContext.Folders
                                   .AsNoTracking()
                                   .Where(f => f.Id == folderId && f.Active == true)
                                   .AnyAsync();
        }

        public async Task<List<Folder>> GetFoldersInDirectory(long parentFolderId)
        {
            return await _dbContext.Folders
                                   .AsNoTracking()
                                   .Where(f => f.ParentFolderId == parentFolderId && f.Active == true)
                                   .ToListAsync();
        }
    }
}
