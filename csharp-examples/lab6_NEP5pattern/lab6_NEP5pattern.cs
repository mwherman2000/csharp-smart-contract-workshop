using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

// NEP5 Token Proposal: https://github.com/neo-project/proposals/blob/master/nep-5.mediawiki

// Testing:
// operation,     args
// ---------,     -------------------------------------------------------------------------------
// "deploy"
//
// "totalSupply"
// "name"
// "symbol"
// "decimals"
//
// "balanceOf",  ["ATrzHaicmhRj15C3Vv6e6gLfLqhSD2PtTr"] // Owner Account
// "balanceOf",  ["AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y"] // Account #2
// "balanceOf",  ["AZ9Bmz6qmboZ4ry1z8p2KF3ftyA2ckJAym"] // Account #3
// "transfer",   ["ATrzHaicmhRj15C3Vv6e6gLfLqhSD2PtTr", "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y", 100]
// "transfer",   ["ATrzHaicmhRj15C3Vv6e6gLfLqhSD2PtTr", "AZ9Bmz6qmboZ4ry1z8p2KF3ftyA2ckJAym", 100]
// "transfer",   ["AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y", "AZ9Bmz6qmboZ4ry1z8p2KF3ftyA2ckJAym", 50]
// "transfer",   ["AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y", "AZ9Bmz6qmboZ4ry1z8p2KF3ftyA2ckJAym", 500]
// "transfer",   ["AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y", "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y", 50]

namespace lab6_NEP5pattern
{
    public class NEP5Base
    {
        public BigInteger TotalSupply;
        public string Name;
        public string Symbol;
        public byte Decimals;
        public byte[] OwnerAccountScriptHash;
    }

    public class lab6_NEP5pattern : SmartContract
    {
        private static readonly BigInteger _TotalSupply = 10000;
        private const string _Name = "My Test Token";
        private const string _Symbol = "MTT";
        private const byte _Decimals = 8;
        private static readonly byte[] _OwnerAccountScriptHash = "ATrzHaicmhRj15C3Vv6e6gLfLqhSD2PtTr".AsByteArray(); // .ToScriptHash() //  Demo

        public static event Action<byte[], byte[], BigInteger> transfer;

        public static object Main(string operation, params object[] args)
        {
            object result = false; // = 0 (zero)

            NEP5Base TOKENBASE = new NEP5Base { TotalSupply = _TotalSupply,
                                                Name = _Name,
                                                Symbol = _Symbol,
                                                Decimals = _Decimals,
                                                OwnerAccountScriptHash = _OwnerAccountScriptHash };

            if (operation == "totalSupply")
            {
                Runtime.Notify("totalSupply");
                result = TotalSupply();
            }
            else if (operation == "name")
            {
                Runtime.Notify("name");
                result = Name();
            }
            else if (operation == "symbol")
            {
                Runtime.Notify("symbol");
                result = Symbol();
            }
            else if (operation == "decimals")
            {
                Runtime.Notify("decimals");
                result = Decimals();
            }
            else if (operation == "balanceOf")
            {
                if (args.Length < 1)
                {
                    result = false;
                }
                else
                {
                    byte[] account = (byte[])args[0];
                    Runtime.Notify("balanceOf");
                    result = BalanceOf(account);
                }
            }
            else if (operation == "transfer")
            {
                if (args.Length < 3)
                {
                    result = false;
                }
                else
                {
                    byte[] from = (byte[])args[0];
                    byte[] to = (byte[])args[1];
                    BigInteger amount = (BigInteger)args[2];
                    Runtime.Notify("transfer", args[0], args[1], args[2]);
                    result = Transfer(from, to, amount);
                }
            }
            else if (operation == "deploy")
            {
                Runtime.Notify("deploy");
                result = Deploy(TOKENBASE);
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static BigInteger TotalSupply()
        {
            return _TotalSupply;
        }

        public static string Name()
        {
            return _Name;
        }

        public static string Symbol()
        {
            return _Symbol;
        }

        public static byte Decimals()
        {
            return _Decimals;
        }

        public static BigInteger BalanceOf(byte[] account)
        {
            BigInteger result = 0;

            StorageContext ctx = Storage.CurrentContext;
            BigInteger currentBalance = Storage.Get(ctx, account).AsBigInteger();

            result = currentBalance;

            return result;
        }

        public static bool Transfer(byte[] from, byte[] to, BigInteger amount)
        {
            bool result = false;

            if (amount > 0)
            {
                if (true) // Runtime.CheckWitness(from)) // is the account invoking this contract == "from" account
                {
                    StorageContext ctx = Storage.CurrentContext;

                    // Get balance from the "from" ledger
                    BigInteger fromBalance = Storage.Get(ctx, from).AsBigInteger();
                    if (fromBalance >= amount)
                    {
                        if (from == to)
                        {
                            result = true;
                        }
                        else // from != to
                        {
                            // Update "from" ledger
                            Storage.Put(ctx, from, fromBalance - amount);

                            // Update "to" ledger
                            BigInteger toBalance = Storage.Get(ctx, to).AsBigInteger();
                            Storage.Put(ctx, to, toBalance + amount);

                            // Log a "transfer" event
                            transfer(from, to, amount);

                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        private static bool Deploy(NEP5Base tokenBase)
        {
            bool result = false;

            if (true) // Runtime.CheckWitness(_OwnerAccountScriptHash))
            {
                StorageContext ctx = Storage.CurrentContext;

                // Create on-chain ledger entry for the owner of this token. Check to see if the ledger already exists
                byte[] currentBalance = Storage.Get(ctx, tokenBase.OwnerAccountScriptHash);
                if (currentBalance.Length == 0)
                {
                    Storage.Put(ctx, _OwnerAccountScriptHash, tokenBase.TotalSupply);
                    result = true;
                }
            }

            return result;
        }
    }
}


