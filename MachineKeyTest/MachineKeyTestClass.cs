using MachineKeyTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Security.Cryptography;
using System.Web.UI;

namespace System.Web.Security.Cryptography.MachineKeyTest
{
    class MachineKeyTestClass
    {
        // Combined with Purpose objects which are passed in during serialization / deserialization.
        private List<string> _specificPurposes;
        // We use page to determine whether to to encrypt or decrypt based on Page.RequiresViewStateEncryptionInternal or Page.ContainsEncryptedViewstate
        private Page _page;

        static void Main(string[] args)
        {
            MachineKeyTestClass demo = new MachineKeyTestClass();

            demo.Test2(args[0], args[1]);
        }

        void Test1()
        {
            // Init() must be called or the auto-generated keys won't be read from registry key
            AutoGenKeys keys = new AutoGenKeys();
            AutoGenKeys.Init();

            // Can't find method MachineKeySection.GetApplicationConfig
            //MachineKeySection machineKeySection = RuntimeConfig.GetAppConfig().MachineKey;
            // = MachineKeySection.GetApplicationConfig();


            IMasterKeyProvider masterKeyProvider = new MachineKeyMasterKeyProvider(new MachineKeySectionMinimal());

            CryptographicKey masterKey = masterKeyProvider.GetEncryptionKey();

            string hex = CryptoUtil.BinaryToHex(masterKey.GetKeyMaterial());

            Console.WriteLine(hex);

#if DEBUG
            Console.ReadKey();
#endif
        }

        void Test2(string templateDirectory, string pageType)
        {
            // Init() must be called or the auto-generated keys won't be read from registry key
            AutoGenKeys keys = new AutoGenKeys();
            AutoGenKeys.Init();

            // Can't find method MachineKeySection.GetApplicationConfig
            //MachineKeySection machineKeySection = RuntimeConfig.GetAppConfig().MachineKey;
            // = MachineKeySection.GetApplicationConfig();


            IMasterKeyProvider masterKeyProvider = new MachineKeyMasterKeyProvider(new MachineKeySectionMinimal());

            // Not in use
            //CryptographicKey masterKey = masterKeyProvider.GetEncryptionKey();

            Purpose purpose = Purpose.WebForms_HiddenFieldPageStatePersister_ClientState;
            // Don't forget to add the specific purposes
            //string pageName = "/MinEterm/SimpleTerm";
            //string pageType = "SimpleTerm_GettingStarted_aspx";

            //pageName = "/MinEterm/SimpleTerm";
            //pageType = "SimpleTerm_InsuranceInformation_aspx";

            Purpose derivedPurpose = purpose.AppendSpecificPurposes(GetSpecificPurposes(templateDirectory, pageType));

            //CryptographicKey actualDerivedKey = purpose.GetDerivedEncryptionKey(masterKeyProvider, SP800_108.DeriveKey);

            CryptographicKey encryptionKey = derivedPurpose.GetDerivedEncryptionKey(masterKeyProvider, SP800_108.DeriveKey);
            CryptographicKey validationKey = derivedPurpose.GetDerivedValidationKey(masterKeyProvider, SP800_108.DeriveKey);

            NetFXCryptoService cryptoService = new NetFXCryptoService(new MachineKeyCryptoAlgorithmFactory(new MachineKeySectionMinimal()),
                encryptionKey, validationKey, false);

            // Now use NetFxCryptoService to protect or unprotect byte arrays

            ParseViewStateClass parser = new ParseViewStateClass();

            // Calling ObjectStateFormatter.Deserialize won't work because you can't set the Purpose
            ObjectStateFormatter formatter = new ObjectStateFormatter();

            string inputFile = "viewstate.txt";

            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine("Failed to open file '{0}'", inputFile);
                return;
            }

            string vsRawBase64;
            object viewstate;

            try
            {
                vsRawBase64 = File.ReadAllText(inputFile);
                byte[] inputBytes = Convert.FromBase64String(vsRawBase64);
                byte[] clearData = cryptoService.Unprotect(inputBytes);

                if (clearData == null)
                {
                    throw new ViewStateException();
                }

                using (MemoryStream objectStream = new MemoryStream(2048))
                {
                    objectStream.Write(clearData, 0, clearData.Length);
                    objectStream.Position = 0;
                    // Deserialize(Stream) is public
                    // Deserialize(string, Purpose) is private
                    viewstate = formatter.Deserialize(objectStream);
                }

                parser.ParseViewState(viewstate, 0);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            
        }

        // This will return a list of specific purposes (for cryptographic subkey generation).
        public List<string> GetSpecificPurposes(string templateSourceDirectory, string type)
        {
            if (_specificPurposes == null)
            {
                // Only generate a specific purpose list if we have a Page
                // Not a page
                //if (_page == null)
                //{
                //    return null;
                //}

                // Note: duplicated (somewhat) in GetMacKeyModifier, keep in sync
                // See that method for comments on why these modifiers are in place

                List<string> specificPurposes = new List<string>() {
                    "TemplateSourceDirectory: " + ((_page != null) ? _page.TemplateSourceDirectory.ToUpperInvariant() : templateSourceDirectory.ToUpperInvariant()),
                    "Type: " + ((_page != null) ? _page.GetType().Name.ToUpperInvariant() : type.ToUpperInvariant())
                };

                if (_page != null && _page.ViewStateUserKey != null)
                {
                    specificPurposes.Add("ViewStateUserKey: " + _page.ViewStateUserKey);
                }

                _specificPurposes = specificPurposes;
            }

            return _specificPurposes;
        }
    }
}
