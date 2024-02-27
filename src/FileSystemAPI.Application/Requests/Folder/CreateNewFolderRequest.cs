using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Requests.Folder
{
    public class CreateNewFolderRequest
    {
        public string FolderName { get; set; } = string.Empty;
        public long ParentFolderID { get; set; }
    }
}
