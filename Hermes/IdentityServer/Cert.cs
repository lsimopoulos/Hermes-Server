using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Hermes.IdentityServer
{
    //code taken from https://github.com/IdentityServer/IdentityServer3.Demo/blob/master/AzureWebSitesDeployment/Config/Cert.cs 

    public class Cert
    {
        /// <summary>
        /// returns the loaded certificate.
        /// </summary>
        /// <param name="certificateName"> certificate name with the ending</param>
        /// <param name="password">the password</param>
        public static X509Certificate2 Get(string certificateName, string password)
        {
            using var stream = File.OpenRead(certificateName);
            return new X509Certificate2(ReadStream(stream), password);
        }

        /// <summary>
        /// Read stream.
        /// </summary>
        /// <param name="input"></param>
        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using var ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0) ms.Write(buffer, 0, read);
            return ms.ToArray();
        }
    }
}
