using ChatHubServer.CacheData;
using ChatHubServer.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatHubServer.ChatHub
{
    public class ChatHub : Hub
    {

        //連線list
        private static Dictionary<string, List<string>> _onnectList;
        public static Dictionary<string, List<string>> ConnectList
        {
            get
            {
                if (_onnectList == null)
                {
                    var newdic = new Dictionary<string, List<string>>();
                    _onnectList = newdic;
                }
                return _onnectList;
            }
            set
            {
            }
        }

        //主要是這個方法
        public async Task SendBothMsg(string userid, string recieveid, string input)
        {

            //獲取用戶資料
            var userdata = new MemberData();
            userdata = MemberList.Memlist.Where(i => i.memberid == Convert.ToInt32(userid)).FirstOrDefault();

            var recieverdata = new MemberData();
            recieverdata = MemberList.Memlist.Where(i => i.memberid == Convert.ToInt32(recieveid)).FirstOrDefault();


            var speakerData = new ChatResp 
            { 
                memberid = userdata.memberid,
                username = userdata.nickname,
                gender =userdata.gender
            };


            //要傳到message組件的訊息
            //也要分傳給接收者還是自己的
            //自己接收的性別 要是接收 者的 不然大頭貼會嘿嘿嘿
            var chatLastMsgData = new ChatMsgLastData
            {
                memberid = userdata.memberid,
                gender = recieverdata.gender,
                username = userdata.nickname,
                text = input,
                //是對哪個用戶的msg
                chatname = recieverdata.nickname,
                //傳這個是要讓點訊息時可以到該聊天室
                chatid = recieverdata.memberid.ToString(),
            };

            var chatLastMsgDataRec = new ChatMsgLastData
            {
                memberid = userdata.memberid,
                gender = userdata.gender,
                username = userdata.nickname,
                text = input,
                //是對哪個用戶的msg
                chatname = userdata.nickname,
                //傳這個是要讓點訊息時可以到該聊天室
                chatid = userdata.memberid.ToString(),
                unreadcount = 1
            };


            //取得要傳送的id 列表
            //member 的 connectionid list
            var useridList = ConnectList[userid];
            //reciever 的 connectionid list
            var recieveidList = new List<string>();
            if (ConnectList.ContainsKey(recieveid))
            {
                //reciever 的 connectionid list

                recieveidList = ConnectList[recieveid];
            }

            var resultList = useridList.Concat(recieveidList).ToList();
            //傳到聊天室的訊息
            await Clients.Clients(resultList).SendAsync("RecieveBothMsg", speakerData, input);

            //傳訊息到message 組件 (最後訊息)
            await Clients.Clients(useridList).SendAsync("SendLastMsg", chatLastMsgData); 
            await Clients.Clients(recieveidList).SendAsync("SendLastMsg", chatLastMsgDataRec);//result.forReciever

            //修改未讀總數
            //改變footer 未讀總數
            //讓接收者知道是誰傳的
            await Clients.Clients(recieveidList).SendAsync("ChangeTotal", userid, 1);

        }


        //讀訊息
        public async Task ReadMsg(string userid, string recieveid)
        {
            //member 的 connectionid list
            var useridList = ConnectList[userid];
            //改變footer 未讀總數
            await Clients.Clients(useridList).SendAsync("ChangeTotal", 0, 0);
        }


        //連線時將id 加入 連線list
        public async Task AddConnectList(string userid, string recieveid)
        {

            var recieveidList = new List<string>();
            if (!ConnectList.ContainsKey(userid))
            {
                ConnectList.Add(userid, new List<string>() { Context.ConnectionId });
            }
            else
            {
                ConnectList[userid].Add(Context.ConnectionId);
            }

            //如果他還沒上線過，就傳空的connectionID
            if (ConnectList.ContainsKey(recieveid))
            {
                //reciever 的 connectionid list

                recieveidList = ConnectList[recieveid];
            }

            //member 的 connectionid list
            var useridList = ConnectList[userid];

            var resultList = useridList.Concat(recieveidList).ToList();

            await Clients.Clients(resultList).SendAsync("IntoChat", "已進入聊天室" + "自己ID是: " + Context.ConnectionId);


        }


    }
}
