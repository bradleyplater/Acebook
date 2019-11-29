using Acebook.Models;
using NUnit.Framework;

namespace AcebookTests
{
    public class Tests
    {
        User user;

        int id;
        string name;
        string email;
        string password;
        Post[] posts = new Post[] { new Post(), new Post()};

        [SetUp]
        public void Setup()
        {
            id = 2;
            name = "John";
            email = "John@gmail.com";
            password = "Password";

            user = new User(id, name, email, password, posts);
        }

        [Test]
        public void User_Initialize_Properties()
        {
            Assert.AreEqual(user.Id, id);
            Assert.AreEqual(user.Name, name);
            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(user.Password, password);
            Assert.AreEqual(user.Posts, posts);

        }
    }
}