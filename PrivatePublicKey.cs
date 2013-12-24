using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
namespace PipeServer
{
    public class PrivatePublicKey
    {
        private RSACryptoServiceProvider rsa;
        private string publicPrivateKeyXML;
        private string publicOnlyKeyXML;
        private List<string> Keys;
        public PrivatePublicKey()
        {
            this.rsa = new RSACryptoServiceProvider();
            this.Keys = new List<string>();
            this.publicPrivateKeyXML = string.Empty;
            this.publicOnlyKeyXML = string.Empty;
        }
        public List<string> CreateKeyPairs()
        {
            if (rsa != null)
            {
                this.publicPrivateKeyXML = rsa.ToXmlString(true);
                this.Keys.Add(publicPrivateKeyXML);

                this.publicOnlyKeyXML = rsa.ToXmlString(false);
                this.Keys.Add(publicOnlyKeyXML);
                return this.Keys;
            }
            else
                return null;
        }
    }
}
