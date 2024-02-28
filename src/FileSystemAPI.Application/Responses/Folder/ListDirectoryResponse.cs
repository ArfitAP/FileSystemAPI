using FileSystemAPI.Application.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Responses.Folder
{
    public class ListDirectoryResponse : BaseResponse
    {
        public ListDirectoryResponse() : base()
        {

        }

        public string FullPath { get; set; } = string.Empty;

        public IEnumerable<DirectoryListingItem> DirectoryItems { get; set; } = [];
    }
}
