namespace Flow.Net.Sdk.Core
{
    public enum SignatureAlgo
    {
        ECDSA_P256 = 2,
        ECDSA_secp256k1 = 3
    }

    public enum HashAlgo
    {
        SHA2_256 = 1,
        SHA3_256 = 3
    }

    /// <summary>This value indicates whether the transaction execution succeded or not, this value should be checked when determining transaction success.</summary>
    public enum TransactionExecution
    {
        Pending = 0,
        Success = 1,
        Failure = 2
    }

    /// <summary>This value indicates the state of the transaction execution. Only sealed and expired are final and immutable states.</summary>
    public enum TransactionStatus
    {
        Pending = 0,
        Finalized = 1,
        Executed = 2,
        Sealed = 3,
        Expired = 4
    }
}
