using System;
using System.Collections.Generic;
using System.Text;

namespace RpcLibService
{
    public class RemoteServiceException : Exception
    {
        public RemoteServiceException(int code, string message) : base(message)
        {
            HResult = code;
        }
    }
}
