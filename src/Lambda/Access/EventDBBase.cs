using Amazon.DynamoDBv2;

namespace Lambda.Access
{
    public class EventDBBase
    {
        protected readonly AmazonDynamoDBClient DbClient = new AmazonDynamoDBClient();
        protected const string EventTable = "EventDB";
        //protected const string GSI = "SK-GSI-PK";
        //protected const string GSI_DATA_INDEX = "SK-GSI-PK-Data-index";
    }
}