using Amazon.DynamoDBv2;

namespace Lambda.Access
{
    public class EventDBBase
    {
        protected readonly AmazonDynamoDBClient DbClient = new AmazonDynamoDBClient();
        protected const string EventTable = "EventDB";
        protected const string GSI = "skgsi";
        protected const string SKGSI_DATA_INDEX = "skgsi-data-index";
    }
}