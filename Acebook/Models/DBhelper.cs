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
		public static IMongoCollection<Post> ConnectToDBForPost(string dbName, string collectionName)
		{
			var connectionString = "mongodb+srv://Makers1:Admin@acebook-ye6db.mongodb.net/test?retryWrites=true&w=majority";
			var client = new MongoClient(connectionString);
			var database = client.GetDatabase(dbName);
			return database.GetCollection<Post>(collectionName);
		}

		public static User CheckIfUserExists(string email, string password)
		{

			var collection = ConnectToDB("Acebook", "User");

			var document = collection.Find(new BsonDocument("Email", email)).FirstOrDefault();


			if(document != null)
			{
				if ((string)document.GetValue("Password") == password)
				{

					BsonObjectId id = (BsonObjectId)document.GetValue("_id");
					string firstname = (string)document.GetValue("Firstname");
					string surname = (string)document.GetValue("Surname");
					string username = (string)document.GetValue("Username");

					return new User(
						id,
						firstname,
						surname,
						username,
						email,
						password
						); 
				}
				else
				{
					return new User(
						new BsonObjectId(new ObjectId()),
						"",
						"",
						"",
						"",
						""
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
						"",
						""
						);
			}
		}

		public static void CreateNewUser(string Firstname, string Surname, string Email, string Username, string Password)
		{
			var collection = ConnectToDB("Acebook", "User");

			var document = new BsonDocument
			{
				{ "_id", new BsonObjectId(new ObjectId()) },
				{ "Firstname", Firstname },
				{ "Surname", Surname },
				{ "Email", Email },
				{ "Username", Username },
				{ "Password", Password },
			};

			collection.InsertOne(document);
		}

		public static bool CheckUsernameExists(string Username)
		{
			var collection = ConnectToDB("Acebook", "User");

			var document = collection.Find(new BsonDocument("Username", Username)).FirstOrDefault();

			if(document != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool CheckEmailExists(string Email)
		{
			var collection = ConnectToDB("Acebook", "User");

			var document = collection.Find(new BsonDocument("Email", Email)).FirstOrDefault();

			if (document != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static void CreatePost(string Firstname, string Surname, string Username, string Body, DateTime Date)
		{
			var collection = ConnectToDB("Acebook", "Posts");

			var document = new BsonDocument
			{
				{ "_id", new BsonObjectId(new ObjectId()) },
				{ "Firstname", Firstname },
				{ "Surname", Surname },
				{ "Username", Username },
				{ "Body", Body },
				{ "Date", Date },
                { "Like", new BsonArray() },
                { "Dislike", new BsonArray() }
			};

			collection.InsertOne(document);
		}

		public static List<BsonDocument> GetAllPosts()
		{

			var collection = ConnectToDB("Acebook", "Posts");

			var results = collection.Find(new BsonDocument()).ToList();

			return results;
	
		}

		public static void AddLike(BsonDocument document, string id)
		{
			bool userLike = false;
			var collection = ConnectToDB("Acebook", "Posts");

			BsonArray like = (BsonArray)document.GetValue("Like");
			BsonArray dislike = (BsonArray)document.GetValue("Dislike");
			var filter = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));
			var newDocument = new BsonDocument { { "user", id } };

			foreach (var i in like)
			{
				if (i == newDocument)
				{
					userLike = true;
				}
			}

			if (userLike != true)
			{
				like.Add(newDocument);

				Tuple<BsonValue, int> item = SearchThroughDislike(dislike, id);

				BsonValue userId = item.Item1;
				int index = item.Item2;

				if (userId != null)
				{
					RemoveDislike(dislike, collection, index, filter);
				}

				var update = Builders<BsonDocument>.Update.Set("Like", like);
				collection.UpdateOne(filter, update);
			}
			else
			{
				Tuple<BsonValue, int> item = SearchThroughLike(like, id);
				int index = item.Item2;
				RemoveLike(like, collection, index, filter);
			}

			
		}

		public static void AddDislike(BsonDocument document, string id)
		{
			bool userDislike = false;
			var collection = ConnectToDB("Acebook", "Posts");

			BsonArray like = (BsonArray)document.GetValue("Like");
			BsonArray dislike = (BsonArray)document.GetValue("Dislike");
			var filter = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));
			var newDocument = new BsonDocument { { "user", id } };

			foreach (var i in dislike)
			{
				if (i == newDocument)
				{
					userDislike = true;
				}
			}

			if (userDislike != true)
			{
				dislike.Add(newDocument);

				Tuple<BsonValue, int> item = SearchThroughLike(like, id);

				BsonValue userId = item.Item1;
				int index = item.Item2;

				if (userId != null)
				{
					RemoveLike(like, collection, index, filter);
				}

				var update = Builders<BsonDocument>.Update.Set("Dislike", dislike);
				collection.UpdateOne(filter, update);
			}
			else
			{
				Tuple<BsonValue, int> item = SearchThroughDislike(dislike, id);

				int index = item.Item2;
				
				RemoveDislike(dislike, collection, index, filter);
			}
		}

		public static void RemoveLike(BsonArray like, IMongoCollection<BsonDocument> collection, int index, FilterDefinition<BsonDocument> filter)
		{
			like.RemoveAt(index);
			var updateLike = Builders<BsonDocument>.Update.Set("Like", like);
			collection.UpdateOne(filter, updateLike);
		}

		public static void RemoveDislike(BsonArray dislike, IMongoCollection<BsonDocument> collection, int index, FilterDefinition<BsonDocument> filter)
		{
			dislike.RemoveAt(index);
			var updateDislike = Builders<BsonDocument>.Update.Set("Dislike", dislike);
			collection.UpdateOne(filter, updateDislike);
		}

		public static Tuple<BsonValue, int> SearchThroughLike(BsonArray like, string id)
		{
			var index = -1;
			BsonValue x = null;
			foreach (var i in like)
			{
				index++;
				if (i["user"] == id)
				{
					x = i;
					break;
				}
			}
			return Tuple.Create(x, index);
		}

		public static Tuple<BsonValue, int> SearchThroughDislike(BsonArray dislike, string id)
		{
			var index = -1;
			BsonValue x = null;
			foreach (var i in dislike)
			{
				index++;
				if (i["user"] == id)
				{
					x = i;
					break;
				}
			}
			return Tuple.Create(x, index);
		}

		public static BsonDocument SearchForDocument(int count)
		{
			var documents = GetAllPosts();

			return documents[count];
		}
	}

}
