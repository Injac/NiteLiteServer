using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLiteServer.Model
{
    public class User
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}