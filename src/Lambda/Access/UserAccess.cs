using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Lambda.Models;

namespace Lambda.Access
{
    public class UserAccess : EventDBBase
    {
        public async Task<UserModel> CreateUser(UserModel userModel)
        {
            var table = Table.LoadTable(DbClient, EventTable);

            var newGuid = Guid.NewGuid();
            var userDocument = new Document();
            userDocument["pk"] = newGuid;
            userDocument["skgsi"] = "User";
            userDocument["data"] = userModel.Name;
            userDocument["address"] = userModel.Address;
            userDocument["phone"] = userModel.Phone;
            userDocument["email"] = userModel.Email;

            try
            {
                await table.PutItemAsync(userDocument);
                userModel.PK = newGuid;
                return userModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserModel> GetUser(UserModel userModel)
        {
            var query = new QueryRequest
            {
                TableName = EventTable,
                KeyConditionExpression = $"pk = :userId and {GSI} = :userStatic",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":userId", new AttributeValue { S = userModel.PK.ToString() } },
                    { ":userStatic", new AttributeValue { S = "User" } }
                }
            };

            var queryResult = await DbClient.QueryAsync(query);

            if (queryResult.Items.Count != 1)
                throw new Exception("Not found");

            var item = queryResult.Items.First();
            return new UserModel()
            {
                PK = new Guid(item["pk"].S),
                SKGSI = item["skgsi"].S,
                Data = item["data"].S,
                Name = item["data"].S,
                Address = item["address"].S,
                Phone = item["phone"].S,
                Email = item["email"].S
            };
        }
    }
}