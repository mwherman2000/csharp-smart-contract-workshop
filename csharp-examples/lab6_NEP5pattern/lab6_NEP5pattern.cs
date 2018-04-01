using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

// NEP5 Token Proposal: https://github.com/neo-project/proposals/blob/master/nep-5.mediawiki

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
        static readonly byte[] _OwnerAccountScriptHash = "ATrzHaicmhRj15C3Vv6e6gLfLqhSD2PtTr".ToScriptHash(); // for demo purposes

        public static event Action<byte[], byte[], BigInteger> transfer;

        public static object Main(string operation, params object[] args)
        {
            object result = false; // = 0 (zero)

            NEP5Base TOKENBASE = new NEP5Base { TotalSupply = 10000,
                                                Name = "My Test Token",
                                                Symbol = "MTT",
                                                Decimals = 8,
                                                OwnerAccountScriptHash = _OwnerAccountScriptHash };

            if (operation == "totalSupply")
            {
                Runtime.Notify("totalSupply");
                result = TotalSupply(TOKENBASE);
            }
            else if (operation == "name")
            {
                Runtime.Notify("name");
                result = Name(TOKENBASE);
            }
            else if (operation == "symbol")
            {
                Runtime.Notify("symbol");
                result = Symbol(TOKENBASE);
            }
            else if (operation == "decimals")
            {
                Runtime.Notify("decimals");
                result = Decimals(TOKENBASE);
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
                    result = BalanceOf(TOKENBASE, account);
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
                    result = Transfer(TOKENBASE, from, to, amount);
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

        private static BigInteger TotalSupply(NEP5Base TOKENBASE)
        {
            return TOKENBASE.TotalSupply;
        }

        private static string Name(NEP5Base TOKENBASE)
        {
            return TOKENBASE.Name;
        }

        private static string Symbol(NEP5Base TOKENBASE)
        {
            return TOKENBASE.Symbol;
        }

        private static byte Decimals(NEP5Base TOKENBASE)
        {
            return TOKENBASE.Decimals;
        }

        private static BigInteger BalanceOf(NEP5Base TOKENBASE, byte[] account)
        {
            BigInteger result = 0;

            StorageContext ctx = Storage.CurrentContext;
            BigInteger currentBalance = Storage.Get(ctx, account).AsBigInteger();

            result = currentBalance;

            return result;
        }

        private static bool Deploy(NEP5Base TOKENBASE)
        {
            bool result = false;

            if (Runtime.CheckWitness(TOKENBASE.OwnerAccountScriptHash))
            {
                StorageContext ctx = Storage.CurrentContext;

                // Create on-chain ledger entry for the owner of this token. Check to see if the ledger already exists
                byte[] currentBalance = Storage.Get(ctx, TOKENBASE.OwnerAccountScriptHash);
                if (currentBalance.Length == 0)
                {
                    Storage.Put(ctx, TOKENBASE.OwnerAccountScriptHash, TOKENBASE.TotalSupply);
                    result = true;
                }
            }

            return result;
        }

        private static bool Transfer(NEP5Base TOKENBASE, byte[] from, byte[] to, BigInteger amount)
        {
            bool result = false;

            if (amount > 0)
            {
                if (Runtime.CheckWitness(from)) // is the account invoking this contract == "from" account
                {
                    if (from == to)
                    {
                        result = true;
                    }
                    else // from != to
                    {
                        StorageContext ctx = Storage.CurrentContext;

                        // Get balance from the "from" ledger
                        BigInteger fromBalance = Storage.Get(ctx, from).AsBigInteger();
                        if (fromBalance > amount)
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
    }
}


