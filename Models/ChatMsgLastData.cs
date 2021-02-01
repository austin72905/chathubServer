using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatHubServer.Models
{
    //要傳到message 組件的資訊
    //對所有對話紀錄的最後一筆對話
    public class ChatMsgLastData
    {
        public string username { get; set; }
        //哪個ID 的用戶傳的
        public int memberid { get; set; }

        public string gender { get; set; }

        public string text { get; set; }

        public string chatname { get; set; }

        public string chatid { get; set; }

        public int unreadcount { get; set; }
    }
}
