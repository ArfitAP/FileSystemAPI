using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Contracts.Infrastructure
{
    public interface IFileStorageHandler
    {
        /// <summary>
        /// Writes file bytes to storage location defined in configuration
        /// </summary>
        /// <returns>Stored file size in bytes.</returns>
        public Task<long> WriteFileToStorage(byte[] bytes, string storageFileName);

        /// <summary>
        /// Reads requested file bytes from storage location defined in configuration
        /// </summary>
        /// <returns>File bytes.</returns>
        public Task<byte[]> ReadFileFromStorage(string storageFileName);
    }
}
