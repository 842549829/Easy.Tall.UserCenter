using System;
using System.Collections.Generic;
using System.Text;
using RpcLib.Models;

namespace RpcLib
{
    public interface IContextAccessor
    {
        ServiceContext Context { get; }
    }
}
