using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MassTransit.Testing;

namespace Blah.MessageContracts.UnitTests
{
    public abstract class MessageContractTestBase
    {
        protected static async Task<string> PublishAndSerializeMessageViaHarness<T>(object message) where T : class
        {
            string serializedMessage;
            
            var harness = new InMemoryTestHarness();
            await harness.Start();

            try
            {
                await harness.Bus.Publish<T>(message);
                
                var messageObject = harness.Published.Select<T>().First().MessageObject;
                serializedMessage = JsonSerializer.Serialize(messageObject);
            } finally
            {
                await harness.Stop();
                harness.Dispose();
            }

            return serializedMessage;
        }
    }
}