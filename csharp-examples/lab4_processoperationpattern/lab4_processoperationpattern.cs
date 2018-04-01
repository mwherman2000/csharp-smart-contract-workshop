using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace lab4_processoperationpattern
{
    public class lab4_processoperationpattern : SmartContract
    {
        public static object Main(string operation, params object[] args)
        {
            object result = false; // = 0 (zero)

            if (args.Length == 0)
            {
                Runtime.Log("Missing parameters");
                result = false;
            }
            else
            {
                if (operation == "query")
                {
                    Runtime.Notify("query", args[0]);
                    result = args[0];
                }
                else if (operation == "delete")
                {
                    Runtime.Notify("delete", args[0]);
                    result = true;
                }
                else if (operation == "register")
                {
                    Runtime.Notify("register", args[0]);
                    result = true;
                }
                else if (operation == "transfer")
                {
                    Runtime.Notify("transfer", args[0]);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}


