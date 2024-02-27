using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Contracts.Infrastructure
{
    public interface IFileStorageHandler
    {
        public Task<long> WriteFileToStorage(string bytesBase64, string storageFileName);

        public Task<byte[]> ReadFileFromStorage(string storageFileName);
    }
}
