using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineKeyTest
{
    // How does Web Forms handle cryptographic errors in Protect()/Unprotect() methods?
    class HomogenizeErrors
    {
        private static byte[] HomogenizeErrors(Func<byte[], byte[]> func, byte[] input)
        {
            byte[] numArray = (byte[])null;
            bool flag = false;
            try
            {
                numArray = func(input);
                return numArray;
            }
            catch (ConfigurationException ex)
            {
                flag = true;
                throw;
            }
            finally
            {
                if (numArray == null && !flag)
                    throw new CryptographicException();
            }
        }
    }
}
