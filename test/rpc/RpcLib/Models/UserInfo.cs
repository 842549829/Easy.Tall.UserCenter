using System;
using System.Collections.Generic;
using System.Text;

namespace RpcLib.Models
{
    public class UserInfo<T>
    {
        public int Id { get; set; }

        public T Data { get; set; }
    }
}
