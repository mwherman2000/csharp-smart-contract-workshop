using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace lab3_storage
{
    public class lab3_storage : SmartContract
    {
        public static void Main()
        {
            // Get a copy of the NEO Storage context
            StorageContext ctx = Storage.CurrentContext;

            string item_key = "test-storage-key";
            Runtime.Notify("item_key", item_key);

            // Try to get a value for this key from storage
            BigInteger item_value = Storage.Get(ctx, item_key).AsBigInteger();
            Runtime.Notify("item_value", item_value);
            if (item_value == 0)
            {
                Runtime.Notify("Storage key not set. Setting item_value to 1");
                item_value = 1;
            }
            else
            {
                Runtime.Notify("Storage key already set. Incrementing item_value by 1");
                item_value += 1;
            }

            // Put the updated value for this key into NEO Storage
            Storage.Put(ctx, item_key, item_value);
            Runtime.Notify("New item_value", item_value);
        }
    }
}


