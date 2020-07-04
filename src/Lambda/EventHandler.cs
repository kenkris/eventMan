using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Lambda.Access;
using Lambda.Models;
using Lambda.Response;
using Newtonsoft.Json;

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

        public async Task<APIGatewayProxyResponse> CreateEvent(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var eventModel = JsonConvert.DeserializeObject<EventModel>(request.Body);
                return ApiResponse.Ok(await _eventAccess.CreateEvent(eventModel));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return ApiResponse.ClientError("Bad request");
            }
        }

        public APIGatewayProxyResponse EventTesterHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return ApiResponse.Ok( _eventAccess.EventTester());
        }

        public async Task<APIGatewayProxyResponse> GetEvents(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return ApiResponse.Ok(await  _eventAccess.FetchEvents());
        }
    }
}