using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.User
{
    public class LoginResponseContract
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Display_Name { get; set; }

        public string Fullname { get; set; }
        
        public bool Sex { get; set; }

        public int Account_Type { get; set; }
                
        public string Cover_Img { get; set; }

        public int Cover_Top { get; set; }

        public string Cover_Path { get; set; }

        public int Avatar_Id { get; set; }

        public string Avatar_Img { get; set; }

        public string Avatar_Path { get; set; }

        public string Avatar_Coords { get; set; }

        public string AccessToken { get; set; }
    }
}
