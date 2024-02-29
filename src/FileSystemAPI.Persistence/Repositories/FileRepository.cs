using FileSystemAPI.Application.Contracts.Persistence;
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

        /// <summary>
        /// Adds file to file system, writes it to database and updates size of all ancestor folders
        /// </summary>
        public async Task<Domain.Entities.File> AddFile(Domain.Entities.File file)
        {
            // Using transaction because update of size for all ancestor has to be done as single operation unit
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                if (file is not null)
                {
                    var size = file.Size;

                    file.Active = true;

                    await this.AddAsync(file);

                    // Climbing the folder ancestral hierarchy and updating size
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

        /// <summary>
        /// Updates file with same name in file system, writes it to database and updates size of all ancestor folders
        /// </summary>
        public async Task<Domain.Entities.File> UpdateFile(Domain.Entities.File file, long newSize)
        {
            // Using transaction because update of size for all ancestor has to be done as single operation unit
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                if (file is not null)
                {
                    var sizedifference = file.Size - newSize;

                    file.Size = newSize;

                    await this.UpdateAsync(file);

                    // Climbing the folder ancestral hierarchy and updating size
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

        /// <summary>
        /// Deletes file from file system, deactivate it in database and updates size of all ancestor folders
        /// </summary>
        public async Task DeleteFile(Domain.Entities.File file)
        {
            // Using transaction because update of size for all ancestor has to be done as single operation unit
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                if (file is not null)
                {
                    var size = file.Size;

                    file.DeletedDate = DateTime.Now;
                    file.Active = false;

                    await this.UpdateAsync(file);

                    // Climbing the folder ancestral hierarchy and updating size
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

        public async Task<Domain.Entities.File?> GetFileById(long FileId)
        {
            return await _dbContext.Files
                                   .Where(f => f.Id == FileId)
                                   .Include(f => f.Folder)
                                   .SingleOrDefaultAsync();
        }

        public async Task<Domain.Entities.File> GetFileByNameAndParent(string fileName, long parentFolderId)
        {
            return await _dbContext.Files
                                   .Where(f => f.FolderId == parentFolderId && f.FileName == fileName && f.Active == true)
                                   .Include(f => f.Folder)
                                   .SingleAsync();
        }

        public async Task<List<Domain.Entities.File>> GetFilesInDirectory(long parentFolderId)
        {
            return await _dbContext.Files
                                   .AsNoTracking()
                                   .Where(f => f.FolderId == parentFolderId && f.Active == true)
                                   .Include(f => f.Folder)
                                   .ToListAsync();
        }

        /// <summary>
        /// Searches for files in file system whose name starts with search string
        /// </summary>
        /// <param name="startFolderId">The ID of the Folder to start searching. Searching is continued in all subfolders.</param>
        public async Task<List<Domain.Entities.File>> SearchFile(long startFolderId, string searchString)
        {
            List<Domain.Entities.File> result = [];

            // BFS search algorithm
            LinkedList<long> folderQueue = [];

            folderQueue.AddLast(startFolderId);

            while (folderQueue.Any())
            {
                // Take first folder from queue
                var folderId = folderQueue.First();

                folderQueue.RemoveFirst();

                // Get all search results in current folder
                var files = await _dbContext.Files.Where(f => f.FolderId == folderId && f.Active == true && f.FileName.StartsWith(searchString))
                                                  .Include(f => f.Folder)
                                                  .ToListAsync();

                // We need 10 results in search, do not take more than that
                result.AddRange(files.Take(10 - result.Count));
                // If 10 results are found, stop BFS algorithm
                if (result.Count >= 10) break;

                // Add subfolders to queue
                var childFolders = await _dbContext.Folders.Where(f => f.ParentFolderId == folderId && f.Active == true)
                                                           .Select(f => f.Id)
                                                           .ToListAsync();

                foreach (var child in childFolders)
                {
                    folderQueue.AddLast(child);                  
                }
            }

            return result;
        }
    }
}
