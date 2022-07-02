using Flow.Net.Sdk.Core.Exceptions;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Sdk.Core.Client
{
    public interface IFlowClient
    {
        /// <summary>
        /// Check if the access node is alive and healthy.
        /// </summary>
        /// <exception cref="FlowException"></exception>
        Task PingAsync();

        /// <summary>
        /// Gets the latest sealed or unsealed block header.
        /// </summary>
        /// <param name="isSealed"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowBlockHeader> GetLatestBlockHeaderAsync(bool isSealed = true);

        /// <summary>
        /// Gets a block header by Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowBlockHeader> GetBlockHeaderByIdAsync(string blockId);

        /// <summary>
        /// Gets a block header by height.
        /// </summary>
        /// <param name="height"></param>
        /// <returns><see cref="FlowBlockHeader"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowBlockHeader> GetBlockHeaderByHeightAsync(ulong height);

        /// <summary>
        /// Gets the full payload of the latest sealed or unsealed block.
        /// </summary>
        /// <param name="isSealed"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowBlock> GetLatestBlockAsync(bool isSealed = true);

        /// <summary>
        /// Gets a full block by Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowBlock> GetBlockByIdAsync(string blockId);

        /// <summary>
        /// Gets a full block by height.
        /// </summary>
        /// <param name="height"></param>
        /// <returns><see cref="FlowBlock"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowBlock> GetBlockByHeightAsync(ulong height);

        /// <summary>
        /// Gets a collection by Id.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns><see cref="FlowCollection"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowCollection> GetCollectionAsync(string collectionId);

        /// <summary>
        /// Submits a transaction to the network.
        /// </summary>
        /// <param name="flowTransaction"></param>
        /// <returns><see cref="FlowTransactionId"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowTransactionId> SendTransactionAsync(FlowTransaction flowTransaction);

        /// <summary>
        /// Gets a transaction by Id.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns><see cref="FlowTransactionResponse"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowTransactionResponse> GetTransactionAsync(string transactionId);

        /// <summary>
        /// Gets the result of a transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns><see cref="FlowTransactionResult"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowTransactionResult> GetTransactionResultAsync(string transactionId);

        /// <summary>
        /// Gets an account by address at the latest sealed block.
        /// </summary>
        /// <param name="address"></param>
        /// <returns><see cref="FlowAccount"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowAccount> GetAccountAtLatestBlockAsync(string address);

        /// <summary>
        /// Gets an account by address at the given block height.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="blockHeight"></param>
        /// <returns><see cref="FlowAccount"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowAccount> GetAccountAtBlockHeightAsync(string address, ulong blockHeight);

        /// <summary>
        /// Executes a read-only Cadence script against the latest sealed execution state.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<ICadence> ExecuteScriptAtLatestBlockAsync(FlowScript flowScript);

        /// <summary>
        /// Executes a ready-only Cadence script against the execution state at the block with the given Id.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="blockId"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<ICadence> ExecuteScriptAtBlockIdAsync(FlowScript flowScript, string blockId);

        /// <summary>
        /// Executes a ready-only Cadence script against the execution state at the given block height.
        /// </summary>
        /// <param name="flowScript"></param>
        /// <param name="blockHeight"></param>
        /// <returns><see cref="ICadence"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<ICadence> ExecuteScriptAtBlockHeightAsync(FlowScript flowScript, ulong blockHeight);

        /// <summary>
        /// Retrieves events for all sealed blocks between the start and end block heights (inclusive) with the given type.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="startHeight"></param>
        /// <param name="endHeight"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockEvent"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<IEnumerable<FlowBlockEvent>> GetEventsForHeightRangeAsync(string eventType, ulong startHeight, ulong endHeight);

        /// <summary>
        /// Retrieves events with the given type from the specified block Ids.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="blockIds"></param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="FlowBlockEvent"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<IEnumerable<FlowBlockEvent>> GetEventsForBlockIdsAsync(string eventType, IEnumerable<string> blockIds);

        /// <summary>
        /// Retrieves the latest snapshot of the protocol state in serialized form.
        /// This is used to generate a root snapshot file used by Flow nodes to bootstrap their local protocol state database.
        /// </summary>
        /// <returns><see cref="FlowProtocolStateSnapshot"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowProtocolStateSnapshot> GetLatestProtocolStateSnapshotAsync();

        /// <summary>
        /// Retrieves execution result for the specified block Id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns><see cref="FlowExecutionResult"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowExecutionResult> GetExecutionResultForBlockIdAsync(string blockId);

        /// <summary>
        /// Waits for transaction result status to be sealed.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="delayMs"></param>
        /// <param name="timeoutMs"></param>
        /// <returns><see cref="FlowTransactionResult"/></returns>
        /// <exception cref="FlowException"></exception>
        Task<FlowTransactionResult> WaitForSealAsync(string transactionId, int delayMs = 1000, int timeoutMs = 30000);
    }
}
