using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

// Testing:
// operation,   args
// ---------,   --------------------------------------------------
// "query",     ["test.com"]
// "register",  ["test.com", "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y"]
// "delete",    ["test.com"]
// "transfer",  ["test.com", "AZ9Bmz6qmboZ4ry1z8p2KF3ftyA2ckJAym"]

namespace lab5_domain
{
    public class lab5_domain : SmartContract
    {
        public static object Main(string operation, params object[] args)
        {
            object result = false; // = 0 (zero)

            if (args.Length == 0)
            {
                Runtime.Log("No domain named supplied");
                result = 0;
            }
            else
            {
                string domain_name = (string)args[0];
                if (operation == "query")
                {
                    result = QueryDomain(domain_name);
                }
                else if (operation == "delete")
                {
                    result = DeleteDomain(domain_name);
                }
                else if (operation == "register")
                {
                    if (args.Length < 2)
                    {
                        Runtime.Log("Required arguments: [\"domain_name\", \"owner\"]");
                        result = 0;
                    }
                    else
                    {
                        byte[] owner = (byte[])args[1];
                        result = RegisterDomain(domain_name, owner);
                    }
                }
                else if (operation == "transfer")
                {
                    if (args.Length < 2)
                    {
                        Runtime.Log("Required arguments: [\"domain_name\", \"to_address\"]");
                        result = 0;
                    }
                    else
                    {
                        byte[] to_address = (byte[])args[1];
                        result = TransferDomain(domain_name, to_address);
                    }
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        private static object QueryDomain(string domain_name)
        {
            object result = 0;

            Runtime.Notify("QueryDomain", domain_name);

            StorageContext ctx = Storage.CurrentContext;
            byte[] owner = Storage.Get(ctx, domain_name);
            if (owner.Length == 0)
            {
                Runtime.Notify("Domain is not registered", domain_name);
                result = 0;
            }
            else
            {
                result = owner;
            }

            return result;
        }

        private static bool RegisterDomain(string domain_name, byte[] owner)
        {
            bool result = false;

            Runtime.Notify("RegisterDomain", domain_name, owner);

            if (false) // !Runtime.CheckWitness(owner))
            {
                Runtime.Notify("Owner argument is not the same as the sender", owner);
                result = false;
            }
            else
            {
                StorageContext ctx = Storage.CurrentContext;
                byte[] exists = Storage.Get(ctx, domain_name);
                if (exists.Length != 0)
                {
                    Runtime.Notify("Domain is already registered", domain_name, exists);
                    result = false;
                }
                else
                {
                    Storage.Put(ctx, domain_name, owner);
                    result = true;
                }
            }

            return result;
        }

        private static bool TransferDomain(string domain_name, byte[] to_address)
        {
            bool result = false;

            Runtime.Notify("TransferDomain", domain_name, to_address);

            StorageContext ctx = Storage.CurrentContext;
            byte[] owner = Storage.Get(ctx, domain_name);
            if (owner.Length == 0)
            {
                Runtime.Notify("Domain is not registered", domain_name);
                result = false;
            }
            else
            {
                if (false) // !Runtime.CheckWitness(owner))
                {
                    Runtime.Notify("Sender is not the owner of this domain, cannot transfer", domain_name);
                    result = false;
                }
                else
                {
                    if (to_address.Length != 34)
                    {
                        Runtime.Notify("Invalid new owner address. Must be exactly 34 characters", to_address);
                        result = false;
                    }
                    else
                    {
                        Storage.Put(ctx, domain_name, to_address);
                        result = true;
                    }
                }
            }

            return result;
        }

        private static bool DeleteDomain(string domain_name)
        {
            bool result = false;

            Runtime.Notify("DeleteDomain", domain_name);

            StorageContext ctx = Storage.CurrentContext;
            byte[] owner = Storage.Get(ctx, domain_name);
            if (owner.Length == 0)
            {
                Runtime.Notify("Domain is not registered", domain_name);
                result = false;
            }
            else
            {
                if (false) // !Runtime.CheckWitness(owner))
                {
                    Runtime.Notify("Sender is not the owner on this domain, cannot delete", domain_name);
                    result = false;
                }
                else
                {
                    Storage.Delete(ctx, domain_name);
                    result = true;
                }
            }
    
            return result;
        }
    }
}


