using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Responses.File
{
    public class GetFileResponse : BaseResponse
    {
        public GetFileResponse() : base()
        {

        }

        public byte[] Bytes { get; set; } = Array.Empty<byte>();

        public string FileName { get; set; } = string.Empty;
    }
}
