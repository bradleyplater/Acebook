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
        //Connects to the mongo database and retrieves the collection which is specified by the function calling the code
        public static IMongoCollection<BsonDocument> ConnectToDB(string dbName, string collectionName)
        {
            var connectionString = "mongodb+srv://Makers1:Admin@acebook-ye6db.mongodb.net/test?retryWrites=true&w=majority";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            return database.GetCollection<BsonDocument>(collectionName);
        }

        /* Takes parameters and checks if a user exists from checking the database against those parameters
		 * returns a filled user object if a user exists and returns an empty one if not */
        public static User CheckIfUserExists(string email, string password, string collectionName)
        {

            var collection = ConnectToDB("Acebook", collectionName);

            var document = collection.Find(new BsonDocument("Email", email)).FirstOrDefault();


            if (document != null)
            {
                if ((string)document.GetValue("Password") == password)
                {

                    BsonObjectId id = (BsonObjectId)document.GetValue("_id");
                    string firstname = (string)document.GetValue("Firstname");
                    string surname = (string)document.GetValue("Surname");
                    string username = (string)document.GetValue("Username");

                    return new User(id, firstname, surname, username, email, password);
                }
                else
                {
                    return new User(new BsonObjectId(new ObjectId()), "", "", "", "", "");
                }
            }
            else
            {
                return new User(new BsonObjectId(new ObjectId()), "", "", "", "", "");
            }
        }

        //Creates a new user document within the mongo database
        public static void CreateNewUser(string Firstname, string Surname, string Email, string Username, string Password, string collectionName)
        {
            var collection = ConnectToDB("Acebook", collectionName);

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

        //Checks if the username provided exists in the database
        public static bool CheckUsernameExists(string Username, string collectionName)
        {
            var collection = ConnectToDB("Acebook", collectionName);

            var document = collection.Find(new BsonDocument("Username", Username)).FirstOrDefault();

            if (document != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Checks if the email provided exists in the database
        public static bool CheckEmailExists(string Email, string collectionName)
        {
            var collection = ConnectToDB("Acebook", collectionName);

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

        //Creates a post document within the mongo database
        public static void CreatePost(string Firstname, string Surname, string Username, string Body, DateTime Date, string collectionName)
        {
            var collection = ConnectToDB("Acebook", collectionName);

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

        //Returns a list of all posts that are stored within the mongo database
        public static List<BsonDocument> GetAllPosts(string collectionName)
        {

            var collection = ConnectToDB("Acebook", collectionName);

            var results = collection.Find(new BsonDocument()).ToList();

            return results;

        }

        public static List<BsonDocument> GetPostById(string id, string collectionName)
        {
            var collection = ConnectToDB("Acebook", collectionName);
			var newId = ObjectId.Parse(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", newId);
            var results = collection.Find(new BsonDocument( "_id", newId )).ToList();
            return results; 
        }
            


		//Adds a like to an existing post document
		public static void AddLike(BsonDocument document, string id, string collectionName)
		{
			bool userLike = false;
			var collection = ConnectToDB("Acebook", collectionName);

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

		//Adds a dislike to an existing post document
		public static void AddDislike(BsonDocument document, string id, string collectionName)
		{
			bool userDislike = false;
			var collection = ConnectToDB("Acebook", collectionName);

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

		//Removes a like from a post document
		public static void RemoveLike(BsonArray like, IMongoCollection<BsonDocument> collection, int index, FilterDefinition<BsonDocument> filter)
		{
			like.RemoveAt(index);
			var updateLike = Builders<BsonDocument>.Update.Set("Like", like);
			collection.UpdateOne(filter, updateLike);
		}

		//Removes a dislike from a post document
		public static void RemoveDislike(BsonArray dislike, IMongoCollection<BsonDocument> collection, int index, FilterDefinition<BsonDocument> filter)
		{
			dislike.RemoveAt(index);
			var updateDislike = Builders<BsonDocument>.Update.Set("Dislike", dislike);
			collection.UpdateOne(filter, updateDislike);
		}

		//Searches through the likes on a post and when the id for current user matches the id within a like it breaks and returns the id and the index
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

		//Searches through the dislikes on a post and when the id for current user matches the id within a dislike it breaks and returns the id and the index
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

		//Returns a singular document by position in the array
		public static BsonDocument SearchForDocument(int count, string collectionName)
		{
			var documents = GetAllPosts(collectionName);

			return documents[count];
		}
	}

}
