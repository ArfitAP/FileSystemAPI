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

    }
}
