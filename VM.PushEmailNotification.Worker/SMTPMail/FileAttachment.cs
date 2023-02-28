using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OREmailNoti.SMTPMail
{
    [ProtoContract]
    public class FileAttachment
    {
        [ProtoMember(1)]
        private byte[] f;
        [ProtoMember(3)]
        public byte[] F
        {
            get { return f; }
            set { f = value; }
        }
        [ProtoMember(2)]
        private string fileName;
        [ProtoMember(4)]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public FileAttachment()
        {
        }
        [ProtoMember(5)]
        public String PathFile { get; set; }
    }
}
