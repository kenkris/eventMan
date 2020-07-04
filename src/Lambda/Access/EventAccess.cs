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

            var newGuid = Guid.NewGuid();
            var eventDocument = new Document();
            eventDocument["pk"] = newGuid;
            eventDocument["skgsi"] = "EventInfo";
            eventDocument["data"] = eventModel.StartDate;
            eventDocument["endDate"] = eventModel.EndDate;
            eventDocument["name"] = eventModel.Name;
            eventDocument["venue"] = eventModel.Venue;
            eventDocument["address"] = eventModel.Address;

            try
            {
                await table.PutItemAsync(eventDocument);
                eventModel.PK = newGuid;
                return eventModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<EventModel>> FetchEvents()
        {
            var query = new QueryRequest
            {
                TableName = EventTable,
                IndexName = SKGSI_DATA_INDEX,
                KeyConditionExpression = GSI + " = :eventStatic",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":eventStatic", new AttributeValue { S = "EventInfo" } }
                },

            };
            var queryResult = await DbClient.QueryAsync(query);

            var result = new List<EventModel>();
            foreach (var item in queryResult.Items)
            {
                result.Add(new EventModel()
                {
                    PK =  new Guid(item["pk"].S),
                    SKGSI = item["skgsi"].S,
                    Data = item["data"].S, // Start date string goes here. Not sure its needed in future
                    Address = item["address"].S,
                    StartDate = Convert.ToDateTime(item["data"].S),
                    EndDate = Convert.ToDateTime(item["endDate"].S),
                    Name = item["name"].S,
                    Venue = item["venue"].S
                });
            }

            return result;
        }
    }
}