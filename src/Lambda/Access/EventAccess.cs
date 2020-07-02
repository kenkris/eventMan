using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Lambda.Models;

namespace Lambda.Access
{
    public class EventAccess : EventDBBase
    {

        public string EventTester()
        {
            return "HEJ HEJ HEEEJ!";
        }

        public async Task<List<EventModel>> FetchAllEvents()
        {
            var query = new QueryRequest
            {
                TableName = EventTable,
                KeyConditionExpression = "SKGSI = :eventStatic",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":eventStatic", new AttributeValue { S = "Artist" } }
                },

            };
            var queryResult = await DbClient.QueryAsync(query);

            var result = new List<EventModel>();
            foreach (var item in queryResult.Items)
            {
                result.Add(new EventModel()
                {
                    PK = item["PK"].S,
                    SKGSI = item["SK-GSI-PK"].S,
                    Data = item["Data"].S,
                    Name = item["Name"].S
                });
            }

            return result;
        }
    }
}