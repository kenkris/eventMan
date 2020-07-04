using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
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
    }
}