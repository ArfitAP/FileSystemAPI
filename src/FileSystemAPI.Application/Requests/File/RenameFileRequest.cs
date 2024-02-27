using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Requests.File
{
    public class RenameFileRequest
    {
        public long FileID { get; set; }
        public string NewFileName { get; set; } = string.Empty;
    }
}
