using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MachineKey
{
    internal static class UnsafeNativeMethods
    {
        //[DllImport(ModName.ENGINE_FULL_NAME)]
        [DllImport(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\webengine4.dll")]
        internal static extern int EcbCallISAPI(IntPtr pECB, UnsafeNativeMethods.CallISAPIFunc iFunction, byte[] bufferIn, int sizeIn, byte[] bufferOut, int sizeOut);

        /////////////////////////////////////////////////////////////////////////////
        // List of functions supported by PMCallISAPI
        //
        // ATTENTION!!
        // If you change this list, make sure it is in sync with the
        // CallISAPIFunc enum in ecbdirect.h
        //
        internal enum CallISAPIFunc : int
        {
            GetSiteServerComment = 1,
            RestrictIISFolders = 2,
            CreateTempDir = 3,
            GetAutogenKeys = 4,
            GenerateToken = 5
        };
    }
}
