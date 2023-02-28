using ProtoBuf;
using System;

namespace BMS.Contract.MediaFile
{
    [ProtoContract]
    public class MediaFileContract
    {
        [ProtoMember(1)]
        public int FileId { get; set; }

        [ProtoMember(2)]
        public string FileName { get; set; }

        [ProtoMember(3)]
        public string FileUrl { get; set; }

        [ProtoMember(4)]
        public int FileType { get; set; }

        [ProtoMember(5)]
        public int CreatedBy { get; set; }

        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(7)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(8)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(9)]
        public string FileDesc { get; set; }

        [ProtoMember(10)]
        public Int64 FileSize { get; set; }

        public string FileSizeStr
        {
            get {
                if (FileSize > 1024*1024)
                {
                    return Math.Round(((double)FileSize / 1024 / 1024), 1).ToString() + " Mb";
                }
                else if (FileSize > 1024)
                {
                    return Math.Round(((double)FileSize / 1024 ), 1).ToString() + " Kb";
                }
                else
                {
                    return FileSize.ToString() + " b";
                }
                
            }
        }
    }
}