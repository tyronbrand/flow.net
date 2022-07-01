using Flow.Net.Sdk.Client.Http.Generated;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Exceptions;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flow.Net.Sdk.Client.Http
{
    public class FlowHttpClient : IFlowClient
    {
        private readonly FlowApiV1Generated _flowApiV1;

        public FlowHttpClient(HttpClient httpClient, string baseUrl)
        {
            _flowApiV1 = new FlowApiV1Generated(httpClient)
            {
                BaseUrl = baseUrl
            };
        }

        public async Task<ICadence> ExecuteScriptAtBlockHeightAsync(FlowScript flowScript, ulong blockHeight)
        {
            try
            {
                var converted = flowScript.FromFlowScript();
                var response = await _flowApiV1.ScriptsAsync(null, blockHeight.ToString(), converted).ConfigureAwait(false);
                return response.Value.Decode();
            }
            catch (Exception ex)
            {
                throw new FlowException("Execute script at block height error", ex);
            }
        }

        public async Task<ICadence> ExecuteScriptAtBlockIdAsync(FlowScript flowScript, string blockId)
        {
            try
            {
                var converted = flowScript.FromFlowScript();
                var response = await _flowApiV1.ScriptsAsync(blockId, null, converted).ConfigureAwait(false);
                return response.Value.Decode();
            }
            catch (Exception ex)
            {
                throw new FlowException("Execute script at block Id error", ex);
            }
        }

        public async Task<ICadence> ExecuteScriptAtLatestBlockAsync(FlowScript flowScript)
        {
            try
            {
                var converted = flowScript.FromFlowScript();
                var response = await _flowApiV1.ScriptsAsync(null, null, converted).ConfigureAwait(false);
                return response.Value.Decode();
            }
            catch (Exception ex)
            {
                throw new FlowException("Execute script at latest block error", ex);
            }
        }

        public async Task<FlowAccount> GetAccountAtBlockHeightAsync(string address, ulong blockHeight)
        {
            try
            {
                var response = await _flowApiV1.AccountsAsync(address, blockHeight.ToString(), new List<string> { "keys", "contracts" }).ConfigureAwait(false);
                return response.ToFlowAccount();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get account at block height error", ex);
            }
        }

        public async Task<FlowAccount> GetAccountAtLatestBlockAsync(string address)
        {
            try
            {
                var response = await _flowApiV1.AccountsAsync(address, "sealed", new List<string> { "keys", "contracts" }).ConfigureAwait(false);
                return response.ToFlowAccount();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get account at latest block error", ex);
            }
        }

        public async Task<FlowBlock> GetBlockByHeightAsync(ulong height)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(new List<string> { height.ToString() }, null, null, new List<string> { "payload" }, null).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block by height error", ex);
            }
        }

        public async Task<IEnumerable<FlowBlock>> GetBlocksByHeightAsync(IEnumerable<ulong> heights)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(heights.Select(s => s.ToString()).ToList(), null, null, new List<string> { "payload" }, null).ConfigureAwait(false);
                return response.ToFlowBlock();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block by height error", ex);
            }
        }

        public async Task<FlowBlock> GetBlockByIdAsync(string blockId)
        {
            try
            {
                var response = await _flowApiV1.BlocksAsync(new List<string> { blockId }, new List<string> { "payload" }, null).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block by Id error", ex);
            }
        }

        public async Task<IEnumerable<FlowBlock>> GetBlocksByIdAsync(IEnumerable<string> blockIds)
        {
            try
            {
                var response = await _flowApiV1.BlocksAsync(blockIds, new List<string> { "payload" }, null).ConfigureAwait(false);
                return response.ToFlowBlock();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block by Id error", ex);
            }
        }

        public async Task<FlowBlockHeader> GetBlockHeaderByHeightAsync(ulong height)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(new List<string> { height.ToString() }, null, null, null, null).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault()?.Header;
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block header by height error", ex);
            }
        }

        public async Task<IEnumerable<FlowBlockHeader>> GetBlockHeadersByHeightAsync(IEnumerable<ulong> heights)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(heights.Select(s => s.ToString()).ToList(), null, null, null, null).ConfigureAwait(false);                
                return response.ToFlowBlock().Select(s => s.Header).ToList();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block header by height error", ex);
            }
        }

        public async Task<FlowBlockHeader> GetBlockHeaderByIdAsync(string blockId)
        {
            try
            {
                var response = await _flowApiV1.BlocksAsync(new List<string> { blockId }, null, null).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault()?.Header;
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block header by Id error", ex);
            }
        }

        public async Task<IEnumerable<FlowBlockHeader>> GetBlockHeadersByIdAsync(IEnumerable<string> blockIds)
        {
            try
            {
                var response = await _flowApiV1.BlocksAsync(blockIds, null, null).ConfigureAwait(false);
                return response.ToFlowBlock().Select(s => s.Header).ToList();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get block header by Id error", ex);
            }
        }

        public async Task<FlowCollection> GetCollectionAsync(string collectionId)
        {
            try
            {
                var response = await _flowApiV1.CollectionsAsync(collectionId).ConfigureAwait(false);
                return response.ToFlowCollection();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get collection error", ex);
            }
        }

        public async Task<IEnumerable<FlowBlockEvent>> GetEventsForBlockIdsAsync(string eventType, IEnumerable<string> blockIds)
        {
            try
            {
                var response = await _flowApiV1.EventsAsync(eventType, null, null, blockIds, null).ConfigureAwait(false);
                return response.ToFlowBlockEvent();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get events for block Ids error", ex);
            }
        }

        public async Task<IEnumerable<FlowBlockEvent>> GetEventsForHeightRangeAsync(string eventType, ulong startHeight, ulong endHeight)
        {
            try
            {
                var response = await _flowApiV1.EventsAsync(eventType, startHeight.ToString(), endHeight.ToString(), null, null).ConfigureAwait(false);
                return response.ToFlowBlockEvent();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get events for height range error", ex);
            }
        }

        public async Task<FlowExecutionResult> GetExecutionResultForBlockIdAsync(string blockId)
        {
            try
            {
                var response = await _flowApiV1.ResultsAllAsync(new List<string> { blockId }).ConfigureAwait(false);
                return response.ToFlowExecutionResult().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get execution result for block Id error", ex);
            }
        }

        public async Task<FlowBlock> GetLatestBlockAsync(bool isSealed = true)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(new List<string> { isSealed ? "sealed" : "final" }, null, null, null, null).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get latest block error", ex);
            }
        }

        public async Task<FlowBlockHeader> GetLatestBlockHeaderAsync(bool isSealed = true)
        {
            try
            {
                var response = await _flowApiV1.BlocksAllAsync(new List<string> { isSealed ? "sealed" : "final" }, null, null, null, null).ConfigureAwait(false);
                return response.ToFlowBlock().FirstOrDefault()?.Header;
            }
            catch (Exception ex)
            {
                throw new FlowException("Get latest block header error", ex);
            }
        }

        public Task GetLatestProtocolStateSnapshotAsync()
        {
            throw new NotImplementedException("get latest protocol snapshot is currently not supported for HTTP API, if you require this functionality please use gRPC.");
        }

        public async Task<FlowTransactionResponse> GetTransactionAsync(string transactionId)
        {
            try
            {
                var response = await _flowApiV1.TransactionsAsync(transactionId, null, null).ConfigureAwait(false);
                return response.ToFlowTransactionResponse();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get transaction error", ex);
            }
        }

        public async Task<FlowTransactionResult> GetTransactionResultAsync(string transactionId)
        {
            try
            {
                var response = await _flowApiV1.TransactionsAsync(transactionId, new List<string> { "result" }, null).ConfigureAwait(false);
                return response.ToFlowTransactionResult();
            }
            catch (Exception ex)
            {
                throw new FlowException("Get transaction result error", ex);
            }
        }

        public async Task PingAsync()
        {
            try
            {
                await _flowApiV1.BlocksAllAsync(new List<string> { "sealed" }, "", "", null, null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new FlowException("Ping error", ex);
            }
        }

        public async Task<FlowTransactionId> SendTransactionAsync(FlowTransaction flowTransaction)
        {
            try
            {
                var converted = flowTransaction.FromFlowTransaction();
                var response = await _flowApiV1.SendTransactionAsync(converted).ConfigureAwait(false);
                return response.ToFlowTransactionId();
            }
            catch (Exception ex)
            {
                throw new FlowException("Send transaction error", ex);
            }
        }

        /// <summary>
        /// Waits for transaction result status to be sealed.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="delayMs"></param>
        /// <param name="timeoutMs"></param>
        /// <returns><see cref="FlowTransactionResult"/></returns>
        public async Task<FlowTransactionResult> WaitForSealAsync(string transactionId, int delayMs = 1000, int timeoutMs = 30000)
        {
            var startTime = DateTime.UtcNow;
            while (true)
            {
                var result = await GetTransactionResultAsync(transactionId);

                if (result != null && result.Status == Core.TransactionStatus.Sealed)
                    return result;

                if (DateTime.UtcNow.Subtract(startTime).TotalMilliseconds > timeoutMs)
                    throw new FlowException("Timed out waiting for seal.");

                await Task.Delay(delayMs);
            }
        }
    }
}
