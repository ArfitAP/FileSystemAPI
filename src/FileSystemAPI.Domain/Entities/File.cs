using FileSystemAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Domain.Entities
{
    public class File : AuditableEntity
    {
        public long Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public long FolderId { get; set; }
        public bool Active { get; set; }

        public virtual Folder Folder { get; set; } = null!;

    }
}
