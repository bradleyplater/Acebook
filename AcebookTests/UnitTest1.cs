using Acebook.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcebookTests
{
    
    public class Tests
    {
        User user;
        int id;
        string name;
        string email;
        string password;
        Post[] posts;

        [SetUp]
        public void Setup()
        {
            id = 2;
            name = "John";
            email = "John@gmail.com";
            password = "password";
            posts = new Post[] { new Post(), new Post()};

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