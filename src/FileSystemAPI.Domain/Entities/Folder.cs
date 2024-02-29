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
        public long Id { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public long? ParentFolderId { get; set; }
        /// <summary>
        /// Size of all files in this Folder and all sub folders
        /// </summary>
        // Size could be obtained from file system's tree structure
        // But because file system is large-scaled, it is more optimal to update Size on single file creation/deletion and store it here
        // than calculating it each time.
        // Assuming that changing structure (creating/deleting of files) is less often than browsing.
        public long Size { get; set; }
        /// <summary>
        /// Full absolute path of the folder in file system
        /// </summary>
        // Full path could be obtained from file system's tree structure
        // But because file system is large-scaled, it is more optimal to write FullPath on folder creation/renaming and store it
        // than obtaining it by walking all the way to root of file system each time.
        // Assuming that changing structure (creating/renaming/deleting of files/folders) is less often than browsing.
        public string FullPath { get; set; } = string.Empty;
        public bool Active { get; set; }

        public virtual List<File> Files { get; set; } = new();
        public virtual Folder? ParentFolder { get; set; } = null;
        public virtual List<Folder> SubFolders { get; set; } = new();
    }
}
