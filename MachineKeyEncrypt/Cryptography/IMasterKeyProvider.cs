//------------------------------------------------------------------------------
// <copyright file="IMasterKeyProvider.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

namespace System.Web.Security.Cryptography {
    using System;

    // Represents an object that can provide master encryption / validation keys

    public interface IMasterKeyProvider {
    // public won't work because the CryptographicKey is less accessible than the methods
    // internal sealed class CryptographicKey
    //internal interface IMasterKeyProvider {


        // encryption + decryption key
        CryptographicKey GetEncryptionKey();

        // signing + validation key
        CryptographicKey GetValidationKey();

    }
}
