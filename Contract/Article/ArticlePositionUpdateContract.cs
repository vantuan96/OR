using ProtoBuf;

namespace BMS.Contract.Article
{
    [ProtoContract]
    public class ArticlePositionUpdateContract
    {

        [ProtoMember(1)]
        public int ArticleId { get; set; }

        [ProtoMember(3)]
        public int Sort { get; set; }

        
    }
}