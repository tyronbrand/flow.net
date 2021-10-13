using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using System;
using System.Linq;

namespace Flow.Net.Sdk.Crypto.Ecdsa
{
    public class Signer : ISigner
    {
        public ECPrivateKeyParameters PrivateKey { get; private set; }
        public HashAlgo HashAlgo { get; private set; }
        public string SignatureCurveName { get; private set; }

        public Signer(ECPrivateKeyParameters privateKey, HashAlgo hashAlgorithm, SignatureAlgo signatureAlgo)
        {
            PrivateKey = privateKey;
            HashAlgo = hashAlgorithm;
            SignatureCurveName = Utilities.SignatureAlgorithmCurveName(signatureAlgo);
        }

        public byte[] Sign(byte[] bytes)
        {
            var curve = ECNamedCurveTable.GetByName(SignatureCurveName);
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var keyParameters = new ECPrivateKeyParameters(new BigInteger(PrivateKey.D.ToByteArrayUnsigned().FromByteArrayToHex(), 16), domain);

            var hash = Hasher.CalculateHash(bytes, HashAlgo);

            var signer = new ECDsaSigner();
            signer.Init(true, keyParameters);

            var output = signer.GenerateSignature(hash);

            var r = output[0].ToByteArrayUnsigned();
            var s = output[1].ToByteArrayUnsigned();            

            var rSig = new byte[32];
            Array.Copy(r, 0, rSig, rSig.Length - r.Length, r.Length);

            var sSig = new byte[32];
            Array.Copy(s, 0, sSig, sSig.Length - s.Length, s.Length);

            return rSig.Concat(sSig).ToArray();
        }
    }
}
