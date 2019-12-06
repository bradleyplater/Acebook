using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using NUnit.Framework;
using Acebook.Models;
using Acebook.Controllers;



namespace AcebookTests
{
	[TestFixture]
	class DBhelperTest
	{
		string Firstname = "John";
		string Surname = "Smith";
		string Email = "John@smith.com";
		string Username = "JohnSmith";
		string Password = "Password";

		[SetUp]
		public void Setup()
		{
			var collection = DBhelper.ConnectToDB("Acebook", "PostsTest");
			var collection2 = DBhelper.ConnectToDB("Acebook", "UserTest");
			collection.DeleteMany(new BsonDocument());
			collection.DeleteMany(new BsonDocument());
		}


		[Test]
		public void DBhelper_Can_Create_User()
		{
			User user = CreateUser();
			

			Assert.AreEqual(user.Firstname, Firstname);
			Assert.AreEqual(user.Surname, Surname);
			Assert.AreEqual(user.Email, Email);
			Assert.AreEqual(user.Username, Username);
			Assert.AreEqual(user.Password, Password);
		}

		[Test]
		public void DBhelper_Can_Find_User()
		{
			User user = CreateUser();

			User foundUser = DBhelper.CheckIfUserExists(user.Email, user.Password, "UserTest");

			Assert.AreEqual(user.Firstname, foundUser.Firstname);
			Assert.AreEqual(user.Surname, foundUser.Surname);
			Assert.AreEqual(user.Email, foundUser.Email);
			Assert.AreEqual(user.Username, foundUser.Username);
			Assert.AreEqual(user.Password, foundUser.Password);
		}

		[Test]
		public void DBhelper_Can_Find_Username()
		{
			User user = CreateUser();

			bool username = DBhelper.CheckUsernameExists(user.Username, "UserTest");

			Assert.AreEqual(username, true);
		}

		[Test]
		public void DBhelper_Can_Find_Email()
		{
			User user = CreateUser();

			bool email = DBhelper.CheckEmailExists(user.Email, "UserTest");

			Assert.AreEqual(email, true);
		}

		[Test]
		public void DBhelper_Can_Create_Post()
		{
			User user = CreateUser();
			DBhelper.CreatePost(user.Firstname, user.Surname, user.Username, "body", new DateTime(), "PostsTest");
			var posts = DBhelper.GetAllPosts("PostsTest");

			Assert.AreEqual(posts.Count, 1);
		}

		public User CreateUser()
		{
			DBhelper.CreateNewUser(Firstname, Surname, Email, Username, Password, "UserTest");
			return DBhelper.CheckIfUserExists(Email, Password, "UserTest");
		}

	}
}
