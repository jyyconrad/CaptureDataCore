using System;
using System.Security.Cryptography;
using System.Text;

namespace CaptureData.SIP.Cores
{
    public class HTTPDigest
    {
        /// <summary>
        /// Calculate H(A1) as per HTTP Digest specification.
        /// </summary>
        public static string DigestCalcHA1(
            string username,
            string realm,
            string password)
        {
            string a1 = String.Format("{0}:{1}:{2}", username, realm, password);
            return GetMD5HashBinHex(a1);
        }

        /// <summary>
        /// Calculate H(A2) as per HTTP Digest specification.
        /// </summary>
        public static string DigestCalcHA2(
            string method,
            string uri)
        {
            string A2 = String.Format("{0}:{1}", method, uri);

            return GetMD5HashBinHex(A2);
        }

        public static string DigestCalcResponse(
            string algorithm,
            string username,
            string realm,
            string password,
            string uri,
            string nonce,
            string nonceCount,
            string cnonce,
            string qop,         // qop-value: "", "auth", "auth-int".
            string method,
            string digestURL,
            string hEntity
            )
        {
            string HA1 = DigestCalcHA1(username, realm, password);
            string HA2 = DigestCalcHA2(method, uri);

            string unhashedDigest = null;
            if (nonceCount != null && cnonce != null && qop != null)
            {
                unhashedDigest = String.Format("{0}:{1}:{2}:{3}:{4}:{5}",
                HA1,
                nonce,
                nonceCount,
                cnonce,
                qop,
                HA2);
            }
            else
            {
                unhashedDigest = String.Format("{0}:{1}:{2}",
                HA1,
                nonce,
                HA2);
            }

            return GetMD5HashBinHex(unhashedDigest);
        }

        public static string GetMD5HashBinHex(string val)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bHA1 = md5.ComputeHash(Encoding.UTF8.GetBytes(val));
            string HA1 = null;
            for (int i = 0; i < 16; i++)
                HA1 += String.Format("{0:x02}", bHA1[i]);
            return HA1;
        }
    }
}