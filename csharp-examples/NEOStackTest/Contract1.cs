using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using Neo.SmartContract.Framework;
using System.Numerics;
using System;

namespace Neo.SmartContract
{
    public class Contract1 : Framework.SmartContract
    {
        public static object Main(string operation, params object[] args)
        {
            Runtime.Notify("operation", operation);
            Runtime.Notify("args", args);
            Runtime.Notify("args[0]", args[0]);
            Runtime.Notify("args[1]", args[1]);
            Runtime.Notify("args[2]", args[2]);
            return args;
        }
    }
}