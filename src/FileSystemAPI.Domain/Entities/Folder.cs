using FileSystemAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Domain.Entities
{
    public class Folder : AuditableEntity
    {
        public int Id { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public int? ParentFolderId { get; set; }
        public long Size { get; set; }
        public bool Active { get; set; }

        public virtual List<File> Files { get; set; } = new();
        public virtual Folder? ParentFolder { get; set; } = null;
        public virtual List<Folder> SubFolders { get; set; } = new();
    }
}
