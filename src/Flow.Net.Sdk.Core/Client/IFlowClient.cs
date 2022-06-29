using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Sdk.Core.Client
{
    public interface IFlowClient
    {
        Task PingAsync();
        Task<FlowBlockHeader> GetLatestBlockHeaderAsync(bool isSealed = true);
        Task<FlowBlockHeader> GetBlockHeaderByIdAsync(string blockId);
        Task<FlowBlockHeader> GetBlockHeaderByHeightAsync(ulong height);
        Task<FlowBlock> GetLatestBlockAsync(bool isSealed = true);
        Task<FlowBlock> GetBlockByIdAsync(string blockId);
        Task<FlowBlock> GetBlockByHeightAsync(ulong height);
        Task<FlowCollection> GetCollectionAsync(string collectionId);
        Task<FlowTransactionId> SendTransactionAsync(FlowTransaction flowTransaction);
        Task<FlowTransactionResponse> GetTransactionAsync(string transactionId);
        Task<FlowTransactionResult> GetTransactionResultAsync(string transactionId);
        Task<FlowAccount> GetAccountAtLatestBlockAsync(string address);
        Task<FlowAccount> GetAccountAtBlockHeightAsync(string address, ulong blockHeight);
        Task<ICadence> ExecuteScriptAtLatestBlockAsync(FlowScript flowScript);
        Task<ICadence> ExecuteScriptAtBlockIdAsync(FlowScript flowScript, string blockId);
        Task<ICadence> ExecuteScriptAtBlockHeightAsync(FlowScript flowScript, ulong blockHeight);
        Task<IEnumerable<FlowBlockEvent>> GetEventsForHeightRangeAsync(string eventType, ulong startHeight, ulong endHeight);
        Task<IEnumerable<FlowBlockEvent>> GetEventsForBlockIdsAsync(string eventType, IEnumerable<string> blockIds);
        Task GetLatestProtocolStateSnapshotAsync();
        Task<FlowExecutionResult> GetExecutionResultForBlockIdAsync(string blockId);
    }
}
