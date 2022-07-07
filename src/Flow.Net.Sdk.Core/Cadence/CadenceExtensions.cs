using Flow.Net.Sdk.Core.Cadence.Types;
using Flow.Net.Sdk.Core.Exceptions;
using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence
{
    public static class CadenceExtensions
    {
        ///<inheritdoc cref="Cadence.Encode"/>
        public static string Encode(this ICadence cadence)
        {
            return cadence.Encode(cadence);
        }
        
        ///<inheritdoc cref="CadenceType.Encode"/>
        public static string Encode(this ICadenceType cadenceType)
        {
            return cadenceType.Encode(cadenceType);
        }

        /// <summary>
        /// Decodes a <see cref="ICadence"/> JSON string.
        /// </summary>
        /// <param name="cadenceJson"></param>
        /// <param name="cadenceConverter"></param>
        /// <returns>The deserialized <see cref="ICadence"/> from the JSON string.</returns>
        public static ICadence Decode(this string cadenceJson)
        {
            JsonConverter[] jsonConverters = { new CadenceConverter(), new CadenceTypeConverter() };
            return JsonConvert.DeserializeObject<ICadence>(cadenceJson, jsonConverters);
        }

        /// <summary>
        /// Decodes a <see cref="ICadenceType"/> JSON string.
        /// </summary>
        /// <param name="cadenceJson"></param>
        /// <param name="cadenceConverter"></param>
        /// <returns>The deserialized <see cref="ICadenceType"/> from the JSON string.</returns>
        public static ICadenceType DecodeType(this string cadenceJson)
        {
            JsonConverter[] jsonConverters = { new CadenceConverter(), new CadenceTypeConverter() };
            return JsonConvert.DeserializeObject<ICadenceType>(cadenceJson, jsonConverters);
        }

        /// <summary>
        /// Casts <see cref="ICadence"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cadence"></param>
        /// <returns><typeparamref name="T"/>.</returns>
        public static T As<T>(this ICadence cadence)
            where T : ICadence
        {
            return (T)cadence;
        }

        ///<inheritdoc cref="Cadence.CompositeField"/>
        public static ICadence CompositeField(this CadenceComposite cadenceComposite, string fieldName)
        {
            return cadenceComposite.CompositeField(cadenceComposite, fieldName);
        }

        ///<inheritdoc cref="Cadence.CompositeFieldAs{T}(CadenceComposite, string)"/>
        public static T CompositeFieldAs<T>(this CadenceComposite cadenceComposite, string fieldName)
            where T : ICadence
        {
            return cadenceComposite.CompositeFieldAs<T>(cadenceComposite, fieldName);
        }

        /// <summary>
        /// Converts from <see cref="HashAlgo"/> to Cadence Enum <see cref="CadenceHashAlgorithm"/>
        /// </summary>
        /// <param name="hashAlgo"></param>
        /// <returns><see cref="CadenceHashAlgorithm"/> value</returns>
        public static CadenceHashAlgorithm FromHashAlgoToCadenceHashAlgorithm(HashAlgo hashAlgo)
        {
            switch (hashAlgo)
            {
                case HashAlgo.SHA3_256:
                    return CadenceHashAlgorithm.SHA3_256;
                case HashAlgo.SHA2_256:
                    return CadenceHashAlgorithm.SHA2_256;
            }

            throw new FlowException($"Failed to convert {hashAlgo} to adenceHashAlgorithm");
        }

        /// <summary>
        /// Converts from <see cref="SignatureAlgo"/> to Cadence Enum <see cref="CadenceSignatureAlgorithm"/>
        /// </summary>
        /// <param name="signatureAlgo"></param>
        /// <returns><see cref="CadenceSignatureAlgorithm"/> value</returns>
        public static CadenceSignatureAlgorithm FromSignatureAlgoToCadenceSignatureAlgorithm(SignatureAlgo signatureAlgo)
        {
            switch (signatureAlgo)
            {
                case SignatureAlgo.ECDSA_P256:
                    return CadenceSignatureAlgorithm.ECDSA_P256;
                case SignatureAlgo.ECDSA_secp256k1:
                    return CadenceSignatureAlgorithm.ECDSA_secp256k1;
            }

            throw new FlowException($"Failed to convert {signatureAlgo} to CadenceSignatureAlgorithm");
        }
    }
}
