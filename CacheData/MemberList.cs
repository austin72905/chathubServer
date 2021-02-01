using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using ChatHubServer.Models;

namespace ChatHubServer.CacheData
{
    public class MemberList
    {

        private static readonly HttpClient client = new HttpClient();

        //用戶資訊
        //去資料庫撈
        private static List<MemberData> _memlist { get; set; }

        public static List<MemberData> Memlist
        {
            get
            {
                if (_memlist == null)
                {
                    var memlist = GetMemberlist();
                    _memlist = memlist.Result;
                }

                return _memlist;
            }
            set { }
        }


        //向遠端發起get 請求
        public static async Task<List<MemberData>> GetMemberlist()
        {
            var responseString = await client.GetStringAsync("http://localhost:62001/chat");

            ChatMemResp resp = JsonSerializer.Deserialize<ChatMemResp>(responseString);
            
            return resp.data;
        }
    }
}
