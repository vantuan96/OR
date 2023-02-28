using Contract.MasterData;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Contract.CheckList
{
    [ProtoContract]
    public class CheckListContract
    {
        [ProtoMember(1)]
        public int CheckListId { get; set; }
        [ProtoMember(2)]
        public string CheckListName { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public bool Visible { get; set; }
        [ProtoMember(5)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(6)]
        public int CreatedBy { get; set; }
        [ProtoMember(7)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(8)]
        public int UpdatedBy { get; set; }
        [ProtoMember(9)]      
        public int Priority { get; set; }
        [ProtoMember(10)]
        public int CheckListTypeId { get; set; }
        [ProtoMember(11)]
        public string CheckListTypeName { get; set; }
        [ProtoMember(12)]
        public string StateName { get; set; }
        [ProtoMember(13)]
        public string CreateName { get; set; }
        [ProtoMember(14)]
        public Boolean IsSync { get; set; }
        [ProtoMember(15)]
        public DateTime DateSync { get; set; }
        [ProtoMember(16)]
        public string SystemName { get; set; }
        [ProtoMember(17)]
        public int SystemId { get; set; }
        [ProtoMember(18)]
        public List<int> lstItemIds{ get; set; }
        [ProtoMember(19)]
        public DateTime SetupDateFrom { get; set; }
        [ProtoMember(19)]
        public List<ItemContract> Items { get; set; }
    }
    [ProtoContract]
    public class CheckListItemContract
    {
        [ProtoMember(1)]
        public int CheckListDetailId { get; set; }
        [ProtoMember(2)]
        public string CheckListDetailName { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public int CheckListId { get; set; }
        [ProtoMember(5)]
        public bool Visible { get; set; }
        [ProtoMember(6)]
        public int Priority { get; set; }
        [ProtoMember(7)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(8)]
        public int CreatedBy { get; set; }
        [ProtoMember(9)]
        public DateTime UpdatedDate { get; set; }
        [ProtoMember(10)]
        public int UpdatedBy { get; set; }
        [ProtoMember(11)]
        public string CheckListName { get; set; }

    }

    public class UserContract
    {
        [ProtoMember(1)]
        public int UserId { get; set; }
        [ProtoMember(2)]
        public string UserName { get; set; }
        [ProtoMember(3)]
        public string FullName { get; set; }
        [ProtoMember(4)]
        public string Email { get; set; }
        [ProtoMember(5)]
        public string PhoneNumber { get; set; }
        [ProtoMember(6)]
        public Boolean IsOwner { get; set; }
    }


    public class SearchCheckList
    {
        [ProtoMember(1)]
        public List<CheckListContract> Data { get; set; }
        [ProtoMember(21)]
        public int TotalRows { get; set; }
    }

}
