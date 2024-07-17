using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
namespace Blog.Helpers
{
    public static class CsrHelper
    {
        public static byte[] GenerateCsr(string subjectName, out string base64Csr)
        {
            using (var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
            {
                var request = new CertificateRequest(
                    new X500DistinguishedName(subjectName),
                    ecdsa,
                    HashAlgorithmName.SHA256);

                var csr = request.CreateSigningRequest();
                base64Csr = Convert.ToBase64String(csr);

                return csr;
            }
        }
    }
}
