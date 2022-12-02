using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Exceptions;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Flow.Net.Sdk.Client.Http
{
    public class FlowHttpClient : IFlowClient
    {
        private readonly FlowApiV1 _flowApiV1;

        /// <summary>
        /// A HTTP client for the Flow v1 API.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="clientOptions"></param>
        public FlowHttpClient(HttpClient httpClient, FlowClientOptions clientOptions)
        {
            _flowApiV1 = new FlowApiV1(httpClient)
            {
                BaseUrl = clientOptions.ServerUrl
            };
        }
        
        public Task<ICadence> ExecuteScriptAtBlockHeightAsync(FlowScript flowScript, ulong blockHeight) => ExecuteScriptAtBlockHeightAsync(flowScript, blockHeight, CancellationToken.None);

        /// <summary>
        /// Executes a ready-only Cadence script against the execution state at the given block height.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="blockHeight"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<ICadence> ExecuteScriptAtBlockHeightAsync(FlowScript flowScript, ulong blockHeight, CancellationToken cancellationToken)
        {
            try
            {
                var converted = flowScript.FromFlowScript();
                var response = await _flowApiV1.ScriptsAsync(null, blockHeight.ToString(), converted, cancellationToken).ConfigureAwait(false);
                return response.Value.Decode();
            }
            catch (Exception ex)
            {
                throw new FlowException("Execute script at block height error", ex);
            }
        }
       
        public Task<ICadence> ExecuteScriptAtBlockIdAsync(FlowScript flowScript, string blockId) => ExecuteScriptAtBlockIdAsync(flowScript, blockId, CancellationToken.None);

        /// <summary>
        /// Executes a ready-only Cadence script against the execution state at the block with the given Id.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="blockId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<ICadence> ExecuteScriptAtBlockIdAsync(FlowScript flowScript, string blockId, CancellationToken cancellationToken)
        {
            try
            {
                var converted = flowScript.FromFlowScript();
                var response = await _flowApiV1.ScriptsAsync(blockId, null, converted, cancellationToken).ConfigureAwait(false);
                return response.Value.Decode();
            }
            catch (Exception ex)
            {
                throw new FlowException("Execute script at block Id error", ex);
            }
        }

        public Task<ICadence> ExecuteScriptAtLatestBlockAsync(FlowScript flowScript) => ExecuteScriptAtLatestBlockAsync(flowScript, CancellationToken.None);

        /// <summary>
        /// Executes a read-only Cadence script against the latest sealed execution state.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<ICadence> ExecuteScriptAtLatestBlockAsync(FlowScript flowScript, CancellationToken cancellationToken)
        {
            try
            {
                var converted = flowScript.FromFlowScript();
                var response = await _flowApiV1.ScriptsAsync(null, null, converted, cancellationToken).ConfigureAwait(false);
                return response.Value.Decode();
            }
            catch (Exception ex)
            {
                throw new FlowException("Execute script at latest block error", ex);
            }
        }
        
        public Task<FlowAccount> GetAccountAtBlockHeightAsync(string address, ulong blockHeight) => GetAccountAtBlockHeightAsync(address, blockHeight, CancellationToken.None);

        /// <summary>
        /// Gets an account by address at the given block height.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="blockHeight"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowAccount"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowAccount> GetAccountAtBlockHeightAsync(string address, ulong blockHeight, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.AccountsAsync(address, blockHeight.ToString(), new List<string> { "keys", "contracts" }, cancellationToken).ConfigureAwait(false);
                return response.ToFlowAccount();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get account at block height error", ex);
            }
        }

        public Task<FlowAccount> GetAccountAtLatestBlockAsync(string address) => GetAccountAtLatestBlockAsync(address, CancellationToken.None);

        /// <summary>
        /// Gets an account by address at the latest sealed block.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowAccount"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowAccount> GetAccountAtLatestBlockAsync(string address, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.AccountsAsync(address, "sealed", new List<string> { "contracts", "keys" }, cancellationToken).ConfigureAwait(false);
                return response.ToFlowAccount();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get account at latest block error", ex);
            }
        }

        public  Task<FlowBlock> GetBlockByHeightAsync(ulong height) => GetBlockByHeightAsync(height, CancellationToken.None);

        /// <summary>
        /// Gets a full block by height.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlock> GetBlockByHeightAsync(ulong height, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(new List<string> { height.ToString() }, null, null, new List<string> { "payload" }, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block by height error", ex);
            }
        }

        /// <summary>
        /// Gets full blocks by heights.
        /// </summary>
        /// <param name="heights"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public Task<IEnumerable<FlowBlock>> GetBlocksByHeightAsync(IEnumerable<ulong> heights) => GetBlocksByHeightAsync(heights, CancellationToken.None);

        /// <summary>
        /// Gets full blocks by heights.
        /// </summary>
        /// <param name="heights"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<IEnumerable<FlowBlock>> GetBlocksByHeightAsync(IEnumerable<ulong> heights, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(heights.Select(s => s.ToString()).ToList(), null, null, new List<string> { "payload" }, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block by height error", ex);
            }
        }

        public Task<FlowBlock> GetBlockByIdAsync(string blockId) => GetBlockByIdAsync(blockId, CancellationToken.None);

        /// <summary>
        /// Gets a full block by Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlock> GetBlockByIdAsync(string blockId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.BlocksAsync(new List<string> { blockId }, new List<string> { "payload" }, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block by Id error", ex);
            }
        }

        /// <summary>
        /// Gets full blocks by Ids.
        /// </summary>
        /// <param name="blockIds"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public Task<IEnumerable<FlowBlock>> GetBlocksByIdAsync(IEnumerable<string> blockIds) => GetBlocksByIdAsync(blockIds, CancellationToken.None);

        /// <summary>
        /// Gets full blocks by Ids.
        /// </summary>
        /// <param name="blockIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<IEnumerable<FlowBlock>> GetBlocksByIdAsync(IEnumerable<string> blockIds, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.BlocksAsync(blockIds, new List<string> { "payload" }, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block by Id error", ex);
            }
        }

        public Task<FlowBlockHeader> GetBlockHeaderByHeightAsync(ulong height) => GetBlockHeaderByHeightAsync(height, CancellationToken.None);

        /// <summary>
        /// Gets a block header by height.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlockHeader> GetBlockHeaderByHeightAsync(ulong height, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(new List<string> { height.ToString() }, null, null, null, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault()?.Header;
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block header by height error", ex);
            }
        }

        /// <summary>
        /// Gets a block headers by heights.
        /// </summary>
        /// <param name="heights"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public Task<IEnumerable<FlowBlockHeader>> GetBlockHeadersByHeightAsync(IEnumerable<ulong> heights) => GetBlockHeadersByHeightAsync(heights, CancellationToken.None);

        /// <summary>
        /// Gets a block headers by heights.
        /// </summary>
        /// <param name="heights"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<IEnumerable<FlowBlockHeader>> GetBlockHeadersByHeightAsync(IEnumerable<ulong> heights, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(heights.Select(s => s.ToString()).ToList(), null, null, null, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock().Select(s => s.Header).ToList();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block header by height error", ex);
            }
        }

        public Task<FlowBlockHeader> GetBlockHeaderByIdAsync(string blockId) => GetBlockHeaderByIdAsync(blockId, CancellationToken.None);

        /// <summary>
        /// Gets a block header by Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlockHeader> GetBlockHeaderByIdAsync(string blockId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.BlocksAsync(new List<string> { blockId }, null, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault()?.Header;
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block header by Id error", ex);
            }
        }

        /// <summary>
        /// Gets block headers by Ids.
        /// </summary>
        /// <param name="blockIds"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public Task<IEnumerable<FlowBlockHeader>> GetBlockHeadersByIdAsync(IEnumerable<string> blockIds) => GetBlockHeadersByIdAsync(blockIds, CancellationToken.None);

        /// <summary>
        /// Gets block headers by Ids.
        /// </summary>
        /// <param name="blockIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<IEnumerable<FlowBlockHeader>> GetBlockHeadersByIdAsync(IEnumerable<string> blockIds, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.BlocksAsync(blockIds, null, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock().Select(s => s.Header).ToList();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block header by Id error", ex);
            }
        }

        public Task<FlowCollection> GetCollectionAsync(string collectionId) => GetCollectionAsync(collectionId, CancellationToken.None);

        /// <summary>
        /// Gets a collection by Id.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowCollection"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowCollection> GetCollectionAsync(string collectionId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.CollectionsAsync(collectionId, new List<string> { "transactions" }, cancellationToken).ConfigureAwait(false);
                return response.ToFlowCollection();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get collection error", ex);
            }
        }

        public Task<IEnumerable<FlowBlockEvent>> GetEventsForBlockIdsAsync(string eventType, IEnumerable<string> blockIds) => GetEventsForBlockIdsAsync(eventType, blockIds, CancellationToken.None);

        /// <summary>
        /// Retrieves events with the given type from the specified block Ids.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="blockIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockEvent"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<IEnumerable<FlowBlockEvent>> GetEventsForBlockIdsAsync(string eventType, IEnumerable<string> blockIds, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.EventsAsync(eventType, null, null, blockIds, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlockEvent();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get events for block Ids error", ex);
            }
        }

        public Task<IEnumerable<FlowBlockEvent>> GetEventsForHeightRangeAsync(string eventType, ulong startHeight, ulong endHeight) => GetEventsForHeightRangeAsync(eventType, startHeight, endHeight, CancellationToken.None);

        /// <summary>
        /// Retrieves events for all sealed blocks between the start and end block heights (inclusive) with the given type.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="startHeight"></param>
        /// <param name="endHeight"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockEvent"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<IEnumerable<FlowBlockEvent>> GetEventsForHeightRangeAsync(string eventType, ulong startHeight, ulong endHeight, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.EventsAsync(eventType, startHeight.ToString(), endHeight.ToString(), null, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlockEvent();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get events for height range error", ex);
            }
        }

        public Task<FlowExecutionResult> GetExecutionResultForBlockIdAsync(string blockId) => GetExecutionResultForBlockIdAsync(blockId, CancellationToken.None);

        /// <summary>
        /// Retrieves execution result for the specified block Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowExecutionResult"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowExecutionResult> GetExecutionResultForBlockIdAsync(string blockId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.ResultsAllAsync(new List<string> { blockId }, cancellationToken).ConfigureAwait(false);
                return response.ToFlowExecutionResult().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get execution result for block Id error", ex);
            }
        }

        public Task<FlowBlock> GetLatestBlockAsync(bool isSealed = true) => GetLatestBlockAsync(CancellationToken.None, isSealed);

        /// <summary>
        /// Gets the full payload of the latest sealed or unsealed block.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="isSealed"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlock> GetLatestBlockAsync(CancellationToken cancellationToken, bool isSealed = true)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(new List<string> { isSealed ? "sealed" : "final" }, null, null, null, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get latest block error", ex);
            }
        }

        public Task<FlowBlockHeader> GetLatestBlockHeaderAsync(bool isSealed = true) => GetLatestBlockHeaderAsync(CancellationToken.None, isSealed);

        /// <summary>
        /// Gets the latest sealed or unsealed block header.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="isSealed"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowBlockHeader> GetLatestBlockHeaderAsync(CancellationToken cancellationToken, bool isSealed = true)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(new List<string> { isSealed ? "sealed" : "final" }, null, null, null, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault()?.Header;
            }
            catch (Exception ex)
            {
                throw new FlowException("Get latest block header error", ex);
            }
        }

        public Task<FlowProtocolStateSnapshot> GetLatestProtocolStateSnapshotAsync() => throw new NotImplementedException("get latest protocol snapshot is currently not supported for HTTP API, if you require this functionality please use gRPC.");

        public Task<FlowTransactionResponse> GetTransactionAsync(string transactionId) => GetTransactionAsync(transactionId, CancellationToken.None);

        /// <summary>
        /// Gets a transaction by Id.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowTransactionResponse"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowTransactionResponse> GetTransactionAsync(string transactionId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.TransactionsAsync(transactionId, new List<string> { "result" }, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowTransactionResponse();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get transaction error", ex);
            }
        }

        public Task<FlowTransactionResult> GetTransactionResultAsync(string transactionId) => GetTransactionResultAsync(transactionId, CancellationToken.None);

        /// <summary>
        /// Gets the result of a transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowTransactionResult"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowTransactionResult> GetTransactionResultAsync(string transactionId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _flowApiV1.TransactionsAsync(transactionId, new List<string> { "result" }, null, cancellationToken).ConfigureAwait(false);
                return response.ToFlowTransactionResult();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get transaction result error", ex);
            }
        }

        public Task PingAsync() => PingAsync(CancellationToken.None);

        /// <summary>
        /// Check if the access node is alive and healthy.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <exception cref="FlowException"></exception>
        public async Task PingAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _flowApiV1.BlocksAllAsync(new List<string> { "sealed" }, "", "", null, null, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new FlowException("Ping error", ex);
            }
        }

        public Task<FlowTransactionId> SendTransactionAsync(FlowTransaction flowTransaction) => SendTransactionAsync(flowTransaction, CancellationToken.None);

        /// <summary>
        /// Submits a transaction to the network.
        /// </summary>
        /// <param name="flowTransaction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="FlowTransactionId"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowTransactionId> SendTransactionAsync(FlowTransaction flowTransaction, CancellationToken cancellationToken)
        {
            try
            {
                var converted = flowTransaction.FromFlowTransaction();
                var response = await _flowApiV1.SendTransactionAsync(converted, cancellationToken).ConfigureAwait(false);
                return response.ToFlowTransactionId();
            }
            catch (Exception ex)
            {
                throw new FlowException("Send transaction error", ex);
            }
        }
        
        public Task<FlowTransactionResult> WaitForSealAsync(string transactionId, int delayMs = 1000, int timeoutMs = 30000) => WaitForSealAsync(transactionId, CancellationToken.None, delayMs, timeoutMs);

        /// <summary>
        /// Waits for transaction result status to be sealed.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="delayMs"></param>
        /// <param name="timeoutMs"></param>
        /// <returns><see cref="FlowTransactionResult"/></returns>
        /// <exception cref="FlowException"></exception>
        public async Task<FlowTransactionResult> WaitForSealAsync(string transactionId, CancellationToken cancellationToken, int delayMs = 1000, int timeoutMs = 30000)
        {
            var startTime = DateTime.UtcNow;
            while (true)
            {
                FlowTransactionResult result = null;
                try
                {
                    result = await GetTransactionResultAsync(transactionId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception) { }

                if (result != null && result.Status == Core.TransactionStatus.Sealed)
                    return result;

                if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > timeoutMs)
                    throw new FlowException("Timed out waiting for seal.");

                await Task.Delay(delayMs, cancellationToken);
            }
        }
    }
}
