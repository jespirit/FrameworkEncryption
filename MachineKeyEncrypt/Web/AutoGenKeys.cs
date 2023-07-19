using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System.Web
{
    public class AutoGenKeys
    {
        // HKEY_CURRENT_USER\Software\Microsoft\ASP.NET\4.0.30319.0
        // AutoGenKeyV4

        internal static byte[] s_autogenKeys = new byte[1024];

        public static void Init()
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

            // FIXME: Unchecked prefer 32-bit for MachineKeyTest and getting System.BadImageFormatException
            // MachineKeyEncrypt: Any CPU, Prefer 32-bit is greyed out?
            // Prefer 32-bit is only allowed for executables
            // MachineKeyTest: Was Any CPU + Prefer 32-bit
            // System.BadImageFormatException: 'An attempt was made to load a program with an incorrect format. (Exception from HRESULT: 0x8007000B)'

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
