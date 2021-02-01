using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatHubServer.Models
{
    public class ChatMemResp
    {
        public int code { get; set; }
        public List<MemberData> data { get; set; }
        public string msg { get; set; }
    }


    public class MemberData
    {
        public string username { get; set; }
        //哪個ID 的用戶傳的
        public int memberid { get; set; }

        public string gender { get; set; }

        public string nickname { get; set; }

    }
}
