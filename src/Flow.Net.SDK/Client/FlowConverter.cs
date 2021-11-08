using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Protos.access;
using Google.Protobuf;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Client
{
    public static class FlowConverter
    {
        private static readonly CadenceConverter _cadenceConverter;
        static FlowConverter()
        {
            _cadenceConverter = new CadenceConverter();
        }

        public static FlowGetNetworkParametersResponse ToFlowGetNetworkParametersResponse(this GetNetworkParametersResponse getNetworkParametersResponse)
        {
            return new FlowGetNetworkParametersResponse
            {
                ChainId = getNetworkParametersResponse.ChainId
            };
        }

        public static FlowProtocolStateSnapshotResponse ToFlowProtocolStateSnapshotResponse(this ProtocolStateSnapshotResponse protocolStateSnapshotResponse)
        {
            return new FlowProtocolStateSnapshotResponse
            {
                SerializedSnapshot = protocolStateSnapshotResponse.SerializedSnapshot
            };
        }

        public static FlowExecutionResultForBlockIdResponse ToFlowExecutionResultForBlockIdResponse(this ExecutionResultForBlockIDResponse executionResultForBlockIDResponse)
        {
            var flowChunks = new List<FlowChunk>();
            foreach(var chunk in executionResultForBlockIDResponse.ExecutionResult.Chunks)
            {
                flowChunks.Add(
                    new FlowChunk
                    {
                        BlockId = chunk.BlockId,
                        EndState = chunk.EndState,
                        EventCollection = chunk.EventCollection,
                        Index = chunk.Index,
                        NumberOfTransactions = chunk.NumberOfTransactions,
                        StartState = chunk.StartState,
                        TotalComputationUsed = chunk.TotalComputationUsed
                    });
            }

            var serviceEvents = new List<FlowServiceEvent>();
            foreach (var serviceEvent in executionResultForBlockIDResponse.ExecutionResult.ServiceEvents)
            {
                serviceEvents.Add(new FlowServiceEvent
                {
                    Payload = serviceEvent.Payload,
                    Type = serviceEvent.Type
                });
            }

            return new FlowExecutionResultForBlockIdResponse
            {
                BlockId = executionResultForBlockIDResponse.ExecutionResult.BlockId,
                PreviousResultId = executionResultForBlockIDResponse.ExecutionResult.PreviousResultId,
                Chunks = flowChunks,
                ServiceEvents = serviceEvents
            };
        }

        public static FlowCollectionResponse ToFlowCollectionResponse(this CollectionResponse collectionResponse)
        {
            return new FlowCollectionResponse 
            { 
                Id = collectionResponse.Collection.Id,
                TransactionIds = collectionResponse.Collection.TransactionIds
            };
        }

        public static FlowTransactionResult ToFlowTransactionResult(this TransactionResultResponse transactionResultResponse)
        {
            var events = new List<FlowEvent>();
            foreach(var @event in transactionResultResponse.Events)
                events.Add(@event.ToFlowEvent());
            
            return new FlowTransactionResult
            {
                BlockId = transactionResultResponse.BlockId,
                ErrorMessage = transactionResultResponse.ErrorMessage,
                Status = transactionResultResponse.Status,
                StatusCode = transactionResultResponse.StatusCode,
                Events = events
            };
        }

        public static FlowTransactionResponse ToFlowTransactionResponse(this TransactionResponse transactionResponse)
        {
            var payloadSignatures = new List<FlowSignature>();
            foreach (var payloadSignature in transactionResponse.Transaction.PayloadSignatures)
            {
                payloadSignatures.Add(
                    new FlowSignature
                    {
                        Address = payloadSignature.Address,
                        KeyId = payloadSignature.KeyId,
                        Signature = payloadSignature.Signature_.ToByteArray()
                    });
            }

            var envelopeSignatures = new List<FlowSignature>();
            foreach(var envelopeSignature in transactionResponse.Transaction.EnvelopeSignatures)
            {
                envelopeSignatures.Add(
                    new FlowSignature
                    {
                        Address = envelopeSignature.Address,
                        KeyId = envelopeSignature.KeyId,
                        Signature = envelopeSignature.Signature_.ToByteArray()
                    });
            }

            var sendResponse = new FlowTransactionResponse
            {
                Script = transactionResponse.Transaction.Script.FromByteStringToString(),
                ReferenceBlockId = transactionResponse.Transaction.ReferenceBlockId,
                GasLimit = transactionResponse.Transaction.GasLimit,
                Payer = new FlowAddress(transactionResponse.Transaction.Payer),                
                Authorizers = transactionResponse.Transaction.Authorizers.Select(s => new FlowAddress(s)).ToList(),
                ProposalKey = new FlowProposalKey
                { 
                    Address = new FlowAddress(transactionResponse.Transaction.ProposalKey.Address),
                    KeyId = transactionResponse.Transaction.ProposalKey.KeyId,
                    SequenceNumber = transactionResponse.Transaction.ProposalKey.SequenceNumber
                },
                PayloadSignatures = payloadSignatures,
                EnvelopeSignatures = envelopeSignatures 
            };

            foreach(var argument in transactionResponse.Transaction.Arguments)
                sendResponse.Arguments.Add(argument.FromByteStringToString().Decode());

            return sendResponse;
        }

        public static FlowSendTransactionResponse ToFlowSendTransactionResponse(this SendTransactionResponse sendTransactionResponse)
        {
            return new FlowSendTransactionResponse
            {
                Id = sendTransactionResponse.Id
            };
        }        

        public static FlowBlockHeader ToFlowBlockHeader(this BlockHeaderResponse blockHeaderResponse)
        {
            return new FlowBlockHeader
            {
                Height = blockHeaderResponse.Block.Height,
                Id = blockHeaderResponse.Block.Id,
                ParentId = blockHeaderResponse.Block.ParentId,
                Timestamp = blockHeaderResponse.Block.Timestamp.ToDateTimeOffset(),
            };
        }

        public static FlowBlock ToFlowBlock(this BlockResponse blockResponse)
        {
            var block = new FlowBlock
            {
                Height = blockResponse.Block.Height,
                Id = blockResponse.Block.Id,
                ParentId = blockResponse.Block.ParentId,
                Timestamp = blockResponse.Block.Timestamp.ToDateTimeOffset(),
                Signatures = blockResponse.Block.Signatures.ToList()
            };

            foreach(var blockSeal in blockResponse.Block.BlockSeals)
            {
                block.BlockSeals.Add(
                    new FlowBlockSeal
                    {
                        ResultApprovalSignatures = blockSeal.ResultApprovalSignatures.ToList(),
                        BlockId = blockSeal.BlockId,
                        ExecutionReceiptSignatures = blockSeal.ExecutionReceiptSignatures.ToList(),
                        ExecutionReceiptId = blockSeal.ExecutionReceiptId
                    });
            }

            foreach (var collectionGuarantee in blockResponse.Block.CollectionGuarantees)
            {
                block.CollectionGuarantees.Add(
                    new FlowCollectionGuarantee
                    {
                        CollectionId = collectionGuarantee.CollectionId,
                        Signatures = collectionGuarantee.Signatures.ToList()
                    });
            }

            return block;
        }

        public static IEnumerable<FlowBlockEvent> ToFlowBlockEvents(this EventsResponse eventsResponse)
        {
            var blockEvents = new List<FlowBlockEvent>();

            foreach (var result in eventsResponse.Results.ToList())
            {
                var blockEvent = new FlowBlockEvent
                {
                    BlockId = result.BlockId,
                    BlockHeight = result.BlockHeight,
                    BlockTimestamp = result.BlockTimestamp.ToDateTimeOffset()
                };

                foreach (var @event in result.Events)
                    blockEvent.Events.Add(@event.ToFlowEvent());

                blockEvents.Add(blockEvent);
            }

            return blockEvents;
        }

        public static FlowEvent ToFlowEvent(this Protos.entities.Event @event)
        {
            return new FlowEvent
            {
                Type = @event.Type,
                EventIndex = @event.EventIndex,
                Payload = @event.Payload.FromByteStringToString().Decode(_cadenceConverter),
                TransactionId = @event.TransactionId,
                TransactionIndex = @event.TransactionIndex
            };
        }

        public static FlowAccount ToFlowAccount(this AccountResponse accountResponse)
        {
            var flowAccount = new FlowAccount
            {
                Address = new FlowAddress(accountResponse.Account.Address),
                Balance = accountResponse.Account.Balance,
                Code = accountResponse.Account.Code
            };

            foreach(var contract in accountResponse.Account.Contracts)
                flowAccount.Contracts.Add(new FlowContract { Name = contract.Key, Source = contract.Value.FromByteStringToString() });
            
            foreach(var key in accountResponse.Account.Keys)
            {
                flowAccount.Keys.Add(
                    new FlowAccountKey
                    {
                        HashAlgorithm = (HashAlgo)key.HashAlgo,
                        SignatureAlgorithm = (SignatureAlgo)key.SignAlgo,
                        Index = key.Index,
                        PublicKey = key.PublicKey.FromByteStringToHex(),
                        SequenceNumber = key.SequenceNumber,
                        Revoked = key.Revoked,
                        Weight = key.Weight,
                    });
            }

            return flowAccount;
        }

        public static Protos.entities.Transaction FromFlowTransaction(this FlowTransaction flowTransaction)
        {
            var tx = new Protos.entities.Transaction
            {
                Script = flowTransaction.Script.FromStringToByteString(),
                Payer = flowTransaction.Payer.Value,
                GasLimit = flowTransaction.GasLimit,
                ReferenceBlockId = flowTransaction.ReferenceBlockId,
                ProposalKey = flowTransaction.ProposalKey.FromFlowProposalKey()
            };

            if (flowTransaction.Arguments != null && flowTransaction.Arguments.Any())
            {
                foreach (var argument in flowTransaction.Arguments)
                    tx.Arguments.Add(argument.Encode().FromStringToByteString());
            }                
            
            foreach(var authorizer in flowTransaction.Authorizers)
                tx.Authorizers.Add(authorizer.Value);
            
            foreach(var payloadSignature in flowTransaction.PayloadSignatures)
                tx.PayloadSignatures.Add(payloadSignature.FromFlowSignature());

            foreach (var envelopeSignature in flowTransaction.EnvelopeSignatures)
                tx.EnvelopeSignatures.Add(envelopeSignature.FromFlowSignature());

            return tx;
        }

        public static Protos.entities.Transaction.Types.ProposalKey FromFlowProposalKey(this FlowProposalKey flowProposalKey)
        {
            return new Protos.entities.Transaction.Types.ProposalKey
            {
                Address = flowProposalKey.Address.Value,
                KeyId = flowProposalKey.KeyId,
                SequenceNumber = flowProposalKey.SequenceNumber
            };
        }

        public static Protos.entities.Transaction.Types.Signature FromFlowSignature(this FlowSignature flowSignature)
        {
            return new Protos.entities.Transaction.Types.Signature
            {
                Address = flowSignature.Address,
                KeyId = flowSignature.KeyId,
                Signature_ = flowSignature.Signature.FromByteArrayToByteString()
            };
        }
    }
}
