using Contract.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.Question
{
    public class ListQuestionGroupModel
    {
        public List<QuestionGroupContract> Groups { get; set; }

        public SelectList LayoutType { get; set; }

        public SelectList Status { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }

        public string SearchText { get; set; }
    }
}