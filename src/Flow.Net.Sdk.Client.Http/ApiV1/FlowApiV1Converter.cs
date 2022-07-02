using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flow.Net.Sdk.Client.Http.ApiV1
{
    public static class FlowApiV1Converter
    {
        public static FlowAccount ToFlowAccount(this Account account)
        {
            return new FlowAccount
            {
                Address = new FlowAddress(account.Address),
                Balance = decimal.Parse(account.Balance),
                Code = null,
                Contracts = account.Contracts?.ToFlowContract(),
                Keys = account.Keys?.ToFlowAccountKey()
            };
        }

        public static FlowCollection ToFlowCollection(this Collection collection)
        {
            var flowCollection = new FlowCollection
            {
                Id = collection.Id
            };

            if (collection.Transactions != null)
                flowCollection.TransactionIds = collection.Transactions?.Select(s => s.ToFlowTransactionId()).ToList();

            return flowCollection;
        }

        public static IList<FlowBlock> ToFlowBlock(this ICollection<Block> blocks)
        {
            var flowBlock = new List<FlowBlock>();

            foreach (var block in blocks)
            {
                flowBlock.Add(new FlowBlock
                {
                    Header = new FlowBlockHeader
                    {
                        Height = ulong.Parse(block.Header.Height),
                        Id = block.Header.Id,
                        ParentId = block.Header.Parent_id,
                        Timestamp = block.Header.Timestamp
                    },
                    Payload = new FlowBlockPayload
                    {
                        CollectionGuarantees = block.Payload?.Collection_guarantees.FromCollectionGuarantees(),
                        Seals = block.Payload?.Block_seals.FromBlockSeals(),
                    }
                });
            }
            return flowBlock;
        }

        public static IList<FlowBlockSeal> FromBlockSeals(this ICollection<BlockSeal> blockSeals)
        {
            var flowBlockSeal = new List<FlowBlockSeal>();
            foreach (var seal in blockSeals)
            {
                flowBlockSeal.Add(new FlowBlockSeal
                {
                    BlockId = seal.Block_id,
                    ResultId = seal.Result_id
                });
            }
            return flowBlockSeal;
        }

        public static IList<FlowCollectionGuarantee> FromCollectionGuarantees(this ICollection<CollectionGuarantee> collectionGuarantees)
        {
            var flowCollectionGuarantees = new List<FlowCollectionGuarantee>();
            foreach (var seal in collectionGuarantees)
            {
                flowCollectionGuarantees.Add(new FlowCollectionGuarantee
                {
                    CollectionId = seal.Collection_id,
                });
            }
            return flowCollectionGuarantees;
        }

        public static IList<FlowContract> ToFlowContract(this IDictionary<string, byte[]> contracts)
        {
            var flowContracts = new List<FlowContract>();
            foreach (var contract in contracts)
            {
                flowContracts.Add(new FlowContract
                {
                    Name = contract.Key,
                    Source = Encoding.UTF8.GetString(contract.Value)
                });
            }
            return flowContracts;
        }

        public static IList<FlowAccountKey> ToFlowAccountKey(this ICollection<AccountPublicKey> accountPublicKey)
        {
            var flowAccountKeys = new List<FlowAccountKey>();
            foreach (var key in accountPublicKey)
            {
                flowAccountKeys.Add(new FlowAccountKey
                {
                    Index = uint.Parse(key.Index),
                    PublicKey = key.Public_key.RemoveHexPrefix(),
                    SequenceNumber = ulong.Parse(key.Sequence_number),
                    Revoked = key.Revoked,
                    Weight = uint.Parse(key.Weight),
                    HashAlgorithm = (HashAlgo)Enum.Parse(typeof(HashAlgo), key.Hashing_algorithm.ToString()),
                    SignatureAlgorithm = (SignatureAlgo)Enum.Parse(typeof(SignatureAlgo), key.Signing_algorithm.ToString()),
                });
            }
            return flowAccountKeys;
        }

        public static ScriptBody FromFlowScript(this FlowScript flowScript)
        {
            return new ScriptBody
            {
                Script = Encoding.UTF8.GetBytes(flowScript.Script),
                Arguments = flowScript.Arguments.FromArguments()
            };
        }

        public static FlowTransactionId ToFlowTransactionId(this Transaction transaction)
        {
            return new FlowTransactionId
            {
                Id = transaction.Id
            };
        }

        public static FlowTransactionResult ToFlowTransactionResult(this Transaction transaction)
        {
            var events = transaction.Result.Events.Select(@event => @event.ToFlowEvent()).ToList();

            return new FlowTransactionResult
            {
                BlockId = transaction.Reference_block_id,
                ErrorMessage = transaction.Result.Error_message,
                Status = (Core.TransactionStatus)Enum.Parse(typeof(Core.TransactionStatus), transaction.Result.Status.ToString()),
                StatusCode = transaction.Result.Status_code,
                Events = events
            };
        }

        public static FlowEvent ToFlowEvent(this Event @event)
        {
            return new FlowEvent
            {
                EventIndex = uint.Parse(@event.Event_index),
                TransactionId = @event.Transaction_id,
                TransactionIndex = uint.Parse(@event.Transaction_index),
                Type = @event.Type,
                Payload = Encoding.UTF8.GetString(@event.Payload).Decode(),
            };
        }

        public static FlowTransactionResponse ToFlowTransactionResponse(this Transaction transaction)
        {
            var payloadSignatures = transaction.Payload_signatures.Select(payloadSignature =>
                new FlowSignature
                {
                    Address = new FlowAddress(payloadSignature.Address),
                    KeyId = uint.Parse(payloadSignature.Key_index),
                    Signature = payloadSignature.Signature
                }).ToList();

            var envelopeSignatures = transaction.Envelope_signatures.Select(envelopeSignature =>
                new FlowSignature
                {
                    Address = new FlowAddress(envelopeSignature.Address),
                    KeyId = uint.Parse(envelopeSignature.Key_index),
                    Signature = envelopeSignature.Signature
                }).ToList();

            var sendResponse = new FlowTransactionResponse
            {
                Script = Encoding.UTF8.GetString(transaction.Script),
                ReferenceBlockId = transaction.Reference_block_id,
                GasLimit = ulong.Parse(transaction.Gas_limit),
                Payer = new FlowAddress(transaction.Payer),
                Authorizers = transaction.Authorizers.Select(s => new FlowAddress(s)).ToList(),
                ProposalKey = new FlowProposalKey
                {
                    Address = new FlowAddress(transaction.Proposal_key.Address),
                    KeyId = uint.Parse(transaction.Proposal_key.Key_index),
                    SequenceNumber = ulong.Parse(transaction.Proposal_key.Sequence_number)
                },
                PayloadSignatures = payloadSignatures,
                EnvelopeSignatures = envelopeSignatures
            };

            foreach (var argument in transaction.Arguments)
                sendResponse.Arguments.Add(Encoding.UTF8.GetString(argument).Decode());

            return sendResponse;
        }

        public static TransactionBody FromFlowTransaction(this FlowTransaction flowTransaction)
        {
            var tx = new TransactionBody
            {
                Script = Encoding.UTF8.GetBytes(flowTransaction.Script),
                Payer = flowTransaction.Payer.Address,
                Gas_limit = flowTransaction.GasLimit.ToString(),
                Reference_block_id = flowTransaction.ReferenceBlockId,
                Proposal_key = flowTransaction.ProposalKey.FromFlowProposalKey()
            };

            foreach (var argumnet in flowTransaction.Arguments.FromArguments())
                tx.Arguments.Add(argumnet);

            foreach (var authorizer in flowTransaction.Authorizers)
                tx.Authorizers.Add(authorizer.Address);

            foreach (var payloadSignature in flowTransaction.PayloadSignatures)
                tx.Payload_signatures.Add(payloadSignature.FromFlowSignature());

            foreach (var envelopeSignature in flowTransaction.EnvelopeSignatures)
                tx.Envelope_signatures.Add(envelopeSignature.FromFlowSignature());

            return tx;
        }

        public static TransactionSignature FromFlowSignature(this FlowSignature flowSignature)
        {
            return new TransactionSignature
            {
                Address = flowSignature.Address.Address,
                Key_index = flowSignature.KeyId.ToString(),
                Signature = flowSignature.Signature,
            };
        }

        public static ProposalKey FromFlowProposalKey(this FlowProposalKey flowProposalKey)
        {
            return new ProposalKey
            {
                Address = flowProposalKey.Address.Address,
                Key_index = flowProposalKey.KeyId.ToString(),
                Sequence_number = flowProposalKey.SequenceNumber.ToString()
            };
        }

        public static IList<FlowExecutionResult> ToFlowExecutionResult(this ICollection<ExecutionResult> executionResults)
        {
            var flowExecutionResults = new List<FlowExecutionResult>();

            foreach (var result in executionResults)
            {
                flowExecutionResults.Add(new FlowExecutionResult
                {
                    BlockId = result.Block_id,
                    PreviousResultId = result.Previous_result_id,
                    Chunks = result.Chunks.Select(chunk =>
                        new FlowChunk
                        {
                            BlockId = chunk.Block_id,
                            EndState = chunk.End_state,
                            EventCollection = chunk.Event_collection,
                            Index = ulong.Parse(chunk.Index),
                            NumberOfTransactions = uint.Parse(chunk.Number_of_transactions),
                            StartState = chunk.Start_state,
                            TotalComputationUsed = ulong.Parse(chunk.Total_computation_used)
                        }).ToList(),
                    ServiceEvents = result.Events.Select(@event =>
                        new FlowServiceEvent
                        {
                            Payload = @event.Payload,
                            Type = @event.Type
                        }).ToList()
                });
            }
            return flowExecutionResults;
        }

        public static IEnumerable<FlowBlockEvent> ToFlowBlockEvent(this ICollection<BlockEvents> blockEvents)
        {
            var flowBlockEvents = new List<FlowBlockEvent>();

            foreach (var block in blockEvents)
            {
                flowBlockEvents.Add(new FlowBlockEvent
                {
                    BlockId = block.Block_id,
                    BlockHeight = ulong.Parse(block.Block_height),
                    BlockTimestamp = block.Block_timestamp,
                    Events = block.Events.ToFlowEvent()
                });
            }

            return flowBlockEvents;
        }

        public static IEnumerable<FlowEvent> ToFlowEvent(this ICollection<Event> events)
        {
            var flowEvents = new List<FlowEvent>();

            foreach (var @event in events)
            {
                flowEvents.Add(new FlowEvent
                {
                    EventIndex = uint.Parse(@event.Event_index),
                    Payload = Encoding.UTF8.GetString(@event.Payload).Decode(),
                    TransactionId = @event.Transaction_id,
                    TransactionIndex = uint.Parse(@event.Transaction_index),
                    Type = @event.Type
                });
            }

            return flowEvents;
        }

        private static ICollection<byte[]> FromArguments(this IEnumerable<ICadence> cadenceArguments)
        {
            return cadenceArguments.Select(x => Encoding.UTF8.GetBytes(x.Encode())).ToList();
        }
    }
}
