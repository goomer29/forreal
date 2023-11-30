using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.Models
{
    public static class ErrorMessages
    {
        public const string INVALID_USERNAME = "Invalid username";

        public const string INVALID_PASSWORD = "Invalid password";
        public const string INVALID_LOGIN = "Invalid username or password";
        public const string INVALID_REPASSWORD = "this passsword not the same as the previous one";
        public const string INVALID_EMAIL = "the email must contain @";
        public const string INVALID_SIGNUP = "Invalid User/Email";
    }
}
