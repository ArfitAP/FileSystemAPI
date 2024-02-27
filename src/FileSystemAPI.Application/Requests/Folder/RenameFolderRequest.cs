using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Requests.Folder
{
    public class RenameFolderRequest
    {
        public long FolderID { get; set; }
        public string NewFolderName { get; set; } = string.Empty;
    }
}
