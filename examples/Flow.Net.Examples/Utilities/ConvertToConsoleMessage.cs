using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Examples.Utilities
{
    public class ConvertToConsoleMessage
    {
        public static void WriteSuccessMessage(FlowBlock flowBlock)
        {
            ColorConsole.WriteSuccess("\nSuccess:");

            // write something nice to console
            ColorConsole.WriteSuccess("\n" +
                JsonConvert.SerializeObject(
                    new
                    {
                        flowBlock.Height,
                        Id = flowBlock.Id.FromByteStringToHex(),
                        ParentId = flowBlock.ParentId.FromByteStringToHex(),
                        Signatures = flowBlock.Signatures.Select(s => s.FromByteStringToHex()).ToList(),
                        flowBlock.Timestamp,
                        flowBlock.BlockSeals,
                        CollectionGuarantees = flowBlock.CollectionGuarantees.Select(s => new { CollectionId = s.CollectionId.FromByteStringToHex(), Signatures = s.Signatures.Select(g => g.FromByteStringToHex()).ToList() }).ToList()
                    },
                    Formatting.Indented));
        }

        public static void WriteSuccessMessage(FlowBlockHeader flowBlockHeader)
        {
            ColorConsole.WriteSuccess("\nSuccess:");

            // write something nice to console
            ColorConsole.WriteSuccess("\n" +
                JsonConvert.SerializeObject(
                    new
                    {
                        flowBlockHeader.Height,
                        Id = flowBlockHeader.Id.FromByteStringToHex(),
                        ParentId = flowBlockHeader.ParentId.FromByteStringToHex(),
                        flowBlockHeader.Timestamp
                    },
                    Formatting.Indented));
        }

        public static void WriteSuccessMessage(FlowTransactionResult flowTransactionResult)
        {
            ColorConsole.WriteSuccess("\nSuccess:");

            // write something nice to console
            ColorConsole.WriteSuccess("Transaction result:\n" +
                JsonConvert.SerializeObject(
                    new
                    {
                        BlockId = flowTransactionResult.BlockId.FromByteStringToString(),
                        flowTransactionResult.ErrorMessage,
                        flowTransactionResult.Status,
                        flowTransactionResult.StatusCode
                    },
                    Formatting.Indented));
        }

        public static void WriteSuccessMessage(IEnumerable<FlowBlockEvent> flowBlockEvents)
        {
            ColorConsole.WriteSuccess("\nSuccess:");

            var printEvents = new List<dynamic>();

            foreach(var blockEvent in flowBlockEvents)
            {
                printEvents.Add(
                    new
                    {
                        blockEvent.BlockHeight,
                        BlockId = blockEvent.BlockId.FromByteStringToHex(),
                        blockEvent.BlockTimestamp,
                        Events = blockEvent.Events.Select(s => new { s.EventIndex, s.Payload, TransactionId= s.TransactionId.FromByteStringToHex(), s.TransactionIndex, s.Type }).ToList()
                    }); ;
            }

            // write something nice to console
            ColorConsole.WriteSuccess("Transaction result:\n" +
                JsonConvert.SerializeObject(printEvents, Formatting.Indented));
        }

        public static void WriteInfoMessage(FlowContract flowContract)
        {
            ColorConsole.WriteInfo($"\ncontract:\n{JsonConvert.SerializeObject(flowContract, Formatting.Indented)}");
        }

        public static void WriteInfoMessage(FlowAccountKey flowAccountKey)
        {
            ColorConsole.WriteInfo($"\nnew account key:\n{JsonConvert.SerializeObject(flowAccountKey, Formatting.Indented)}");
        }

        public static void WriteInfoMessage(FlowTransaction tx)
        {
            var printableTx = new
            {
                tx.Script,
                ReferenceBlockId = tx.ReferenceBlockId.FromByteStringToHex(),
                tx.GasLimit,
                Payer = tx.Payer.FromByteStringToHex(),
                Authorizers = tx.Authorizers.Select(s => s.FromByteStringToHex()).ToList(),
                ProposalKey = new { Address = tx.ProposalKey.Address.FromByteStringToHex(), tx.ProposalKey.KeyId, tx.ProposalKey.SequenceNumber },
                Arguments = tx.Arguments.Select(s => s.FromByteStringToString()).ToList(),
                PayloadSignatures = tx.PayloadSignatures.Select(s => new { Address = s.Address.FromByteStringToHex(), s.KeyId, Signature = s.Signature.FromByteArrayToHex() }).ToList(),
                EnvelopeSignatures = tx.EnvelopeSignatures.Select(s => new { Address = s.Address.FromByteStringToHex(), s.KeyId, Signature = s.Signature.FromByteArrayToHex() }).ToList()
            };

            ColorConsole.WriteInfo($"\ntransaction:\n{JsonConvert.SerializeObject(printableTx, Formatting.Indented)}");
        }

        public static void WriteSuccessMessage(ICadence cadence)
        {
            ColorConsole.WriteSuccess("\nSuccess:");

            // write something nice to console
            ColorConsole.WriteSuccess("Transaction result:\n" +
                JsonConvert.SerializeObject(cadence, Formatting.Indented));
        }

        public static void WriteInfoMessage(List<ICadence> cadenceArguments)
        {
            ColorConsole.WriteInfo($"\narguments:\n{JsonConvert.SerializeObject(cadenceArguments, Formatting.Indented)}");
        }
    }
}
