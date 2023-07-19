using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security.Cryptography;

namespace MachineKey
{
    class MachineKeyDemo
    {
        // reg query HKEY_CURRENT_USER\Software\Microsoft\ASP.NET\4.0.30319.0 /v AutoGenKeyV4
        // AutoGenKeyCreationTime
        // AutoGenKeyFormat
        // AutoGenKeyV4
        static void Main(string[] args)
        {
            MachineKeyDemo demo = new MachineKeyDemo();

            AutoGenKeys keys = new AutoGenKeys();
            keys.Init();

            demo.GetAutoGenKeys();
        }

        public void GetEncryptionKey()
        {
            // These are internal sealed classes in System.Web.Security which cannot be accessed
            //MachineKeyMasterKeyProvider provider = new MachineKeyMasterKeyProvider();

            //CryptographicKey masterKey = provider.GetEncryptionKey();
        }

        public void GetAutoGenKeys()
        {
            // GetField returns FieldInfo
            // FieldInfo.GetValue(object obj) expects 'obj' to be an instance of the type
            byte[] autogen_keys = (byte[]) typeof(AutoGenKeys).GetField("s_autogenKeys", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            Console.WriteLine("keys: ({0}) {1}", autogen_keys.Length,
                autogen_keys.Aggregate(new StringBuilder(),
                    (sb, c) => sb.Append(string.Format("{0:x2}", c)),
                    sb => sb.ToString()));

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
