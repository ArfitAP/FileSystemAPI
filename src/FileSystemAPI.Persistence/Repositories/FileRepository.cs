using FileSystemAPI.Application.Contracts.Persistence;
using FileSystemAPI.Persistence.Repositories.Common;
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
    }
}
