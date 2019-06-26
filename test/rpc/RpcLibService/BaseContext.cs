using System;
using System.Collections.Generic;
using System.Text;
using RpcLib;
using RpcLib.Models;

namespace RpcLibService
{
    public abstract class BaseContext
    {
        protected readonly ServiceContext _context;

        protected BaseContext(IContextAccessor context)
        {
            _context = context.Context;
        }
    }
}
