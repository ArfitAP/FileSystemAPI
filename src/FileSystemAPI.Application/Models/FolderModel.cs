using FileSystemAPI.Application.Models.Common;
using FileSystemAPI.Domain.Common;
using FileSystemAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Models
{
    /// <summary>
    /// Composite pattern: Composite item 
    /// </summary>
    public class FolderModel : AuditableEntity, IFileSystemItem
    {
        public long Id { get; set; }

        public ItemType Type { get; set; } = ItemType.Folder;

        public string FolderName { get; set; } = string.Empty;

        public long? ParentFolderId { get; set; }

        /// <summary>
        /// Size of all files in this Folder and all sub folders
        /// </summary>
        // Since this is Composite item in Composite pattern, size could be calculated from all individual components.
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

        [JsonIgnore]
        public bool Active { get; set; }

        /// <summary>
        /// Composite pattern: Components
        /// Directory items
        /// </summary>
        public List<IFileSystemItem> ChildItems = [];

    }
}
