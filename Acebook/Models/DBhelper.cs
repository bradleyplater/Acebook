using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Acebook.Models
{
    public class DBhelper
    {

        public static IMongoCollection<BsonDocument> ConnectToDB(string dbName, string collectionName)
        {
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(MongoUrl.Create(connectionString));
            var database = client.GetDatabase(dbName);
            return database.GetCollection<BsonDocument>(collectionName);
        }

        public static User CheckIfUserExists(string email, string password)
        {
            var collection = DBhelper.ConnectToDB("Acebook", "User");
            BsonDocument person;
            if (collection.EstimatedDocumentCount() > 0)
            {
                person = collection.Find(new BsonDocument("email", email)).First();
                if ((string)person.GetValue("password") == password)
                {
                    return new User(
                        (string)person.GetValue("_id"),
                        (string)person.GetValue("name"),
                        (string)person.GetValue("email"),
                        (string)person.GetValue("password"),
                        new BsonArray()
                        );
                }
            }
            return new User(
                    "",
                    "",
                    "",
                    "",
                    new BsonArray()
                    );
            }

    }

}
