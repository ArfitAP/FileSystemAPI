using FileSystemAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Responses.File
{
    public class CreateNewFileResponse : BaseResponse
    {
        public CreateNewFileResponse() : base()
        {

        }

        public FileModel File { get; set; } = default!;
    }
}
