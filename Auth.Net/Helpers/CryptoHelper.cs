using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Web;

namespace Auth.Net.Helpers
{
    public static class CryptoHelper
    {

        public static string ToHex(this byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }


        public static byte[] GetRandomKey(int length)
        {
            var keyGenerator = new Rfc2898DeriveBytes(Guid.NewGuid().ToString(), Guid.NewGuid().ToByteArray(), 300);

            return  keyGenerator.GetBytes(length);

        }

        static byte[] B(string s)
        {
            var b = BigInteger.Parse(s);
            var ret = b.ToByteArray();
            if (ret[ret.Length - 1] == 0)
            {
                Array.Resize(ref ret, ret.Length - 1);
            }
            Array.Reverse(ret);
            return ret;
        }
        public static byte[] EncryptRSA(byte[] pubKey, byte[] data)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var keyInfo = rsa.ExportParameters(false);
                //Set rsa to the public key values. 
                keyInfo.Modulus = pubKey;
                //cryptico js is using 03 as exponent
                keyInfo.Exponent =  B("03");
                //Import key parameters into RSA.
                rsa.ImportParameters(keyInfo);
                return rsa.Encrypt(data,false);
            }

        }

        public static string DecryptAES256(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                //rijAlg.Padding = PaddingMode.ANSIX923;
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {

                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}