﻿using FileSystemAPI.Application.Contracts.Persistence;
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

        /// <summary>
        /// Deletes folder from file system, deactivate it in database and updates size of all ancestor folders
        /// </summary>
        public async Task DeleteFolder(Folder folder)
        {
            // Using transaction because update of size for all ancestor has to be done as single operation unit
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                if (folder is not null)
                {
                    var size = folder.Size;

                    folder.DeletedDate = DateTime.Now;
                    folder.Active = false;

                    await this.UpdateAsync(folder);

                    // Climbing the folder ancestral hierarchy and updating size
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

                    // Recursively delete all subfolders so they cannot be accesed by ID, we need to delete each of them from database, not only parent
                    await DeleteSubFolders(folder.Id);
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

        /// <summary>
        /// DFS walk through all subfolders of selected folder and deleting them
        /// </summary>
        private async Task DeleteSubFolders(long folderId)
        {
            // Delete containing files
            var files = await _dbContext.Files.Where(f => f.FolderId == folderId && f.Active == true).ToListAsync();
            foreach (var file in files) 
            {
                file.Active = false;
                file.DeletedDate = DateTime.Now;
            }

            // Delete subfolders
            var folders = await _dbContext.Folders.Where(f => f.ParentFolderId == folderId && f.Active == true).ToListAsync();
            foreach (var folder in folders)
            {
                folder.Active = false;
                folder.DeletedDate = DateTime.Now;

                // Recursive call - DFS
                await DeleteSubFolders(folder.Id);
            }
        }


        /// <summary>
        /// Renames folder in file system, updates it in database and updates all the ancestor folders
        /// </summary>
        public async Task RenameFolder(Folder folder, string newFolderName)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                if (folder is not null)
                {
                    folder.FolderName = newFolderName;
                    var parent = await GetByIdAsync(folder.ParentFolderId!.Value);
                    folder.FullPath = Path.Combine(parent!.FullPath, newFolderName);

                    await _dbContext.SaveChangesAsync();

                    // Update full path of subfolders because it contains this fodlers old name
                    await RenameSubFolders(folder.Id, folder.FullPath);
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

        /// <summary>
        /// DFS walk through all subfolders of selected folder and updating their full path
        /// </summary>
        private async Task RenameSubFolders(long folderId, string folderFullPath)
        {
            var folders = await _dbContext.Folders.Where(f => f.ParentFolderId == folderId && f.Active == true).ToListAsync();
            foreach (var folder in folders)
            {
                folder.FullPath = Path.Combine(folderFullPath, folder.FolderName);

                // Recursive call - DFS
                await RenameSubFolders(folder.Id, folder.FullPath);
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
