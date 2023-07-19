using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MachineKey
{
    class AutoGenKeys
    {
        // HKEY_CURRENT_USER\Software\Microsoft\ASP.NET\4.0.30319.0
        // AutoGenKeyV4

        internal static byte[] s_autogenKeys = new byte[1024];

        public void Init()
        {
            SetAutogenKeys();
        }

        private static void SetAutogenKeys()
        {
#if !FEATURE_PAL // FEATURE_PAL does not enable cryptography
            byte[] bKeysRandom = new byte[s_autogenKeys.Length];
            byte[] bKeysStored = new byte[s_autogenKeys.Length];
            bool fGetStoredKeys = false;
            RNGCryptoServiceProvider randgen = new RNGCryptoServiceProvider();

            // Gernerate random keys
            randgen.GetBytes(bKeysRandom);

            // If getting stored keys via WorkerRequest object failed, get it directly
            if (!fGetStoredKeys)
                fGetStoredKeys = (UnsafeNativeMethods.EcbCallISAPI(IntPtr.Zero, UnsafeNativeMethods.CallISAPIFunc.GetAutogenKeys,
                                                                   bKeysRandom, bKeysRandom.Length, bKeysStored, bKeysStored.Length) == 1);

            // If we managed to get stored keys, copy them in; else use random keys
            if (fGetStoredKeys)
                Buffer.BlockCopy(bKeysStored, 0, s_autogenKeys, 0, s_autogenKeys.Length);
            else
                Buffer.BlockCopy(bKeysRandom, 0, s_autogenKeys, 0, s_autogenKeys.Length);
#endif // !FEATURE_PAL
        }
    }
}
