using System;
using System.Collections.Generic;
using System.Text;

namespace RpcLib.Models
{
    public class MyUserInfo : UserInfo<string>
    {
        public string MyName { get; set; }
    }
}
