using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contract.User;

namespace Admin.Models.User
{
    public class ListUserModel
    {
        public List<UserItemContract> ListUser { get; set; }

        public SelectList SelectMicrosite { get; set; }

        public SelectList SelectLocation { get; set; }

        public SelectList SelectRoles { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }

        public int RoleId { get; set; }

        public string SearchText { get; set; }
    }
}