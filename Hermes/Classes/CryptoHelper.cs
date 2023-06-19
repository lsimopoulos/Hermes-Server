using Hermes.IdentityServer;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Hermes.Classes
{
    public class CryptoHelper
    {
        /// <summary>
        ///     Decrypt using RSA private key of certificate.
        /// </summary>
        /// <param name="encryptedText">The encrypted message.</param>
        public string Decrypt(string encryptedText)
        {
            var cert = Cert.Get();
            var rsaPrivateKey = cert.GetRSAPrivateKey();
            var decryptedArray = rsaPrivateKey.Decrypt(Convert.FromBase64String(encryptedText), RSAEncryptionPadding.OaepSHA512);
            return Encoding.ASCII.GetString(decryptedArray);

        }
        /// <summary>
        /// Encrypt using RSA public key of certificate.
        /// </summary>
        /// <param name="text"></param>
        public string Encrypt(string text)
        {
            var cert = Cert.Get();
            var rsaPublicKey = cert.GetRSAPublicKey();
            var encryptedArray = rsaPublicKey.Encrypt(Encoding.ASCII.GetBytes(text), RSAEncryptionPadding.OaepSHA512);
            return Convert.ToBase64String(encryptedArray);
        }

    }
}
