﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Requests.File
{
    public class CreateNewFileRequest
    {
        public string FileName { get; set; } = string.Empty;
        public long FolderID { get; set; }
        public byte[] bytes { get; set; } = Array.Empty<byte>();
    }
}
