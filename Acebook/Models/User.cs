using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acebook.Models
{
    public class User
    {
        public User(string id, string name, string email, string password, BsonArray posts)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Posts = posts;
        }
        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public BsonArray Posts { get; }

    }
}
