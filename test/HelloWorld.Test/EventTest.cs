using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Lambda;
using Newtonsoft.Json;
using Xunit;

namespace EventMan.Tests
{
    public class EventTest
    {

        [Fact]
        public async Task TestGetEvents()
        {
            var expectedResponse = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

            var request = new APIGatewayProxyRequest();
            var context = new TestLambdaContext();
            var eventHandler = new EventHandler();
            var result = await eventHandler.GetEvents(request, context);

            Assert.Equal(expectedResponse.StatusCode, result.StatusCode);
            Assert.Equal(expectedResponse.Headers, result.Headers);
            Assert.StartsWith("[", result.Body);
            Assert.EndsWith("]", result.Body);
        }


    }
}