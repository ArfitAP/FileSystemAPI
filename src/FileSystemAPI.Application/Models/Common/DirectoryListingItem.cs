using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Models.Common
{
    /// <summary>
    /// Item that represent generic item in directory
    /// Used in List directory response 
    /// </summary>
    public class DirectoryListingItem
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Size { get; set; }
        public long FolderId { get; set; }
    }
}
