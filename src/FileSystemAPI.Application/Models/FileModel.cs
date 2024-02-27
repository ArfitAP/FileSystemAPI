﻿using FileSystemAPI.Application.Models.Common;
using FileSystemAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Models
{
    public class FileModel : AuditableEntity, IFileSystemItem
    {
        public long Id { get; set; }
        public ItemType Type { get; set; } = ItemType.File;
        public string FileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public long FolderId { get; set; }
        public bool Active { get; set; }
    }
}
