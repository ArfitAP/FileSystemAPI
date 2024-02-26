using FileSystemAPI.Application.Contracts.Persistence;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Services
{
    public class FileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _configuration;

        public FileService(IFileRepository fileRepository, IConfiguration configuration)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
        }

    }
}
