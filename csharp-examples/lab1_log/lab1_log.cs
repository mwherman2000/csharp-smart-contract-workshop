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
            Runtime.Log("Hello world!");
        }
    }
}


