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

        public async Task<bool> SignUpUser(UserSignUpModel userSignUpModel)
        {
            var table = Table.LoadTable(DbClient, EventTable);

            var newGuid = Guid.NewGuid();
            var eventDocument = new Document();
            eventDocument["pk"] = userSignUpModel.EventPk;
            eventDocument["skgsi"] = "EventSignup#" + userSignUpModel.UserPk;
            eventDocument["data"] = userSignUpModel.SignUpDate;
            eventDocument["cancelDate"] = userSignUpModel.CancelDate;
            eventDocument["payed"] = userSignUpModel.Payed;

            try
            {
                await table.PutItemAsync(eventDocument);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<UserModel>> FetchUserSignUpsForEvent(EventModel eventModel)
        {

            // TODO get EVENTUSER
            // TODO get USER WHERE ID IN .....

            var query = new QueryRequest
            {
                TableName = EventTable,
                KeyConditionExpression = $"pk = :eventId and begins_with({GSI}, :eventSignUpStatic)",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":eventId", new AttributeValue { S = eventModel.PK.ToString() } },
                    { ":eventSignUpStatic", new AttributeValue { S = "EventSignup#" } }
                }
            };

            var queryResult = await DbClient.QueryAsync(query);

            var result = new List<UserModel>();
            var userAccess = new UserAccess();
            foreach (var item in queryResult.Items)
            {
                var userPk = "";
                try
                {
                    userPk = item[GSI].S.Split("#")[1];
                }
                catch (Exception e)
                {
                    throw new Exception("Could not extract personId " + e.Message);
                }

                var user = await userAccess.GetUser(new UserModel{ PK = new Guid(userPk)});
                result.Add(user);
            }

            return result;


            throw new NotImplementedException();
        }
    }
}