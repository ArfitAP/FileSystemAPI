using FileSystemAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Responses.File
{
    public class RenameFileResponse : BaseResponse
    {
        public RenameFileResponse() : base()
        {

        }

        public FileModel File { get; set; } = default!;
    }
}
