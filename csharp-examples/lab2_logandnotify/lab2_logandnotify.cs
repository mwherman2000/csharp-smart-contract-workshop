using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;

namespace lab2_logandnotify
{
    public class lab2_logandnotify : SmartContract
    {
        public static void Main()
        {
            // `Log()` is best to add simple strings to the NEO event log for development purposes 
            Runtime.Log("log 1");
            Runtime.Log("log 2");
            Runtime.Notify("notify 3");

            // `Notify()` is best for adding variables such as lists and objects to the NEO event log
            int ten = 10;
            Runtime.Notify("ten", ten);

            object[] results = { "a", 1, 2, "3" };
            Runtime.Notify("results", results);

            User e1 = new User { FirstName = "Able", LastName = "Baker" };
            Runtime.Notify("e1", e1);
            User e2 = new User { FirstName = "Charlie", LastName = "Delta" };
            User[] userEntities = { e1, e2 };
            Runtime.Notify("userEntities", userEntities);
        }

        public class User
        {
            public string FirstName;
            public string LastName;
        }
    }
}



