using FileSystemAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Responses.Folder
{
    public class CreateNewFolderResponse : BaseResponse
    {
        public CreateNewFolderResponse() : base()
        {

        }

        public FolderModel Folder { get; set; } = default!;
    }
}
