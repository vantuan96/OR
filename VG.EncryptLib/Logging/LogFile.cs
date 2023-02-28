using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VG.EncryptLib.Logging
{
    [ProtoContract]
    public class LogFile
    {
        /// <summary>
        /// File full name
        /// </summary>
        /// 
        [ProtoMember(1)]
        public string FileName { get; set; }

        /// <summary>
        /// 1: Folder; 2: File
        /// </summary>
        [ProtoMember(2)]
        public int FileType { get; set; }

        /// <summary>
        /// List file in current folder
        /// </summary>
        [ProtoMember(3)]
        public List<LogFile> Files { get; set; }
    }

    [ProtoContract]
    public class LogFileContent
    {
        [ProtoMember(1)]
        public string FileName { get; set; }

        [ProtoMember(2)]
        public string FileContent { get; set; }
    }
}
