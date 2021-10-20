using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateAccountExample.RunAsync();
            await CreateAccountWithContractExample.RunAsync();
            await DeployUpdateDeleteContractExample.RunAsync();
            await BlockExamples.RunAsync();
            await ScriptExamples.RunAsync();
            await EventsExamples.RunAsync();
            await TransactionExamples.RunAsync();
        }
    }
}
