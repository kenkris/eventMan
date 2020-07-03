using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
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

        public async Task<EventModel> CreateEvent(EventModel eventModel)
        {
            var table = Table.LoadTable(DbClient, EventTable);

            var eventDocument = new Document();
            eventDocument["pk"] = new Guid();
            eventDocument["skgsi"] = "EventInfo";
            eventDocument["data"] = eventModel.StartDate;
            eventDocument["endDate"] = eventModel.EndDate;
            eventDocument["name"] = eventModel.Name;
            eventDocument["venue"] = eventModel.Venue;
            eventDocument["address"] = eventModel.Address;

            try
            {
                var response = await table.PutItemAsync(eventDocument);
                eventModel.PK = (Guid) response["pk"];
                return eventModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
                    PK =  new Guid(item["PK"].S),
                    SKGSI = item["SK-GSI-PK"].S,
                    Data = item["Data"].S,
                    Name = item["Name"].S
                });
            }

            return result;
        }
    }
}