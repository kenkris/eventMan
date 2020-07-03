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


            // TODO
            try
            {
                var eventModel = JsonConvert.DeserializeObject<EventModel>(request.Body);
                Console.WriteLine(eventModel.Name);
                Console.WriteLine(eventModel.PK);
                return ApiResponse.Ok("O K ");
            }
            catch
            {
                return ApiResponse.ClientError("Bad request");
            }



            return ApiResponse.NotImplemented();
            // return ApiResponse.Ok(_eventAccess.CreateEvent());
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