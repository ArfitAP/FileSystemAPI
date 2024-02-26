using FileSystemAPI.Application.Models.Common;
using FileSystemAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Models
{
    public class FolderModel : AuditableEntity, IFileSystemItem
    {
        public int Id { get; set; }
        public ItemType Type { get; set; } = ItemType.Folder;
        public string FolderName { get; set; } = string.Empty;
        public int? ParentFolderId { get; set; }
        public long Size { get; set; }
        public bool Active { get; set; }

        public List<IFileSystemItem> ChildItems = [];

    }
}
