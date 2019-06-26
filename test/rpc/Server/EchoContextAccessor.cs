using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Rabbit.Rpc.Messages;
using RpcLib;
using RpcLib.Models;

namespace Server
{
    public class EchoContextAccessor : IContextAccessor
    {
        public EchoContextAccessor(IRpcContextAccessor rpcContextAccessor)
        {
            if (rpcContextAccessor.RpcContext == null)
            {
                return;
            }
            var jObject = JObject.FromObject(rpcContextAccessor.RpcContext);
            Context = jObject.ToObject<ServiceContext>();
        }

        public ServiceContext Context { get; private set; }
    }
}
