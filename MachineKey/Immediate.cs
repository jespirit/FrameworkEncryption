using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineKey
{
    class Immediate
    {
        public void GetBytes()
        {
            var getBytes = new Func<byte[], string>(delegate (byte[] randomKeys) {
                var sb = new StringBuilder();
                for (int i = 0; i < randomKeys.Length; i++)
                {
                    sb.Append(string.Format("{0:x2}", randomKeys[i]));
                }
                return sb.ToString();
            });

            getBytes(bKeysRandom);
        }
    }
}
