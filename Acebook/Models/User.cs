using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acebook.Models
{
    public class User
    {
        public User(BsonObjectId id, string firstname, string surname, string username, string email, string password, BsonArray posts)
        {
            Id = id;
			Firstname = firstname;
			Surname = surname;
            Username = username;
            Email = email;
            Password = password;
            Posts = posts;
        }
        public BsonObjectId Id { get; }
        public string Firstname { get; }
		public string Surname { get; }
		public string Username { get; }
        public string Email { get; }
        public string Password { get; }
        public BsonArray Posts { get; }

    }
}
