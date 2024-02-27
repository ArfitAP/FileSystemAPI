using FileSystemAPI.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Infrastructure.Services
{
    public class FileStorageHandler : IFileStorageHandler
    {
        private readonly IConfiguration _configuration;
        public FileStorageHandler(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task<byte[]> ReadFileFromStorage(string storageFileName)
        {
            byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(_configuration["StoragePath"]!, storageFileName));

            return bytes;
        }

        public async Task<long> WriteFileToStorage(string bytesBase64, string storageFileName)
        {
            byte[] bytes = Convert.FromBase64String(bytesBase64);

            await File.WriteAllBytesAsync(Path.Combine(_configuration["StoragePath"]!, storageFileName), bytes);

            return bytes.Length;
        }
    }
}
