using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acebook.Models
{
    public class User
    {
        public User(int id, string name, string email, string password, Array posts)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Posts = posts;


        }
        public int Id { get; }


        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public Array Posts { get; }

    }
}
