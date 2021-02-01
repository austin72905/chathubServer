using ChatHubServer.CacheData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatHubServer.Controllers
{
    public class MemberController :Controller
    {
        //當資料庫有改變時，訪問這個街口，去刷新緩存
        public IActionResult Index()
        {
            //MemberList.Memlist = MemberList.GetMemberlist();
            MemberList.Memlist = MemberList.GetMemberlist().Result;

            foreach(var item in MemberList.Memlist)
            {

            }
            return Json(MemberList.Memlist[0]);
        }
    }
}
