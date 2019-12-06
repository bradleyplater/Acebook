using Acebook.Models;
using MongoDB.Bson;
using NUnit.Framework;

namespace AcebookTests
{
    public class Tests
    {
        User user;

        BsonObjectId id;
        string Firstname;
		string Surname;
		string Username;
        string Email;
        string Password;

        [SetUp]
        public void Setup()
        {
            id = new ObjectId();
            Firstname = "John";
			Surname = "Smith";
			Username = "JohnSmith";
            Email = "John@gmail.com";
            Password = "Password";

            user = new User(id, Firstname, Surname, Username, Email, Password);
        }

        [Test]
        public void User_Initialize_Properties()
        {
            Assert.AreEqual(user.Id, id);
            Assert.AreEqual(user.Firstname, Firstname);
			Assert.AreEqual(user.Surname, Surname);
			Assert.AreEqual(user.Username, Username);
			Assert.AreEqual(user.Email, Email);
            Assert.AreEqual(user.Password, Password);
        }
    }
}