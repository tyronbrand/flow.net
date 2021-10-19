using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await BlockExample.RunAsync();
            await ScriptExample.RunAsync();
            await EventsExample.RunAsync();
            await AccountExample.RunAsync();
            await TransactionExample.RunAsync();
        }
    }
}
