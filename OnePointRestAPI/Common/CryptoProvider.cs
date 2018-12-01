#region Header
/*
 ************************************************************************************
 Name: CryptoProvider
 Description: This is for executing all the excription/decripption operations
 Created On:  28-sep-2018
 Created By:  Uday Kiran
 Last Modified On: 
 Last Modified By: 
 Last Modified Reason: 
 ************************************************************************************
 */
#endregion

using Microsoft.AspNetCore.DataProtection;

namespace OnePointRestAPI.Common
{
    public class CryptoProvider
    {
        IDataProtector _CryptoProvider;

        public CryptoProvider(IDataProtectionProvider provider)
        {
            _CryptoProvider = provider.CreateProtector(GetType().FullName);
        }

        public string Encrypt(string unProtectedString)
        {
            string protectedString = _CryptoProvider.Protect(unProtectedString);
            return protectedString;
        }

        public string Decrypt(string protectedString)
        {
            string unProtectedString = _CryptoProvider.Unprotect(protectedString);
            return unProtectedString;
        }
    }
}
