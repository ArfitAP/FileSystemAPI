using FileSystemAPI.Application.Contracts.Persistence;
using FileSystemAPI.Domain.Entities;
using FileSystemAPI.Persistence.Repositories.Common;
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


    }
}
