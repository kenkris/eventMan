using Amazon.DynamoDBv2.DataModel;

namespace Lambda.Models
{
    [DynamoDBTable("EventsDB")]
    public class BaseModel
    {
        [DynamoDBHashKey] public string PK;
        [DynamoDBHashKey, DynamoDBRangeKey]public string SKGSI;
        [DynamoDBRangeKey] public string Data;
    }
}