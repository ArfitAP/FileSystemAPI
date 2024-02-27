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
    public class FileRepository : BaseRepository<Domain.Entities.File>, IFileRepository
    {
        public FileRepository(FileSystemDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Domain.Entities.File> AddFile(Domain.Entities.File file)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                if (file is not null)
                {
                    var size = file.Size;

                    file.Active = true;

                    await this.AddAsync(file);

                    long? parentId = file.FolderId;
                    while (parentId != null)
                    {
                        var parent = await _dbContext.Folders.Where(f => f.Id == parentId).SingleOrDefaultAsync();
                        if (parent is not null)
                        {
                            parent.Size += size;
                            await _dbContext.SaveChangesAsync();

                            parentId = parent.ParentFolderId;
                        }
                        else parentId = null;
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return file!;
        }

        public async Task<Domain.Entities.File> UpdateFile(Domain.Entities.File file, long newSize)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                if (file is not null)
                {
                    var sizedifference = file.Size - newSize;

                    file.Size = newSize;

                    await this.UpdateAsync(file);

                    long? parentId = file.FolderId;
                    while (parentId != null)
                    {
                        var parent = await _dbContext.Folders.Where(f => f.Id == parentId).SingleOrDefaultAsync();
                        if (parent is not null)
                        {
                            parent.Size -= sizedifference;
                            await _dbContext.SaveChangesAsync();

                            parentId = parent.ParentFolderId;
                        }
                        else parentId = null;
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return file!;
        }

        public async Task DeleteFile(long fileId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                Domain.Entities.File? file = await _dbContext.Files.Where(f => f.Id == fileId).SingleOrDefaultAsync();

                if (file is not null)
                {
                    var size = file.Size;

                    file.DeletedDate = DateTime.Now;
                    file.Active = false;

                    await this.UpdateAsync(file);

                    long? parentId = file.FolderId;
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
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> FileNameExistsInParent(string fileName, long parentFolderId)
        {
            return await _dbContext.Files
                                   .AsNoTracking()
                                   .Where(f => f.FolderId == parentFolderId && f.FileName == fileName && f.Active == true)
                                   .AnyAsync();
        }

        public async Task<Domain.Entities.File> GetFileByNameAndParent(string fileName, long parentFolderId)
        {
            return await _dbContext.Files
                                   .Where(f => f.FolderId == parentFolderId && f.FileName == fileName && f.Active == true)
                                   .SingleAsync();
        }

        public async Task<List<Domain.Entities.File>> GetFilesInDirectory(long parentFolderId)
        {
            return await _dbContext.Files
                                   .AsNoTracking()
                                   .Where(f => f.FolderId == parentFolderId && f.Active == true)
                                   .ToListAsync();
        }
    }
}
