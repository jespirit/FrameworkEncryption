using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Security.Cryptography
{
    public interface IMachineKeySectionMinimal
    {
        // These are read-write properties
        string DecryptionKey { get; set; }
        string ValidationKey { get; set; }

        string GetValidationAttributeSkipValidation();
        string GetDecryptionAttributeSkipValidation();
    }

    public class MachineKeySectionMinimal : IMachineKeySectionMinimal
    {
        // If the default validation algorithm changes, be sure to update the _HashSize and _AutoGenValidationKeySize fields also.
        internal const string DefaultValidationAlgorithm = "HMACSHA256";

        public string DecryptionKey
        {
            get
            {
                return ConfigurationManager.AppSettings["decryptionKey"];
            }

            set {}
        }

        public string ValidationKey
        {
            get
            {
                return ConfigurationManager.AppSettings["validationKey"];
            }

            set { }
        }

        // returns the value in the 'validation' attribute (or the default value if null) without throwing an exception if the value is malformed
        public string GetValidationAttributeSkipValidation()
        {
            //return (string)base[_propValidation] ?? DefaultValidationAlgorithm;
            return DefaultValidationAlgorithm;
        }

        // returns the value in the 'decryption' attribute (or the default value if null) without throwing an exception if the value is malformed
        public string GetDecryptionAttributeSkipValidation()
        {
            //return (string)base[_propDecryption] ?? "Auto";
            return "Auto";
        }
    }
}
