using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Lambda.Access;
using Lambda.Response;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Lambda
{
    public class EventHandler
    {
        private EventAccess _eventAccess;
        public EventHandler()
        {
            _eventAccess = new EventAccess();
        }

        public async Task<APIGatewayProxyResponse> EventTesterHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return ApiResponse.Ok( _eventAccess.EventTester());
        }

        public async Task<APIGatewayProxyResponse> GetAllEvents(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return ApiResponse.Ok(await  _eventAccess.FetchAllEvents());
        }
    }
}