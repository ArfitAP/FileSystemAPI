using FileSystemAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Responses.File
{
    public class SearchFileResponse : BaseResponse
    {
        public SearchFileResponse() : base()
        {
        }

        public List<FileModel> SearchResult { get; set; } = [];
    }
}
