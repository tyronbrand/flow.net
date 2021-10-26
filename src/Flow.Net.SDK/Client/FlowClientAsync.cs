using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Exceptions;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Protos.access;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Grpc.Core;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Flow.Net.Sdk.Protos.access.AccessAPI;

namespace Flow.Net.Sdk.Client
{
    public class FlowClientAsync
    {
        private readonly AccessAPIClient _client;
        private readonly CadenceConverter _cadenceConverter;

        private FlowClientAsync(AccessAPIClient client)
        {
            _client = client;
            _cadenceConverter = new CadenceConverter();
        }

        /// <summary>
        /// A gRPC client for the Flow Access API.
        /// </summary>
        /// <param name="flowNetworkUrl"></param>
        /// <param name="channelCredentialsSecureSsl"></param>
        /// <param name="options"></param>
        /// <returns>FlowClientAsync.</returns>
        public static FlowClientAsync Create(string flowNetworkUrl, bool channelCredentialsSecureSsl = false, List<ChannelOption> options = null)
        {
            try
            {
                var client = new AccessAPIClient(new Channel(
                    flowNetworkUrl,
                    channelCredentialsSecureSsl ? ChannelCredentials.SecureSsl : ChannelCredentials.Insecure,
                    options
                ));

                return new FlowClientAsync(client);
            }
            catch (Exception exception)
            {
                throw new FlowException($"Failed to connect to {flowNetworkUrl}", exception);
            }
        }

        /// <summary>
        /// Ping is used to check if the access node is alive and healthy.
        /// </summary>
        public async Task PingAsync(CallOptions options = new CallOptions())
        {
            try
            {
                await _client.PingAsync(new PingRequest(), options);
            }
            catch (Exception exception)
            {
                throw new FlowException("Ping request failed.", exception);
            }
        }

        /// <summary>
        /// GetLatestBlockAsync gets the full payload of the latest sealed or unsealed block.
        /// </summary>
        /// <param name="isSealed"></param>
        /// <param name="options"></param>
        /// <returns>FlowBlock.</returns>
        public async Task<FlowBlock> GetLatestBlockAsync(bool isSealed = true, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetLatestBlockAsync(
                new GetLatestBlockRequest()
                {
                    IsSealed = isSealed
                }, options);

                return response.ToFlowBlock();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetLatestBlock request failed.", exception);
            }
        }

        /// <summary>
        /// GetBlockByHeightAsync gets a full block by height.
        /// </summary>
        /// <param name="blockHeight"></param>
        /// <param name="options"></param>
        /// <returns>FlowBlock.</returns>
        public async Task<FlowBlock> GetBlockByHeightAsync(ulong blockHeight, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetBlockByHeightAsync(
                new GetBlockByHeightRequest()
                {
                    Height = blockHeight
                }, options);

                return response.ToFlowBlock();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetBlockByHeight request failed.", exception);
            }
        }

        /// <summary>
        /// GetBlockByIdAsync gets a full block by Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="options"></param>
        /// <returns>FlowBlock.</returns>
        public async Task<FlowBlock> GetBlockByIdAsync(ByteString blockId, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetBlockByIDAsync(
                new GetBlockByIDRequest()
                {
                    Id = blockId
                }, options);

                return response.ToFlowBlock();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetBlockById request failed.", exception);
            }
        }

        /// <summary>
        /// GetLatestBlockHeaderAsync gets the latest sealed or unsealed block header.
        /// </summary>
        /// <param name="isSealed"></param>
        /// <param name="options"></param>
        /// <returns>FlowBlockHeader.</returns>
        public async Task<FlowBlockHeader> GetLatestBlockHeaderAsync(bool isSealed = true, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetLatestBlockHeaderAsync(
               new GetLatestBlockHeaderRequest
               {
                   IsSealed = isSealed
               }, options);

                return response.ToFlowBlockHeader();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetLatestBlockHeader request failed.", exception);
            }
        }

        /// <summary>
        /// GetBlockHeaderByHeightAsync gets a block header by height.
        /// </summary>
        /// <param name="blockHeaderHeight"></param>
        /// <param name="options"></param>
        /// <returns>FlowBlockHeader.</returns>
        public async Task<FlowBlockHeader> GetBlockHeaderByHeightAsync(ulong blockHeaderHeight, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetBlockHeaderByHeightAsync(
                new GetBlockHeaderByHeightRequest
                {
                    Height = blockHeaderHeight
                }, options);

                return response.ToFlowBlockHeader();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetBlockHeaderByHeight request failed.", exception);
            }
        }

        /// <summary>
        /// GetBlockHeaderByIdAsync gets a block header by Id.
        /// </summary>
        /// <param name="blockHeaderId"></param>
        /// <param name="options"></param>
        /// <returns>FlowBlockHeader.</returns>
        public async Task<FlowBlockHeader> GetBlockHeaderByIdAsync(ByteString blockHeaderId, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetBlockHeaderByIDAsync(
                new GetBlockHeaderByIDRequest
                {
                    Id = blockHeaderId
                }, options);

                return response.ToFlowBlockHeader();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetBlockHeaderById request failed.", exception);
            }
        }

        /// <summary>
        /// ExecuteScriptAtLatestBlockAsync executes a read-only Cadence script against the latest sealed execution state.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="arguments"></param>
        /// <param name="options"></param>
        /// <returns>ICadence.</returns>
        public async Task<ICadence> ExecuteScriptAtLatestBlockAsync(ByteString script, IEnumerable<ICadence> arguments = null, CallOptions options = new CallOptions())
        {
            try
            {
                var request = new ExecuteScriptAtLatestBlockRequest()
                {
                    Script = script
                };

                AddArgumentsToRequest(arguments, request.Arguments);

                var response = await _client.ExecuteScriptAtLatestBlockAsync(request, options);
                return response.Value.FromByteStringToString().Decode(_cadenceConverter);
            }
            catch (Exception exception)
            {
                throw new FlowException("ExecuteScriptAtLatestBlock request failed.", exception);
            }
        }

        /// <summary>
        /// ExecuteScriptAtBlockHeightAsync executes a ready-only Cadence script against the execution state at the given block height.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="blockHeight"></param>
        /// <param name="arguments"></param>
        /// <param name="options"></param>
        /// <returns>ICadence.</returns>
        public async Task<ICadence> ExecuteScriptAtBlockHeightAsync(ByteString script, ulong blockHeight, IEnumerable<ICadence> arguments = null, CallOptions options = new CallOptions())
        {
            try
            {
                var request = new ExecuteScriptAtBlockHeightRequest()
                {
                    Script = script,
                    BlockHeight = blockHeight
                };

                AddArgumentsToRequest(arguments, request.Arguments);

                var response = await _client.ExecuteScriptAtBlockHeightAsync(request, options);
                return response.Value.FromByteStringToString().Decode(_cadenceConverter);
            }
            catch (Exception exception)
            {
                throw new FlowException("ExecuteScriptAtBlockHeight request failed.", exception);
            }
        }

        /// <summary>
        /// ExecuteScriptAtBlockIdAsync executes a ready-only Cadence script against the execution state at the block with the given Id.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="blockId"></param>
        /// <param name="arguments"></param>
        /// <param name="options"></param>
        /// <returns>ICadence.</returns>
        public async Task<ICadence> ExecuteScriptAtBlockIdAsync(ByteString script, ByteString blockId, IEnumerable<ICadence> arguments = null, CallOptions options = new CallOptions())
        {
            try
            {
                var request = new ExecuteScriptAtBlockIDRequest()
                {
                    Script = script,
                    BlockId = blockId
                };

                AddArgumentsToRequest(arguments, request.Arguments);

                var response = await _client.ExecuteScriptAtBlockIDAsync(request, options);

                return response.Value.FromByteStringToString().Decode(_cadenceConverter);
            }
            catch (Exception exception)
            {
                throw new FlowException("ExecuteScriptAtBlockId request failed.", exception);
            }
        }

        /// <summary>
        /// GetEventsForHeightRangeAsync retrieves events for all sealed blocks between the start and end block heights (inclusive) with the given type.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="startHeight"></param>
        /// <param name="endHeight"></param>
        /// <param name="options"></param>
        /// <returns>IEnumerable<FlowBlockEvent>.</returns>
        public async Task<IEnumerable<FlowBlockEvent>> GetEventsForHeightRangeAsync(string eventType, ulong startHeight, ulong endHeight, CallOptions options = new CallOptions())
        {
            try
            {
                startHeight = startHeight >= 0 ? startHeight : 0;

                var response = await _client.GetEventsForHeightRangeAsync(
                    new GetEventsForHeightRangeRequest
                    {
                        Type = eventType,
                        StartHeight = startHeight,
                        EndHeight = endHeight
                    }, options);

                return response.ToFlowBlockEvents();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetEventsForHeightRange request failed.", exception);
            }
        }

        /// <summary>
        /// GetEventsForBlockIdsAsync retrieves events with the given type from the specified block Ids.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="blockIds"></param>
        /// <param name="options"></param>
        /// <returns>IEnumerable<FlowBlockEvent>.</returns>
        public async Task<IEnumerable<FlowBlockEvent>> GetEventsForBlockIdsAsync(string eventType, IEnumerable<ByteString> blockIds, CallOptions options = new CallOptions())
        {
            try
            {
                var request = new GetEventsForBlockIDsRequest
                {
                    Type = eventType
                };

                if (blockIds != null && blockIds.Count() > 0)
                {
                    foreach (var block in blockIds)
                        request.BlockIds.Add(block);
                }

                var response = await _client.GetEventsForBlockIDsAsync(request, options);
                return response.ToFlowBlockEvents();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetEventsForBlockIds request failed.", exception);
            }
        }

        /// <summary>
        /// SendTransactionAsync submits a transaction to the network.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="options"></param>
        /// <returns>FlowSendTransactionResponse.</returns>
        public async Task<FlowSendTransactionResponse> SendTransactionAsync(FlowTransaction transaction, CallOptions options = new CallOptions())
        {
            try
            {
                var tx = transaction.FromFlowTransaction();

                var response = await _client.SendTransactionAsync(
                    new SendTransactionRequest
                    {
                        Transaction = tx
                    }, options);

                return response.ToFlowSendTransactionResponse();
            }
            catch (Exception exception)
            {
                throw new FlowException("SendTransaction request failed.", exception);
            }
        }

        /// <summary>
        /// GetTransactionAsync gets a transaction by Id.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="options"></param>
        /// <returns>FlowTransactionResponse.</returns>
        public async Task<FlowTransactionResponse> GetTransactionAsync(ByteString transactionId, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetTransactionAsync(
                new GetTransactionRequest()
                {
                    Id = transactionId
                }, options);

                return response.ToFlowTransactionResponse();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetTransaction request failed.", exception);
            }
        }

        /// <summary>
        /// GetTransactionResultAsync gets the result of a transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="options"></param>
        /// <returns>FlowTransactionResult.</returns>
        public async Task<FlowTransactionResult> GetTransactionResultAsync(ByteString transactionId, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetTransactionResultAsync(
                new GetTransactionRequest
                {
                    Id = transactionId
                }, options);

                return response.ToFlowTransactionResult();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetTransactionResult request failed.", exception);
            }
        }

        /// <summary>
        /// GetCollectionByIdAsync gets a collection by Id.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="options"></param>
        /// <returns>FlowCollectionResponse.</returns>
        public async Task<FlowCollectionResponse> GetCollectionByIdAsync(ByteString collectionId, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetCollectionByIDAsync(
                new GetCollectionByIDRequest
                {
                    Id = collectionId
                }, options);

                return response.ToFlowCollectionResponse();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetCollectionById request failed.", exception);
            }
        }

        /// <summary>
        /// GetExecutionResultForBlockIdAsync retrieves execution result for the specified block Id.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>FlowExecutionResultForBlockIDResponse.</returns>
        public async Task<FlowExecutionResultForBlockIdResponse> GetExecutionResultForBlockIdAsync(ByteString blockId, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetExecutionResultForBlockIDAsync(
                new GetExecutionResultForBlockIDRequest
                {
                    BlockId = blockId
                }, options);

                return response.ToFlowExecutionResultForBlockIdResponse();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetExecutionResultForBlockId request failed.", exception);
            }
        }

        /// <summary>
        /// GetLatestProtocolStateSnapshotAsync retrieves the latest snapshot of the protocol state in serialized form.
        /// This is used to generate a root snapshot file used by Flow nodes to bootstrap their local protocol state database.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>FlowProtocolStateSnapshotResponse.</returns>
        public async Task<FlowProtocolStateSnapshotResponse> GetLatestProtocolStateSnapshotAsync(CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetLatestProtocolStateSnapshotAsync(new GetLatestProtocolStateSnapshotRequest(), options);

                return response.ToFlowProtocolStateSnapshotResponse();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetLatestProtocolStateSnapshot request failed.", exception);
            }
        }

        /// <summary>
        /// GetNetworkParametersAsync retrieves network parameters
        /// </summary>
        /// <param name="options"></param>
        /// <returns>FlowGetNetworkParametersResponse.</returns>
        public async Task<FlowGetNetworkParametersResponse> GetNetworkParametersAsync(CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetNetworkParametersAsync(new GetNetworkParametersRequest(), options);

                return response.ToFlowGetNetworkParametersResponse();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetLatestProtocolStateSnapshot request failed.", exception);
            }
        }

        /// <summary>
        /// GetAccountAtLatestBlockAsync gets an account by address at the latest sealed block.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="options"></param>
        /// <returns>FlowAccount.</returns>
        public async Task<FlowAccount> GetAccountAtLatestBlockAsync(ByteString address, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetAccountAtLatestBlockAsync(
                new GetAccountAtLatestBlockRequest
                {
                    Address = address
                }, options);

                return response.ToFlowAccount();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetAccountAtLatestBlock request failed.", exception);
            }            
        }

        /// <summary>
        /// GetAccountAtBlockHeightAsync gets an account by address at the given block height.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="blockHeight"></param>
        /// <param name="options"></param>
        /// <returns>FlowAccount.</returns>
        public async Task<FlowAccount> GetAccountAtBlockHeightAsync(ByteString address, ulong blockHeight, CallOptions options = new CallOptions())
        {
            try
            {
                var response = await _client.GetAccountAtBlockHeightAsync(
                new GetAccountAtBlockHeightRequest
                {
                    Address = address,
                    BlockHeight = blockHeight
                }, options);

                return response.ToFlowAccount();
            }
            catch(Exception exception)
            {
                throw new FlowException("GetAccountAtBlockHeight request failed.", exception);
            }            
        }

        /// <summary>
        /// WaitForSealAsync waits for transaction result status to be sealed.
        /// </summary>
        /// <param name="transactionResponse"></param>
        /// <returns>FlowTransactionResult.</returns>
        public async Task<FlowTransactionResult> WaitForSealAsync(FlowSendTransactionResponse transactionResponse, int delayMs = 1000, int timeoutMS = 30000)
        {
            var startTime = DateTime.UtcNow;
            while (true)
            {
                var result = await GetTransactionResultAsync(transactionResponse.Id);

                if (result != null && result.Status == Protos.entities.TransactionStatus.Sealed)
                    return result;

                if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > timeoutMS)
                    throw new FlowException("Timed out waiting for seal.");

                await Task.Delay(delayMs);
            }
        }

        /// <summary>
        /// ReadAccountFromConfigAsync retrieves the account from flow config file and creates signers where private keys exist
        /// </summary>
        /// <param name="options"></param>
        /// <returns>FlowAccount</returns>
        public async Task<FlowAccount> ReadAccountFromConfigAsync(string accountName, string configFileName = null, string configPath = null)
        {
            var config = Utilities.ReadConfig(configFileName, configPath);
            config.Accounts.TryGetValue(accountName, out FlowConfigAccount configAccount);

            if (configAccount == null)
                throw new FlowException($"Failed to find account \"{accountName}\"");

            var flowAccount = await GetAccountAtLatestBlockAsync(configAccount.Address.FromHexToByteString());

            if (!string.IsNullOrEmpty(configAccount.Key))
            {
                foreach (var key in flowAccount.Keys)
                {
                    // getting the public key so we can match it to our account public keys
                    var keyPair = Crypto.Ecdsa.Utilities.AsymmetricCipherKeyPairFromPrivateKey(configAccount.Key, key.SignatureAlgorithm);
                    var publicKey = Crypto.Ecdsa.Utilities.DecodePublicKeyToHex(keyPair);

                    // select the key with a matching public key
                    var flowAccountKey = flowAccount.Keys.Where(w => w.PublicKey == publicKey).FirstOrDefault();

                    if (flowAccountKey != null)
                    {
                        flowAccountKey.PrivateKey = configAccount.Key;

                        var privateKey = keyPair.Private as ECPrivateKeyParameters;
                        flowAccountKey.Signer = new Crypto.Ecdsa.Signer(privateKey, flowAccountKey.HashAlgorithm, flowAccountKey.SignatureAlgorithm);
                    }
                }
            }

            return flowAccount;
        }

        private void AddArgumentsToRequest(IEnumerable<ICadence> arguments, RepeatedField<ByteString> requestArguments)
        {
            if (arguments != null && arguments.Count() > 0)
            {
                foreach (var argument in arguments)
                    requestArguments.Add(argument.Encode().FromStringToByteString());
            }
        }
    }
}
