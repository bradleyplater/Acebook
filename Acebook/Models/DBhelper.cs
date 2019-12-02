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
			var connectionString = "mongodb+srv://Makers1:Admin@acebook-ye6db.mongodb.net/test?retryWrites=true&w=majority";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
			Console.WriteLine("I got here 1");
			return database.GetCollection<BsonDocument>(collectionName);
        }

		public static User CheckIfUserExists(string email, string password)
		{
		
			Console.WriteLine("I havent connected to db yet");

			var collection = ConnectToDB("Acebook", "User");
			Console.WriteLine(collection);

			var document = collection.Find(new BsonDocument("Email", email)).FirstOrDefault();


			if(document != null)
			{
				if ((string)document.GetValue("Password") == password)
				{

					BsonObjectId id = (BsonObjectId)document.GetValue("_id");
					string name = (string)document.GetValue("Username");
					BsonArray posts = (BsonArray)document.GetValue("Posts");

					return new User(
						id,
						name,
						email,
						password,
						posts
						);
				}
				else
				{
					return new User(
						new BsonObjectId(new ObjectId()),
						"",
						"",
						"",
						new BsonArray()
						);
				}
			}
			else
			{
				return new User(
						new BsonObjectId(new ObjectId()),
						"",
						"",
						"",
						new BsonArray()
						);
			}
		}

    }

}
