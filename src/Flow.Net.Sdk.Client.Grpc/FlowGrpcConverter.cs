using Flow.Net.Sdk.Client.Grpc.Models;
using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using Flow.Net.Sdk.Protos.access;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Client.Grpc
{
    public static class FlowGrpcConverter
    {
        public static FlowNetworkParameters ToFlowGetNetworkParametersResponse(this GetNetworkParametersResponse getNetworkParametersResponse)
        {
            return new FlowNetworkParameters
            {
                ChainId = getNetworkParametersResponse.ChainId
            };
        }

        public static FlowProtocolStateSnapshot ToFlowProtocolStateSnapshotResponse(this ProtocolStateSnapshotResponse protocolStateSnapshotResponse)
        {
            return new FlowProtocolStateSnapshot
            {
                SerializedSnapshot = protocolStateSnapshotResponse.SerializedSnapshot.ByteStringToString()
            };
        }

        public static FlowExecutionResult ToFlowExecutionResult(this ExecutionResultForBlockIDResponse executionResultForBlockIdResponse)
        {
            var flowChunks = executionResultForBlockIdResponse.ExecutionResult.Chunks.Select(chunk =>
                new FlowChunk
                {
                    BlockId = chunk.BlockId.ByteStringToHex(),
                    EndState = chunk.EndState.ToByteArray(),
                    EventCollection = chunk.EventCollection.ToByteArray(),
                    Index = chunk.Index,
                    NumberOfTransactions = chunk.NumberOfTransactions,
                    StartState = chunk.StartState.ToByteArray(),
                    TotalComputationUsed = chunk.TotalComputationUsed
                }).ToList();

            var serviceEvents = executionResultForBlockIdResponse.ExecutionResult.ServiceEvents.Select(serviceEvent =>
                new FlowServiceEvent
                {
                    Payload = serviceEvent.Payload.ToByteArray(),
                    Type = serviceEvent.Type
                }).ToList();

            return new FlowExecutionResult
            {
                BlockId = executionResultForBlockIdResponse.ExecutionResult.BlockId.ByteStringToHex(),
                PreviousResultId = executionResultForBlockIdResponse.ExecutionResult.PreviousResultId.ByteStringToHex(),
                Chunks = flowChunks,
                ServiceEvents = serviceEvents
            };
        }

        public static FlowCollection ToFlowCollection(this CollectionResponse collectionResponse)
        {
            return new FlowCollection
            {
                Id = collectionResponse.Collection.Id.ByteStringToHex(),
                TransactionIds = collectionResponse.Collection.TransactionIds.Select(s =>
                    new FlowTransactionId
                    {
                        Id = s.ByteStringToHex()
                    }).ToList()
            };
        }

        public static IEnumerable<FlowTransactionResult> ToFlowTransactionsResult(this TransactionResultsResponse transactionResultsResponse)
        {
            var result = new List<FlowTransactionResult>();

            foreach(var item in transactionResultsResponse.TransactionResults)
                result.Add(item.ToFlowTransactionResult());            

            return result;
        }

        public static FlowTransactionResult ToFlowTransactionResult(this TransactionResultResponse transactionResultResponse)
        {
            var events = transactionResultResponse.Events.Select(@event => @event.ToFlowEvent()).ToList();

            return new FlowTransactionResult
            {
                BlockId = transactionResultResponse.BlockId.ByteStringToHex(),
                ErrorMessage = transactionResultResponse.ErrorMessage,
                Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), transactionResultResponse.Status.ToString()),
                StatusCode = transactionResultResponse.StatusCode,
                Events = events,
                BlockHeight = transactionResultResponse.BlockHeight,
                CollectionId = transactionResultResponse.CollectionId.ByteStringToHex(),
                TransactionId = transactionResultResponse.TransactionId.ByteStringToHex(),
            };
        }

        public static FlowTransactionResponse ToFlowTransactionResponse(this TransactionResponse transactionResponse)
        {
            var payloadSignatures = transactionResponse.Transaction.PayloadSignatures.Select(payloadSignature =>
                new FlowSignature
                {
                    Address = new FlowAddress(payloadSignature.Address.ByteStringToHex()),
                    KeyId = payloadSignature.KeyId,
                    Signature = payloadSignature.Signature_.ToByteArray()
                }).ToList();

            var envelopeSignatures = transactionResponse.Transaction.EnvelopeSignatures.Select(envelopeSignature =>
                new FlowSignature
                {
                    Address = new FlowAddress(envelopeSignature.Address.ByteStringToHex()),
                    KeyId = envelopeSignature.KeyId,
                    Signature = envelopeSignature.Signature_.ToByteArray()
                }).ToList();

            var sendResponse = new FlowTransactionResponse
            {
                Script = transactionResponse.Transaction.Script.ByteStringToString(),
                ReferenceBlockId = transactionResponse.Transaction.ReferenceBlockId.ByteStringToHex(),
                GasLimit = transactionResponse.Transaction.GasLimit,
                Payer = new FlowAddress(transactionResponse.Transaction.Payer.ByteStringToHex()),
                Authorizers = transactionResponse.Transaction.Authorizers.Select(s => new FlowAddress(s.ByteStringToHex())).ToList(),
                ProposalKey = new FlowProposalKey
                {
                    Address = new FlowAddress(transactionResponse.Transaction.ProposalKey.Address.ByteStringToHex()),
                    KeyId = transactionResponse.Transaction.ProposalKey.KeyId,
                    SequenceNumber = transactionResponse.Transaction.ProposalKey.SequenceNumber
                },
                PayloadSignatures = payloadSignatures,
                EnvelopeSignatures = envelopeSignatures
            };

            foreach (var argument in transactionResponse.Transaction.Arguments)
                sendResponse.Arguments.Add(argument.ByteStringToString().Decode());

            return sendResponse;
        }

        public static FlowTransactionId ToFlowTransactionId(this SendTransactionResponse sendTransactionResponse)
        {
            return new FlowTransactionId
            {
                Id = sendTransactionResponse.Id.ByteStringToHex()
            };
        }

        public static FlowBlockHeader ToFlowBlockHeader(this BlockHeaderResponse blockHeaderResponse)
        {
            return new FlowBlockHeader
            {
                Height = blockHeaderResponse.Block.Height,
                Id = blockHeaderResponse.Block.Id.ByteStringToHex(),
                ParentId = blockHeaderResponse.Block.ParentId.ByteStringToHex(),
                Timestamp = blockHeaderResponse.Block.Timestamp.ToDateTimeOffset(),
                ParentVoterSignature = blockHeaderResponse.Block.ParentVoterIndices.ToArray()
            };
        }

        public static FlowBlock ToFlowBlock(this BlockResponse blockResponse)
        {
            var blockSeals = new List<FlowBlockSeal>();
            foreach (var blockSeal in blockResponse.Block.BlockSeals)
            {
                blockSeals.Add(
                    new FlowBlockSeal
                    {
                        BlockId = blockSeal.BlockId.ByteStringToHex(),
                        ResultId = blockSeal.ExecutionReceiptId.ByteStringToHex()
                    });
            }

            var blockCollectionGuarantees = new List<FlowCollectionGuarantee>();
            foreach (var collectionGuarantee in blockResponse.Block.CollectionGuarantees)
            {
                blockCollectionGuarantees.Add(
                    new FlowCollectionGuarantee
                    {
                        CollectionId = collectionGuarantee.CollectionId.ByteStringToHex(),
                        Signature = collectionGuarantee.Signature.ToByteArray(),
                        SignerIds = collectionGuarantee.SignerIds.Select(s => s.ByteStringToString())
                    });
            }

            return new FlowBlock
            {
                Header = new FlowBlockHeader
                {
                    Height = blockResponse.Block.Height,
                    Id = blockResponse.Block.Id.ByteStringToHex(),
                    ParentId = blockResponse.Block.ParentId.ByteStringToHex(),
                    Timestamp = blockResponse.Block.Timestamp.ToDateTimeOffset()
                },
                Payload = new FlowBlockPayload
                {
                    Seals = blockSeals,
                    CollectionGuarantees = blockCollectionGuarantees
                }
            };
        }

        public static IEnumerable<FlowBlockEvent> ToFlowBlockEvents(this EventsResponse eventsResponse)
        {
            var blockEvents = new List<FlowBlockEvent>();

            foreach (var result in eventsResponse.Results.ToList())
            {
                var flowEvents = new List<FlowEvent>();
                foreach (var @event in result.Events)
                    flowEvents.Add(@event.ToFlowEvent());                               

                blockEvents.Add(new FlowBlockEvent
                {
                    BlockId = result.BlockId.ByteStringToHex(),
                    BlockHeight = result.BlockHeight,
                    BlockTimestamp = result.BlockTimestamp.ToDateTimeOffset(),
                    Events = flowEvents
                });
            }

            return blockEvents;
        }

        private static FlowEvent ToFlowEvent(this Protos.entities.Event @event)
        {
            return new FlowEvent
            {
                Type = @event.Type,
                EventIndex = @event.EventIndex,
                Payload = @event.Payload.ByteStringToString().Decode(),
                TransactionId = @event.TransactionId.ByteStringToHex(),
                TransactionIndex = @event.TransactionIndex
            };
        }

        public static FlowAccount ToFlowAccount(this AccountResponse accountResponse)
        {
            var flowAccount = new FlowAccount
            {
                Address = new FlowAddress(accountResponse.Account.Address.ByteStringToHex()),
                Balance = accountResponse.Account.Balance,
                Code = accountResponse.Account.Code.ByteStringToString()
            };

            foreach (var contract in accountResponse.Account.Contracts)
                flowAccount.Contracts.Add(new FlowContract { Name = contract.Key, Source = contract.Value.ByteStringToString() });

            foreach (var key in accountResponse.Account.Keys)
            {
                flowAccount.Keys.Add(
                    new FlowAccountKey
                    {
                        HashAlgorithm = (HashAlgo)key.HashAlgo,
                        SignatureAlgorithm = (SignatureAlgo)key.SignAlgo,
                        Index = key.Index,
                        PublicKey = key.PublicKey.ByteStringToHex(),
                        SequenceNumber = key.SequenceNumber,
                        Revoked = key.Revoked,
                        Weight = key.Weight
                    });
            }

            return flowAccount;
        }

        public static ExecuteScriptAtBlockHeightRequest FromFlowScript(this FlowScript script, ulong blockHeight)
        {
            var request = new ExecuteScriptAtBlockHeightRequest
            {
                Script = script.Script.StringToByteString(),
                BlockHeight = blockHeight
            };

            request.Arguments.AddRange(script.Arguments.FromArguments());

            return request;
        }

        public static ExecuteScriptAtLatestBlockRequest FromFlowScript(this FlowScript script)
        {
            var request = new ExecuteScriptAtLatestBlockRequest
            {
                Script = script.Script.StringToByteString()
            };

            request.Arguments.AddRange(script.Arguments.FromArguments());

            return request;
        }

        public static ExecuteScriptAtBlockIDRequest FromFlowScript(this FlowScript script, string blockId)
        {
            var request = new ExecuteScriptAtBlockIDRequest
            {
                Script = script.Script.StringToByteString(),
                BlockId = blockId.HexToByteString()
            };

            request.Arguments.AddRange(script.Arguments.FromArguments());

            return request;
        }

        private static IEnumerable<ByteString> FromArguments(this IEnumerable<ICadence> arguments)
        {
            return arguments.Select(x => x.Encode().StringToByteString());
        }

        public static Protos.entities.Transaction FromFlowTransaction(this FlowTransaction flowTransaction)
        {
            var tx = new Protos.entities.Transaction
            {
                Script = flowTransaction.Script.StringToByteString(),
                Payer = flowTransaction.Payer.Address.HexToByteString(),
                GasLimit = flowTransaction.GasLimit,
                ReferenceBlockId = flowTransaction.ReferenceBlockId.HexToByteString(),
                ProposalKey = flowTransaction.ProposalKey.FromFlowProposalKey()
            };

            tx.Arguments.AddRange(flowTransaction.Arguments.FromArguments());

            foreach (var authorizer in flowTransaction.Authorizers)
                tx.Authorizers.Add(authorizer.Address.HexToByteString());

            foreach (var payloadSignature in flowTransaction.PayloadSignatures)
                tx.PayloadSignatures.Add(payloadSignature.FromFlowSignature());

            foreach (var envelopeSignature in flowTransaction.EnvelopeSignatures)
                tx.EnvelopeSignatures.Add(envelopeSignature.FromFlowSignature());

            return tx;
        }

        public static IEnumerable<FlowTransaction> ToFlowTransactions(this TransactionsResponse transactions)
        {
            var result = new List<FlowTransaction>();

            foreach(var transaction in transactions.Transactions)
                result.Add(transaction.ToFlowTransaction());            

            return result;
        }

        public static FlowTransaction ToFlowTransaction(this Protos.entities.Transaction transaction)
        {
            var tx = new FlowTransaction
            {
                Script = transaction.Script.ByteStringToString(),
                Payer = new FlowAddress(transaction.Payer.ByteStringToHex()),
                GasLimit = transaction.GasLimit,
                ReferenceBlockId = transaction.ReferenceBlockId.ByteStringToHex(),
                ProposalKey = transaction.ProposalKey.ToFlowProposalKey()
            };

            foreach(var arg in transaction.Arguments)
                tx.Arguments.Add(CadenceExtensions.Decode(arg.ByteStringToString()));
            
            foreach (var authorizer in transaction.Authorizers)
                tx.Authorizers.Add(new FlowAddress(authorizer.ByteStringToHex()));
                       
            foreach(var payloadSignature in transaction.PayloadSignatures)
                tx.PayloadSignatures.Add(payloadSignature.ToFlowSignature());            

            foreach(var envelopeSignature in transaction.EnvelopeSignatures)
                tx.EnvelopeSignatures.Add(envelopeSignature.ToFlowSignature());

            return tx;
        }

        private static FlowProposalKey ToFlowProposalKey(this Protos.entities.Transaction.Types.ProposalKey proposalKey)
        {
            return new FlowProposalKey
            {
                Address = new FlowAddress(proposalKey.Address.ByteStringToHex()),
                KeyId = proposalKey.KeyId,
                SequenceNumber = proposalKey.SequenceNumber
            };
        }

        private static Protos.entities.Transaction.Types.ProposalKey FromFlowProposalKey(this FlowProposalKey flowProposalKey)
        {
            return new Protos.entities.Transaction.Types.ProposalKey
            {
                Address = flowProposalKey.Address.Address.HexToByteString(),
                KeyId = flowProposalKey.KeyId,
                SequenceNumber = flowProposalKey.SequenceNumber
            };
        }

        private static Protos.entities.Transaction.Types.Signature FromFlowSignature(this FlowSignature flowSignature)
        {
            return new Protos.entities.Transaction.Types.Signature
            {
                Address = flowSignature.Address.Address.HexToByteString(),
                KeyId = flowSignature.KeyId,
                Signature_ = flowSignature.Signature.BytesToByteString()
            };
        }

        private static FlowSignature ToFlowSignature(this Protos.entities.Transaction.Types.Signature signature)
        {
            return new FlowSignature
            {
                Address = new FlowAddress(signature.Address.ByteStringToHex()),
                KeyId = signature.KeyId,
                Signature = signature.ToByteArray()
            };
        }
    }
}
