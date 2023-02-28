using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Question
{
    public class GetQuestionRequestContract: RequestContract
    {
        public int QuestionGroupId { get; set; }
    }
}
