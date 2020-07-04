using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Lambda.Access;
using Lambda.Models;
using Lambda.Response;
using Newtonsoft.Json;

namespace Lambda
{
    public class UserHandler
    {
        private readonly UserAccess _userAccess;

        public UserHandler()
        {
            _userAccess = new UserAccess();
        }

        public async Task<APIGatewayProxyResponse> CreateUser(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var userModel = JsonConvert.DeserializeObject<UserModel>(request.Body);
                return ApiResponse.Ok(await _userAccess.CreateUser(userModel));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return ApiResponse.ClientError("Bad request");
            }
        }
    }


}