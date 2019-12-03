using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acebook.Models
{
	public class SignUpModel
	{

		public string Firstname { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
