using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Requests.File
{
    public class SearchFileRequest
    {
        public string SearchString { get; set; } = string.Empty;
        public long? FolderID { get; set; }
        public bool AllFolders { get; set; }

        public override string ToString()
        {
            if(AllFolders) 
            {
                return $"{SearchString.Trim().Replace(" ", "")}";
            }
            else
            {
                return $"{SearchString.Trim().Replace(" ", "")}_{FolderID!.Value}";
            }
        }
    }
}
