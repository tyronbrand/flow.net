using Flow.Net.Sdk.Client.Grpc.Models;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Exceptions;
using Flow.Net.Sdk.Core.Models;
using Flow.Net.Sdk.Protos.access;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using static Flow.Net.Sdk.Protos.access.AccessAPI;

namespace Flow.Net.Sdk.Client.Grpc
{
    public class FlowGrpcClient : IFlowClient
    {
        private readonly AccessAPIClient _client;
        private readonly CadenceConverter _cadenceConverter;

        /// <summary>
        /// A gRPC client for the Flow Access API.
        /// </summary>
        /// <param name="flowNetworkUrl"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowGrpcClient"/>.</returns>
        public FlowGrpcClient(string flowNetworkUrl, GrpcChannelOptions options = null)
        {
            options = options ?? new GrpcChannelOptions { Credentials = ChannelCredentials.Insecure };
            var networkUrlWithScheme = $"dns:{flowNetworkUrl}";

            try
            {
                _client = new AccessAPIClient(GrpcChannel.ForAddress(networkUrlWithScheme, options));
                _cadenceConverter = new CadenceConverter();
            }
            catch (Exception exception)
            {
                throw new FlowException($"Failed to connect to {flowNetworkUrl}", exception);
            }
        }

        public Task<ICadence> ExecuteScriptAtBlockHeightAsync(FlowScript flowScript, ulong blockHeight) => ExecuteScriptAtBlockHeightAsync(flowScript, blockHeight, new CallOptions());

        /// <summary>
        /// Executes a ready-only Cadence script against the execution state at the given block height.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="blockHeight"></param>
        /// <param name="options"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<ICadence> ExecuteScriptAtBlockHeightAsync(FlowScript flowScript, ulong blockHeight, CallOptions options)
        {
            try
            {
                var request = flowScript.FromFlowScript(blockHeight);
                var response = await _client.ExecuteScriptAtBlockHeightAsync(request, options);
                return response.Value.ByteStringToString().Decode(_cadenceConverter);
            }
            catch (Exception exception)
            {
                throw new FlowException("ExecuteScriptAtBlockHeight request failed.", exception);
            }
        }

        public Task<ICadence> ExecuteScriptAtBlockIdAsync(FlowScript flowScript, string blockId) => ExecuteScriptAtBlockIdAsync(flowScript, blockId, new CallOptions());

        /// <summary>
        /// Executes a ready-only Cadence script against the execution state at the block with the given Id.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="blockId"></param>
        /// <param name="options"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<ICadence> ExecuteScriptAtBlockIdAsync(FlowScript flowScript, string blockId, CallOptions options)
        {
            try
            {
                var request = flowScript.FromFlowScript(blockId);
                var response = await _client.ExecuteScriptAtBlockIDAsync(request, options);
                return response.Value.ByteStringToString().Decode(_cadenceConverter);
            }
            catch (Exception exception)
            {
                throw new FlowException("ExecuteScriptAtBlockId request failed.", exception);
            }
        }

        public Task<ICadence> ExecuteScriptAtLatestBlockAsync(FlowScript flowScript) => ExecuteScriptAtLatestBlockAsync(flowScript, new CallOptions());

        /// <summary>
        /// Executes a read-only Cadence script against the latest sealed execution state.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="options"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<ICadence> ExecuteScriptAtLatestBlockAsync(FlowScript flowScript, CallOptions options)
        {
            try
            {
                var request = flowScript.FromFlowScript();
                var response = await _client.ExecuteScriptAtLatestBlockAsync(request, options);
                return response.Value.ByteStringToString().Decode(_cadenceConverter);
            }
            catch (Exception exception)
            {
                throw new FlowException("ExecuteScriptAtLatestBlock request failed.", exception);
            }
        }

        public Task<FlowAccount> GetAccountAtBlockHeightAsync(string address, ulong blockHeight) => GetAccountAtBlockHeightAsync(address, blockHeight, new CallOptions());

        /// <summary>
        /// Gets an account by address at the given block height.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="blockHeight"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowAccount"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowAccount> GetAccountAtBlockHeightAsync(string address, ulong blockHeight, CallOptions options)
        {
            try
            {
                var response = await _client.GetAccountAtBlockHeightAsync(
                new GetAccountAtBlockHeightRequest
                {
                    Address = address.HexToByteString(),
                    BlockHeight = blockHeight
                }, options);

                return response.ToFlowAccount();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetAccountAtBlockHeight request failed.", exception);
            }
        }

        public Task<FlowAccount> GetAccountAtLatestBlockAsync(string address) => GetAccountAtLatestBlockAsync(address, new CallOptions());

        /// <summary>
        /// Gets an account by address at the latest sealed block.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowAccount"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowAccount> GetAccountAtLatestBlockAsync(string address, CallOptions options)
        {
            try
            {
                var response = await _client.GetAccountAtLatestBlockAsync(
                new GetAccountAtLatestBlockRequest
                {
                    Address = address.HexToByteString(),
                }, options);

                return response.ToFlowAccount();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetAccountAtLatestBlock request failed.", exception);
            }
        }

        public Task<FlowBlock> GetBlockByHeightAsync(ulong height) => GetBlockByHeightAsync(height, new CallOptions());

        /// <summary>
        /// Gets a full block by height.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlock> GetBlockByHeightAsync(ulong height, CallOptions options)
        {
            try
            {
                var response = await _client.GetBlockByHeightAsync(
                    new GetBlockByHeightRequest
                    {
                        Height = height
                    }, options);

                return response.ToFlowBlock();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetBlockByHeight request failed.", exception);
            }
        }

        public Task<FlowBlock> GetBlockByIdAsync(string blockId) => GetBlockByIdAsync(blockId, new CallOptions());

        /// <summary>
        /// Gets a full block by Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlock> GetBlockByIdAsync(string blockId, CallOptions options)
        {
            try
            {
                var response = await _client.GetBlockByIDAsync(
                    new GetBlockByIDRequest
                    {
                        Id = blockId.HexToByteString(),
                    }, options);

                return response.ToFlowBlock();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetBlockById request failed.", exception);
            }
        }

        public Task<FlowBlockHeader> GetBlockHeaderByHeightAsync(ulong height) => GetBlockHeaderByHeightAsync(height, new CallOptions());

        /// <summary>
        /// Gets a block header by height.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlockHeader> GetBlockHeaderByHeightAsync(ulong height, CallOptions options)
        {
            try
            {
                var response = await _client.GetBlockHeaderByHeightAsync(
                    new GetBlockHeaderByHeightRequest
                    {
                        Height = height
                    }, options);

                return response.ToFlowBlockHeader();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetBlockHeaderByHeight request failed.", exception);
            }
        }

        public Task<FlowBlockHeader> GetBlockHeaderByIdAsync(string blockId) => GetBlockHeaderByIdAsync(blockId, new CallOptions());

        /// <summary>
        /// Gets a block header by Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlockHeader> GetBlockHeaderByIdAsync(string blockId, CallOptions options)
        {
            try
            {
                var response = await _client.GetBlockHeaderByIDAsync(
                new GetBlockHeaderByIDRequest
                {
                    Id = blockId.HexToByteString()
                }, options);

                return response.ToFlowBlockHeader();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetBlockHeaderById request failed.", exception);
            }
        }

        public Task<FlowCollection> GetCollectionAsync(string collectionId) => GetCollectionAsync(collectionId, new CallOptions());

        /// <summary>
        /// Gets a collection by Id.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowCollection"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowCollection> GetCollectionAsync(string collectionId, CallOptions options)
        {
            try
            {
                var response = await _client.GetCollectionByIDAsync(
                new GetCollectionByIDRequest
                {
                    Id = collectionId.HexToByteString()
                }, options);

                return response.ToFlowCollection();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetCollectionById request failed.", exception);
            }
        }

        public Task<IEnumerable<FlowBlockEvent>> GetEventsForBlockIdsAsync(string eventType, IEnumerable<string> blockIds) => GetEventsForBlockIdsAsync(eventType, blockIds, new CallOptions());

        /// <summary>
        /// Retrieves events with the given type from the specified block Ids.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="blockIds"></param>
        /// <param name="options"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockEvent"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<IEnumerable<FlowBlockEvent>> GetEventsForBlockIdsAsync(string eventType, IEnumerable<string> blockIds, CallOptions options)
        {
            try
            {
                var request = new GetEventsForBlockIDsRequest
                {
                    Type = eventType
                };

                if (blockIds != null)
                {
                    foreach (var block in blockIds)
                        request.BlockIds.Add(block.HexToByteString());
                }

                var response = await _client.GetEventsForBlockIDsAsync(request, options);
                return response.ToFlowBlockEvents();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetEventsForBlockIds request failed.", exception);
            }
        }

        public Task<IEnumerable<FlowBlockEvent>> GetEventsForHeightRangeAsync(string eventType, ulong startHeight, ulong endHeight) => GetEventsForHeightRangeAsync(eventType, startHeight, endHeight, new CallOptions());

        /// <summary>
        /// Retrieves events for all sealed blocks between the start and end block heights (inclusive) with the given type.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="startHeight"></param>
        /// <param name="endHeight"></param>
        /// <param name="options"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockEvent"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<IEnumerable<FlowBlockEvent>> GetEventsForHeightRangeAsync(string eventType, ulong startHeight, ulong endHeight, CallOptions options)
        {
            try
            {
                startHeight = startHeight > 0 ? startHeight : 0;

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

        public Task<FlowExecutionResult> GetExecutionResultForBlockIdAsync(string blockId) => GetExecutionResultForBlockIdAsync(blockId, new CallOptions());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowExecutionResult> GetExecutionResultForBlockIdAsync(string blockId, CallOptions options)
        {
            try
            {
                var response = await _client.GetExecutionResultForBlockIDAsync(
                    new GetExecutionResultForBlockIDRequest
                    {
                        BlockId = blockId.HexToByteString()
                    }, options);

                return response.ToFlowExecutionResult();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetExecutionResultForBlockId request failed.", exception);
            }
        }

        public Task<FlowBlock> GetLatestBlockAsync(bool isSealed = true) => GetLatestBlockAsync(isSealed, new CallOptions());

        /// <summary>
        /// Gets the full payload of the latest sealed or unsealed block.
        /// </summary>
        /// <param name="isSealed"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlock> GetLatestBlockAsync(bool isSealed, CallOptions options)
        {
            try
            {
                var response = await _client.GetLatestBlockAsync(
                    new GetLatestBlockRequest
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

        public Task<FlowBlockHeader> GetLatestBlockHeaderAsync(bool isSealed = true) => GetLatestBlockHeaderAsync(isSealed, new CallOptions());

        /// <summary>
        /// Gets the latest sealed or unsealed block header.
        /// </summary>
        /// <param name="isSealed"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlockHeader> GetLatestBlockHeaderAsync(bool isSealed, CallOptions options)
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

        public Task<FlowProtocolStateSnapshot> GetLatestProtocolStateSnapshotAsync() => GetLatestProtocolStateSnapshotAsync(new CallOptions());

        /// <summary>
        /// Retrieves the latest snapshot of the protocol state in serialized form.
        /// This is used to generate a root snapshot file used by Flow nodes to bootstrap their local protocol state database.
        /// </summary>
        /// <param name="options"></param>
        /// <returns><see cref="FlowProtocolStateSnapshot"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowProtocolStateSnapshot> GetLatestProtocolStateSnapshotAsync(CallOptions options)
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

        public Task<FlowTransactionResponse> GetTransactionAsync(string transactionId) => GetTransactionAsync(transactionId, new CallOptions());

        /// <summary>
        /// Gets a transaction by Id.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowTransactionResponse"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowTransactionResponse> GetTransactionAsync(string transactionId, CallOptions options)
        {
            try
            {
                var response = await _client.GetTransactionAsync(
                    new GetTransactionRequest
                    {
                        Id = transactionId.HexToByteString()
                    }, options);

                return response.ToFlowTransactionResponse();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetTransaction request failed.", exception);
            }
        }

        public Task<FlowTransactionResult> GetTransactionResultAsync(string transactionId) => GetTransactionResultAsync(transactionId, new CallOptions());

        /// <summary>
        /// Gets the result of a transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="options"></param>
        /// <returns><see cref="FlowTransactionResult"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowTransactionResult> GetTransactionResultAsync(string transactionId, CallOptions options)
        {
            try
            {
                var response = await _client.GetTransactionResultAsync(
                new GetTransactionRequest
                {
                    Id = transactionId.HexToByteString(),
                }, options);

                return response.ToFlowTransactionResult();
            }
            catch (Exception exception)
            {
                throw new FlowException("GetTransactionResult request failed.", exception);
            }
        }

        public Task PingAsync() => PingAsync(new CallOptions());

        /// <summary>
        /// Check if the access node is alive and healthy.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="FlowException"></exception>
        public async Task PingAsync(CallOptions options)
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

        public Task<FlowTransactionId> SendTransactionAsync(FlowTransaction flowTransaction) => SendTransactionAsync(flowTransaction, new CallOptions());

        public async Task<FlowTransactionId> SendTransactionAsync(FlowTransaction flowTransaction, CallOptions options)
        {
            try
            {
                var tx = flowTransaction.FromFlowTransaction();

                var response = await _client.SendTransactionAsync(
                    new SendTransactionRequest
                    {
                        Transaction = tx
                    }, options);

                return response.ToFlowTransactionId();
            }
            catch (Exception exception)
            {
                throw new FlowException("SendTransaction request failed.", exception);
            }
        }

        public Task<FlowTransactionResult> WaitForSealAsync(string transactionId, int delayMs = 1000, int timeoutMs = 30000) => WaitForSealAsync(transactionId, delayMs, timeoutMs, new CallOptions(), CancellationToken.None);

        /// <summary>
        /// Waits for transaction result status to be sealed.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="delayMs"></param>
        /// <param name="timeoutMs"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowTransactionResult"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowTransactionResult> WaitForSealAsync(string transactionId, int delayMs, int timeoutMs, CallOptions options, CancellationToken cancellationToken)
        {
            var startTime = DateTime.UtcNow;
            while (true)
            {
                var result = await GetTransactionResultAsync(transactionId, options);

                if (result != null && result.Status == Core.TransactionStatus.Sealed)
                    return result;

                if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > timeoutMs)
                    throw new FlowException("Timed out waiting for seal.");

                await Task.Delay(delayMs, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieves network parameters
        /// </summary>
        /// <param name="options"></param>
        /// <returns><see cref="FlowNetworkParameters"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowNetworkParameters> GetNetworkParametersAsync(CallOptions options = new CallOptions())
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
    }
}