using Flow.Net.Sdk.Extensions;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using System;
using System.Linq;

namespace Flow.Net.Sdk.Crypto.Ecdsa
{
    public static class Utilities
    {
        public static AsymmetricCipherKeyPair GenerateKeyPair(SignatureAlgo signatureAlgo = SignatureAlgo.ECDSA_P256)
        {
            var curveName = SignatureAlgorithmCurveName(signatureAlgo);

            var curve = ECNamedCurveTable.GetByName(curveName);
            var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            var secureRandom = new SecureRandom();
            var keyParams = new ECKeyGenerationParameters(domainParams, secureRandom);

            var generator = new ECKeyPairGenerator("ECDSA");
            generator.Init(keyParams);
            var key = generator.GenerateKeyPair();

            if (DecodePublicKeyToHex(key).Length != 128)
                return GenerateKeyPair(signatureAlgo);

            return key;
        }

        public static ECPrivateKeyParameters GeneratePrivateKeyFromHex(string privateKeyHex, SignatureAlgo signatureAlgo)
        {
            var curveName = SignatureAlgorithmCurveName(signatureAlgo);
            var curve = ECNamedCurveTable.GetByName(curveName);
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            return new ECPrivateKeyParameters(new BigInteger(privateKeyHex, 16), domain);
        }

        public static string DecodePrivateKeyToHex(AsymmetricCipherKeyPair keyPair)
        {
            var privateKey = keyPair.Private as ECPrivateKeyParameters;
            return privateKey.D.ToByteArrayUnsigned().FromByteArrayToHex();
        }

        public static string DecodePublicKeyToHex(AsymmetricCipherKeyPair keyPair)
        {
            var publicKey = keyPair.Public as ECPublicKeyParameters;
            var pubKeyX = publicKey.Q.XCoord.ToBigInteger().ToByteArrayUnsigned();
            var pubKeyY = publicKey.Q.YCoord.ToBigInteger().ToByteArrayUnsigned();
            return pubKeyX.Concat(pubKeyY).ToArray().FromByteArrayToHex();
        }

        public static AsymmetricCipherKeyPair AsymmetricCipherKeyPairFromPrivateKey(string privateKeyHex, SignatureAlgo signatureAlgo)
        {
            var privateParams = GeneratePrivateKeyFromHex(privateKeyHex, signatureAlgo);
            var privateKeyArray = privateParams.D.ToByteArrayUnsigned();

            var privKeyInt = new BigInteger(+1, privateKeyArray);

            var curveName = SignatureAlgorithmCurveName(signatureAlgo);
            var curve = ECNamedCurveTable.GetByName(curveName);
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            ECPoint eCPoint = curve.G.Multiply(privKeyInt).Normalize();

            var publicKey = new ECPublicKeyParameters(eCPoint, domain);

            return new AsymmetricCipherKeyPair(publicKey, privateParams);
        }

        public static SignatureAlgo GetSignatureAlgorithm(AsymmetricCipherKeyPair keyPair)
        {
            var publicKey = keyPair.Public as ECPublicKeyParameters;
            var ECNamedCurves = ECNamedCurveTable.Names;

            foreach (var name in ECNamedCurves)
            {
                var curve = ECNamedCurveTable.GetByName((string)name);
                if (curve != null
                    && curve.Curve == publicKey.Parameters.Curve
                    && curve.G == publicKey.Parameters.G
                    && curve.N == publicKey.Parameters.N
                    && curve.H == publicKey.Parameters.H)
                {
                    switch ((string)name)
                    {
                        case "secp256r1":
                            return SignatureAlgo.ECDSA_P256;
                        case "secp256k1":
                            return SignatureAlgo.ECDSA_secp256k1;
                    }
                }
            }

            throw new Exception("Failed to find signature algorithm");
        }

        public static string SignatureAlgorithmCurveName(SignatureAlgo signatureAlgo)
        {
            switch (signatureAlgo)
            {
                case SignatureAlgo.ECDSA_P256:
                    return "P-256";
                case SignatureAlgo.ECDSA_secp256k1:
                    return "secp256k1";
            }

            throw new Exception("Invalid signature algorithm");
        }

        public static ISigner CreateSigner(string privateKeyHex, SignatureAlgo signatureAlgo, HashAlgo hashAlgo)
        {
            var privateKeyParams = GeneratePrivateKeyFromHex(privateKeyHex, signatureAlgo);
            return new Signer(privateKeyParams, hashAlgo, signatureAlgo);
        }
    }
}
