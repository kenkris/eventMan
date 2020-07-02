using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace Lambda.Response
{
    public static class ApiResponse
    {

        public static APIGatewayProxyResponse Ok(object data)
        {
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(data),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        public static APIGatewayProxyResponse ClientError(string errorMessage)
        {
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(errorMessage),
                StatusCode = 400,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        public static APIGatewayProxyResponse NotImplemented()
        {
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject("Method not implemented"),
                StatusCode = 501,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}