using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace lab1_log
{
    public class lab1_log : SmartContract
    {
        public static void Main()
        {
            int bar = 10;
            var foo = typeof(Int32);
            Runtime.Log("Hello world!");
        }
    }
}


