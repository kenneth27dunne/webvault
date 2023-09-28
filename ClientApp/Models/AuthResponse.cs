using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }

        public DateTime Created { get; set; }

        public User User { get; set; }

        public bool IsExpired()
        {
            return DateTime.Now > Created.AddSeconds(ExpiresIn - 10);
        }
    }
}
